using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerPickupManager : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField]
    private bool _showLogs;

    [SerializeField] private string _prefix;

    [SerializeField] private float _loadDelay = 1f;

    [Header("Animation")]
    [SerializeField]
    private Animator _playerAnimator;

    [Header("Notes")]
    public float Notes = 0;

    [SerializeField]
    private Text _notesText;

    [SerializeField]
    private AudioSource _pickUp;

    [SerializeField]
    private GameObject _noteInteractText;

    [SerializeField]
    private GameObject _notePanel;

    [SerializeField]
    private GameObject _noteHand;

    [Header("Locks & Key")]
    [SerializeField]
    private GameObject _lockControl;

    [SerializeField]
    private GameObject _keyObtainedText;

    [SerializeField]
    private GameObject _interactText;

    [Header("Scanning")]
    [SerializeField]
    private GameObject _scanText;

    [SerializeField]
    private Collider _scanTrigger;

    private bool _showPanel = false;

    public bool BoxUnlocked = false;

    public bool keyObtained = false;

    [SerializeField]
    private string _templeSceneName;

    [SerializeField]
    private GameObject _doorUnavailableText;

    private SceneLoader.Scene _loadScene;

    private void Awake()
    {
        //Set gameobjects to false on start
        _interactText.SetActive(false);
        _noteInteractText.SetActive(false);
        _notePanel.SetActive(false);
        _lockControl.SetActive(false);
        _keyObtainedText.SetActive(false);
        _doorUnavailableText.SetActive(false);

        _noteHand.SetActive(false);

        //gets the character animator component
        _playerAnimator = _playerAnimator.GetComponent<Animator>();

        //Physics.IgnoreCollision(scanTrigger.GetComponent<Collider>(), GetComponent<Collider>());
    }

    // Update is called once per frame
    private void Update()
    {

        BoxUnlocked = _lockControl.GetComponent<LockControl>().BoxUnlocked;

        //----------- when 4 notes have been picked up, the note panel displaying the clues for opening the locks is displayed
        if (Input.GetKeyDown(KeyCode.E) && Notes >= 4 && BoxUnlocked == false)
        {
            switch (_showPanel)
            {
                case false:
                    _notePanel.SetActive(true);
                    _showPanel = true;
                    break;
                case true:
                    _notePanel.SetActive(false);
                    _showPanel = false;
                    break;
            }
        }

        if (BoxUnlocked == true)
        {
            KeyObtained();
        }
    }

    private void KeyObtained()
    {
        _noteHand.SetActive(false);

        keyObtained = true;
        _keyObtainedText.SetActive(true);
        //Log("Key Obtained");
    }

    //----------- Trigger Collider with tag and switch system
    private void OnTriggerStay(Collider other)
    {

        switch (other.gameObject.tag)
        {
            case "Note":
                
                //Checks if note has been scanned so it cannot be picked up without scanning
                if (other.gameObject.GetComponent<NoteScript>().IsScanned == true) //temp false
                {
                    //Debug.Log("Can Pickup Note");
                    _noteInteractText.SetActive(true);

                    if (Input.GetButtonDown("PickUp"))
                    {
                        _playerAnimator.Play("Pickup", 0, 0.1f);
                        _noteHand.SetActive(true);
                        _pickUp.Play();

                        //Destroy(other.gameObject);
                        other.gameObject.SetActive(false);
                        Notes ++;
                        _notesText.text = ("Notes: " + Notes);
                        _noteInteractText.SetActive(false);
                        Log("Picked up Note");

                        if (Notes >= 4)
                        {
                            _notesText.text = ("Press E to read note");
                            _scanText.SetActive(false);
                            _scanTrigger.enabled = false;
                            _noteHand.SetActive(true);
                        }
                    }
                }
                else
                {
                    //Debug.Log(other.gameObject + " isScanned = false");

                }

                break;

            case "LockTriggerZone":
                if (Notes >= 4)
                {
                    _lockControl.SetActive(true);
                }


                break;

            case "TempleDoorTriggerZone":
                if (keyObtained == true)
                {
                    
                    _interactText.SetActive(true);
                    _doorUnavailableText.SetActive(false);

                    if (keyObtained = true && Input.GetKeyDown(KeyCode.F))
                    {
                        _interactText.SetActive(false);
                        Log("Enter Temple");
                        StartCoroutine(DelaySceneLoad());
                        _loadScene = SceneLoader.Scene.AnimaticScene;
                        Log("Loaded Scene");
                    }

                }
                if (!keyObtained)
                {
                    _doorUnavailableText.SetActive(true);
                }
                break;

            //----------- Default Case
            default:
                break;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Note":

                _noteInteractText.SetActive(false);

                break;

            case "TempleDoorTriggerZone":
                if (keyObtained == true)
                {

                    _interactText.SetActive(false);
                    
                }
                _doorUnavailableText.SetActive(false);
                break;

            //----------- Default Case
            default:
                break;
        }
    }

    //----------- Delays loading the scene
    private IEnumerator DelaySceneLoad()
    {
        yield return new WaitForSeconds(_loadDelay);
        Log("LoadDelay: " + _loadDelay);
        SceneLoader.Load(_loadScene);
    }

    //--------- Enables or disables logging across script and adds prefix
    public void Log(object message)
    {
        if (_showLogs)
            Debug.Log(_prefix + message);
    }

}
