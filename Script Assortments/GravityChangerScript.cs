using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GravityChangerScript : MonoBehaviour
{
 
    // Settings for logging in editor
    [Header("Settings")]
    [SerializeField]
    private bool showLogs;

    [SerializeField] 
    private string prefix;

    [Header("Gravity")]

    [SerializeField]
	private float gravity = -9.81f;
	
	[SerializeField] private GameObject gravPad;
	[SerializeField] private Collider gravPadCol;
    private bool _isGravBtnPressed;
  
    
    // Update is called once per frame
    private void Update()
	{
    	
		// Receives either test input or button/pad state then calls method to reverse the gravity
    	
        if (Input.GetKey(KeyCode.G) || _isGravBtnPressed)
        {
            ReverseGravity();
            Log("Gravity changed to: " + Physics.gravity);
        }
        else
        {
	        Physics.gravity = new Vector3(0, gravity, 0); // Resets gravity back to normal
        }
        
    }

	// Self explanatory. Inverses the gravity float. Only affects physics objects.
    public void ReverseGravity()
    {
        Physics.gravity = new Vector3(0f, -gravity, 0f);
    }

	// Script is on the player object so when player enters the button collider, switches the value of _isGravBtnPressed
    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "GravBtn":
                _isGravBtnPressed = true;
                Log("Button pressed");
                break;
        }
    }

	// When the player is no longer in the collider, gravity is set back to normal 
    private void OnTriggerExit(Collider other)
    {
	    //interactTextGo.SetActive(false);
        switch (other.gameObject.tag)
        {
            case "GravBtn":
                _isGravBtnPressed = false;
                Log("Button released");
                break;
        }
        
    }

	// Basic log method to enable/disable logging in editor per script
	// Identifies source script in console
    private void Log(object message)
    {
        if (showLogs)
            Debug.Log(prefix + message);
    }
    
}
