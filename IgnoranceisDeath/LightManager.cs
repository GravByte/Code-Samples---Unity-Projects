using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightManager : MonoBehaviour
{
    
    public GameObject[] brLights;
    public GameObject[] lrLights;
    public GameObject[] ktchnLights;
    public GameObject[] windowLights;

    private GameObject _enemy;
    private GameObject _chosenWindow;

    private AudioSource lightningSFX;
    
    // Start is called before the first frame update
    void Awake()
    {
        // Gets all of the Window Lights & the enemy object
        windowLights = GameObject.FindGameObjectsWithTag("Window");
        _enemy = GameObject.Find("ENEMY");

        //Disables all window lights in the scene
        foreach (var i in windowLights)
        {
            i.GetComponent<Light2D>().enabled = false;
        }

        // Find and disable all bedroom lights in the scene
        brLights = GameObject.FindGameObjectsWithTag("BRLights");
        
        foreach (var i in brLights)
        {
            i.GetComponent<Light2D>().enabled = false;
        }
        
        // Find and disable all living room lights in the scene
        lrLights = GameObject.FindGameObjectsWithTag("LRLights");
        
        foreach (var i in lrLights)
        {
            i.GetComponent<Light2D>().enabled = false;
        }
        
        // Find and disable all kitchen lights in the scene
        ktchnLights = GameObject.FindGameObjectsWithTag("KtchnLights");
        
        foreach (var i in ktchnLights)
        {
            i.GetComponent<Light2D>().enabled = false;
        }
    }
    
    // Flicker windowLights with lightning flash (Called when enemy moves to a new window)
    public IEnumerator Lightning(GameObject chosenWindow)
    {    
        // play lightning sound effect in location of chosen window
        lightningSFX = chosenWindow.GetComponent<AudioSource>();
        lightningSFX.Play();
        
        // Repeat flashing windowlights i number of times for specified window
        int i;
        for (i = 0; i < 5; i++)
        {
            chosenWindow.GetComponent<Light2D>().enabled = true;
            yield return new WaitForSeconds(0.1f);
            chosenWindow.GetComponent<Light2D>().enabled = false;
            yield return new WaitForSeconds(0.1f);
        }

    }