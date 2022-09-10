using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = System.Random;

public class TimingMechanismScript : MonoBehaviour
{

    [SerializeField] private GameObject doorGO, doorBtnGO, interactTextGO, timerTextGO;
    [SerializeField] private TMP_Text _timerText;
    
    private bool _isTimerRunning;
    
    private float _timer = -1f;

    // Update is called once per frame
    void Update()
	{
		// Creates a timer which begins running when the timer value is changed 
        if (_timer != -1)
        {
            if (_timer > 0)
            {
                _isTimerRunning = true;
                
                timerTextGO.SetActive(true);
                _timer -= Time.deltaTime;
                _timerText.text = _timer.ToString("F\n"+ "#.00");
                
                // disable interaction while timer active
                interactTextGO.SetActive(false);
                
	            // When the timer is not finished but user presses key, open the door
                if (Input.GetKeyDown(KeyCode.F))
                {
                    doorGO.SetActive(false);
                    _timer = -1;
                    timerTextGO.SetActive(false);
                    Debug.Log("Door opened");
                    
                }
            }
            else
            {
            	// Resets the timer 
            	
                _isTimerRunning = false;
                
                timerTextGO.SetActive(false);
                _timer = -1;
            }
        }
    }
    
	// Activates coroutine when Player interacts with Collider and presses input
    private void OnTriggerStay(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Door":
                if (!_isTimerRunning)
                {
                    // Enable interaction and start timer 
                    interactTextGO.SetActive(true);
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        StartCoroutine(StartTimer());
                    }
                }
                
                break;
        }
    }
    
    
	private void OnTriggerExit(Collider other)
	{
		interactTextGO.SetActive(false);
	}


    private IEnumerator StartTimer()
    {
	    // count down timer subroutine selects a random value when called for the timer length.
        
        _timer = UnityEngine.Random.Range(0.1f, 2f);
        yield return null;
    }

}
