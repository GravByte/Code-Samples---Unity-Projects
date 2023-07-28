using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectEnableTrigger : MonoBehaviour
{
    [Header("Game Object to Enable")]
    [SerializeField] private GameObject[] triggeredObjects;
    [SerializeField] private GameObject[] disabledObjects;
    
    [Header("Audio")]
    [SerializeField] private AudioClip audioClip;
    
    private void Awake()
    {
        //---- When the game starts, the other UI game object is disabled
        foreach (GameObject triggeredObject in triggeredObjects)
        {
            triggeredObject.SetActive(false);
        }
        
    }

    //---- When the player clicks on the object, the other UI game objects are enabled/disabled
    private void OnMouseDown()
    {
        if (UIManager.Instance.isUIPanelOpen == false)
        {
            foreach (GameObject triggeredObject in triggeredObjects)
            {
                triggeredObject.SetActive(true);
            }
            
            foreach (GameObject disabledObject in disabledObjects)
            {
                disabledObject.SetActive(false);
            }
            
            UIManager.Instance.isUIPanelOpen = true;

            if (audioClip != null)
            {
                AudioManager.Instance.PlaySound(audioClip);
            }
        }
        
    }

}
