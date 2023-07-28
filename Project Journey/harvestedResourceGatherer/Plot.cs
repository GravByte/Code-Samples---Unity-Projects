using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---- This script is attached to the Plot prefab.
//---- It detects if the harvester has entered and exited its collider then updates the plot's state to harvested.

public class Plot : MonoBehaviour
{
    
    [Header("UI Elements")]
    public Collider2D plotCollider;
    public Image plotImage;
    public Sprite unharvestedSprite;
    public Sprite harvestedSprite;

    [SerializeField] private HarvestedResourceTask _harvestedResourceTask;
    
    [Header("Plot States")] 
    public bool isHarvested = false;
    public bool isHarvestable = true;

    private void Awake()
    {
        plotCollider = GetComponent<Collider2D>();
        plotImage = GetComponent<Image>();
        plotImage.sprite = unharvestedSprite;
    }

    public void OnHarvest()
    {
        if (isHarvestable)
        {
            isHarvested = true;
            isHarvestable = false;

            // Change the plot's sprite to the harvested sprite
            plotImage.sprite = harvestedSprite;

            // Tell the HarvestedResourceTask that this plot has been harvested
            _harvestedResourceTask.CheckIfAllPlotsHarvested();
            _harvestedResourceTask.PlayHarvestSound();
        }
    }

    public void ResetPlot()
    {
        isHarvested = false;
        isHarvestable = true;
        
        // Change the plot's sprite to the unharvested sprite
        plotImage.sprite = unharvestedSprite;
    }
    
    
    
    
    //---- This is the old OnCollisionEnter2D method. I've replaced it with the OnHarvest method as it doesnt work with UI sprites.
    /*private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Harvester"))
        {
            
            if (isHarvestable)
            {
                isHarvested = true;
                isHarvestable = false;
                
                //---- This is where the plot's sprite will change to the harvested sprite
                plotImage.sprite = harvestedSprite;
                
                //---- Tell the HarvestedResourceTask that this plot has been harvested
                _harvestedResourceTask.CheckIfAllPlotsHarvested();

            }
            
            Debug.Log("(Col) Harvester entered " + gameObject.name);
        }
        Debug.Log("(Col) Hit");
    }*/

   
}
