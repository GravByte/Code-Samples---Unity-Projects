using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OD.Effect.HDRP;

public class PlayerScanScript : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField]
    bool _showLogs;

    [SerializeField]
    string _prefix;

    [Header("Scan")]
    ScanAnimation _scanAnimation;

    [SerializeField]
    private GameObject _scanSphere;

    [SerializeField]
    private GameObject[] _activeNotes;

    [SerializeField]
    GameObject _player;

    [SerializeField]
    AudioSource _scanAudio;

    [SerializeField]
    bool _cooldown;

    [SerializeField]
    float _coolTime = 5f;

    private float _coolTimer = -1f;

    [SerializeField]
    private Text _scanText;

    private bool _scanNotes;
    private bool _noteColTag;

    // Start is called before the first frame update
    void Start()
    {
        _scanAnimation = _player.GetComponent<ScanAnimation>();
        _scanAnimation.customInput = true;

        _cooldown = false;
        _scanNotes = false;

        _scanText.text = "Press C to Scan";

        for (int i = 0; i < _activeNotes.Length; i++)
        {
            _activeNotes[i].GetComponent<Renderer>().enabled = false;
        }
    }

    private void Update()
    {
        if (_coolTimer != -1)
        {
            if (_coolTimer > 0)
            {
                _coolTimer -= Time.deltaTime;
                _scanText.text = "Scan Cooldown";

            }
            else
            {
                _coolTimer = -1f;
                _cooldown = false;
                _scanText.text = "Press C to Scan";
                Log("Cooldown Ended");
            }
        }

        if (_cooldown == false && Input.GetKeyDown(KeyCode.C))
        {
            _scanAnimation.StartScan();
            _scanAudio.Play();
            Instantiate(_scanSphere, _player.transform.position, _player.transform.rotation);

            _coolTimer = _coolTime;
            _cooldown = true;
            Log("Cooldown Active");
        }



    }

    private void FixedUpdate()
    {
        if (_noteColTag == true)
        {
            if (_cooldown == false && Input.GetKeyDown(KeyCode.C))
            {
                _scanNotes = true;
                Log("ScanNotes: " + _scanNotes);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "noteCollider":
                _noteColTag = true;
                Log("NoteColTag: " + _noteColTag);
                
                if (_scanNotes == true)
                {
                    _scanNotes = false;
                    Log("ScanNotes: " + _scanNotes);

                    if (!other.gameObject.GetComponentInChildren<Renderer>())
                    {
                        Debug.LogWarning("No Renderer on Note Found");
                    }
                    else
                    {
                        other.gameObject.GetComponentInChildren<Renderer>().enabled = true;
                        other.gameObject.GetComponentInChildren<NoteScript>().IsScanned = true;
                    }
                    //------ Note isScanned = true;
                    
                    Log("Note Visible");
                }

                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "noteCollider":
                _noteColTag = false;
                Log("NoteColTag: " + _noteColTag);

                break;
        }
    }

    //--------- Enables or disables logging across script and adds prefix
    public void Log(object message)
    {
        if (_showLogs)
            Debug.Log(_prefix + message);
    }
}
