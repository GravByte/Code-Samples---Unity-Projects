using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Menu : MonoBehaviour
{
	[Header("Canvas")]
    [SerializeField] private GameObject gameCanvas;

    [SerializeField] private GameObject endCanvas;
	
	[Header("Managers")]
    [SerializeField] private GameManager gm;
	
	[Header("Text")
    [SerializeField] private TMP_Text playtimeText;
    
    
    // Start is called before the first frame update
    void Awake()
    {
		// Disables game related UI and freezes timescale
        gameCanvas.SetActive(false);
        endCanvas.SetActive(false);
        Time.timeScale = 0;
    }

    public void PlayTime()
    {
		// Writes the playtime text for the end screen and resets value
		// Value remains frozen as the alive timer script does not run until timescale is reset
        playtimeText.text = $"You lasted for\n{gm.playTime} seconds";
        gm.playTime = 0;
    }
    
}
