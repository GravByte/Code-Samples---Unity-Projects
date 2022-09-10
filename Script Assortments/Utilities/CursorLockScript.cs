using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CursorLockScript : MonoBehaviour
{
    // Settings for logging in editor
    [Header("Settings")]
    [SerializeField]
    private bool showLogs;

    [SerializeField] 
    private string prefix;
    
    // holds lock values to manage the Windows cursor
    private CursorLockMode _lockMode;

    private void Awake()
    {
		
		// Immediately locks the cursor when the game starts.
		// Can be switched if menu with no splash
        LockCursor();
    }

    private void FixedUpdate()
    {
		// Ensures the other state is true and input is triggered before switching state
        if (Input.GetKeyDown(KeyCode.Q) && _lockMode == CursorLockMode.Locked){
            UnlockCursor();
            
        }
        else if (Input.GetKeyDown(KeyCode.Q) && _lockMode == CursorLockMode.Confined)
        {
            LockCursor();
            
        }
    }

    // can be called by scripts to lock the cursor
    public void LockCursor()
    {
        //Locks the cursor
        _lockMode = CursorLockMode.Locked;
        Cursor.lockState = _lockMode;
        Log("Locked Cursor");
    }

    public void UnlockCursor()
    {
        //Unlocks the cursor
        _lockMode = CursorLockMode.Confined;
        Cursor.lockState = _lockMode;
        Log("Unlocked Cursor");
    }
    
	// Basic log method to enable/disable logging in editor per script
	// Identifies source script in console
    private void Log(object message)
    {
        if (showLogs)
            Debug.Log(prefix + message);
    }
}
