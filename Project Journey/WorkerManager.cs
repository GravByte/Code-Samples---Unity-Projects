using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorkerManager : MonoBehaviour, IDataPersistence
{
    [SerializeField] private int workerDelay = 10;
    
    [Header("Structural Worker")]
    [SerializeField] private int strWorkerCount;
    [SerializeField] private int strWorkerCost = 250;
    [SerializeField] private int strWorkerCostMultiplier = 2;
    [SerializeField] private Button strWorkerButton;
    [SerializeField] private TMP_Text strWorkerCostText;
    [SerializeField] private TMP_Text strWorkerCountText;
    private bool _strIsGathering = false;

    [Header("Harvested Worker")]
    [SerializeField] private int harWorkerCount;
    [SerializeField] private int harWorkerCost = 250;
    [SerializeField] private int harWorkerCostMultiplier = 2;
    [SerializeField] private Button harWorkerButton;
    [SerializeField] private TMP_Text harWorkerCostText;
    [SerializeField] private TMP_Text harWorkerCountText;
    private bool _harIsGathering = false;

    [Header("Mined Worker")]
    [SerializeField] private int minWorkerCount;
    [SerializeField] private int minWorkerCost = 250;
    [SerializeField] private int minWorkerCostMultiplier = 2;
    [SerializeField] private Button minWorkerButton;
    [SerializeField] private TMP_Text minWorkerCostText;
    [SerializeField] private TMP_Text minWorkerCountText;
    private bool _minIsGathering = false;
    
    [Header("Special Worker")]
    [SerializeField] private int speWorkerCount;
    [SerializeField] private int speWorkerCost = 250;
    [SerializeField] private int speWorkerCostMultiplier = 2;
    [SerializeField] private Button speWorkerButton;
    [SerializeField] private TMP_Text speWorkerCostText;
    [SerializeField] private TMP_Text speWorkerCountText;
    private bool _speIsGathering = false;

    public static WorkerManager Instance;
    
    public void Awake()
    {
        //---- If there is an instance, and it's not this, delete this.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void LoadData(GameData data)
    {
        strWorkerCost = data.strWorkerCost;
        strWorkerCount = data.strWorkerCount;
        harWorkerCost = data.harWorkerCost;
        harWorkerCount = data.harWorkerCount;
        minWorkerCost = data.minWorkerCost;
        minWorkerCount = data.minWorkerCount;
        speWorkerCost = data.speWorkerCost;
        speWorkerCount = data.speWorkerCount;
        
        StrWorkerTextUpdate();
        MinWorkerTextUpdate();
        HarWorkerTextUpdate();
        SpeWorkerTextUpdate();
    }
    
    public void SaveData(GameData data)
    {
        data.strWorkerCost = strWorkerCost;
        data.strWorkerCount = strWorkerCount;
        data.harWorkerCost = harWorkerCost;
        data.harWorkerCount = harWorkerCount;
        data.minWorkerCost = minWorkerCost;
        data.minWorkerCount = minWorkerCount;
        data.speWorkerCost = speWorkerCost;
        data.speWorkerCount = speWorkerCount;
    }
    
    
    private void Update()
    {
        //---- Runs the automatic resource gathering
        if (!_strIsGathering && strWorkerCount > 0 && ResourceManager.Instance.structuralResourcesCount < ResourceManager.Instance.structuralResourcesMax)
            StartCoroutine(GatherStructuralResource());

        //---- Enables the button if the player has enough currency
        strWorkerButton.interactable = CurrencyManager.Instance.currencyCount >= strWorkerCost;
        
        if (!_harIsGathering && harWorkerCount > 0 && ResourceManager.Instance.harvestedResourcesCount < ResourceManager.Instance.harvestedResourcesMax)
            StartCoroutine(GatherHarvestedResource());

        harWorkerButton.interactable = CurrencyManager.Instance.currencyCount >= harWorkerCost;
        
        if (!_minIsGathering && minWorkerCount > 0 && ResourceManager.Instance.minedResourcesCount < ResourceManager.Instance.minedResourcesMax)
            StartCoroutine(GatherMinedResource());
        
        minWorkerButton.interactable = CurrencyManager.Instance.currencyCount >= minWorkerCost;

        if (!_speIsGathering && speWorkerCount > 0 && ResourceManager.Instance.specialResourcesCount < ResourceManager.Instance.specialResourcesMax)
            StartCoroutine(GatherSpecialResource());
        
        speWorkerButton.interactable = CurrencyManager.Instance.currencyCount >= speWorkerCost;
    }

    //---- Runs a loop to gather structural resources
    IEnumerator GatherStructuralResource()
    {
        _strIsGathering = true;
        
        yield return new WaitForSeconds(workerDelay);
        
        ResourceManager.Instance.structuralResourcesCount += strWorkerCount;
        ResourceManager.Instance.StructuralUpdate();
        //Debug.Log("Structural Resource Gathered by workers: " + strWorkerCount);
        
        _strIsGathering = false;
    }
    
    //---- Runs a loop to gather harvested resources
    IEnumerator GatherHarvestedResource()
    {
        
        _harIsGathering = true;
        
        yield return new WaitForSeconds(workerDelay);
        
        ResourceManager.Instance.harvestedResourcesCount += harWorkerCount;
        ResourceManager.Instance.HarvestedUpdate();
        //Debug.Log("Harvested Resource Gathered by workers: " + harWorkerCount);
        
        _harIsGathering = false;
    }
    
    //---- Runs a loop to gather mined resources
    IEnumerator GatherMinedResource()
    {
     
        _minIsGathering = true;
        
        yield return new WaitForSeconds(workerDelay);
        
        ResourceManager.Instance.minedResourcesCount += minWorkerCount;
        ResourceManager.Instance.MinedUpdate();
        //Debug.Log("Mined Resource Gathered by workers: " + minWorkerCount);
        
        _minIsGathering = false;
    }
    
    //---- Runs a loop to gather special resources
    IEnumerator GatherSpecialResource()
    {
        
        _speIsGathering = true;
        
        yield return new WaitForSeconds(workerDelay);
        
        ResourceManager.Instance.specialResourcesCount += speWorkerCount;
        ResourceManager.Instance.SpecialUpdate();
        //Debug.Log("Special Resource Gathered by workers: " + speWorkerCount);
        
        _speIsGathering = false;
    }
    
    //---- Increases the structural worker count and decreases the currency count when called
    public void IncreaseStrWorkerCount()
    {
        if (CurrencyManager.Instance.currencyCount >= strWorkerCost)
        {
            CurrencyManager.Instance.currencyCount -= strWorkerCost;
            strWorkerCount++;
            strWorkerCost = strWorkerCost * strWorkerCostMultiplier;
            StrWorkerTextUpdate();
        }
        else
        {
            Debug.Log("Not enough currency to hire a worker");
        }
    }

    //---- Updates the text for the worker count and cost
    private void StrWorkerTextUpdate()
    {
        strWorkerCostText.text = strWorkerCost.ToString();
        strWorkerCountText.text = strWorkerCount.ToString();
    }
    
    //---- Increases the harvester worker count and decreases the currency count when called
    public void IncreaseHarWorkerCount()
    {
        if (CurrencyManager.Instance.currencyCount >= harWorkerCost)
        {
            CurrencyManager.Instance.currencyCount -= harWorkerCost;
            harWorkerCount++;
            harWorkerCost = harWorkerCost * harWorkerCostMultiplier;
            
            HarWorkerTextUpdate();
        }
        else
        {
            Debug.Log("Not enough currency to hire a worker");
        }
    }
    
    private void HarWorkerTextUpdate()
    {
        harWorkerCostText.text = harWorkerCost.ToString();
        harWorkerCountText.text = harWorkerCount.ToString();
    }
    
    //---- Increases the mining worker count and decreases the currency count when called
    public void IncreaseMinWorkerCount()
    {
        if (CurrencyManager.Instance.currencyCount >= minWorkerCost)
        {
            CurrencyManager.Instance.currencyCount -= minWorkerCost;
            minWorkerCount++;
            minWorkerCost = minWorkerCost * minWorkerCostMultiplier;
            
            MinWorkerTextUpdate();
        }
        else
        {
            Debug.Log("Not enough currency to hire a worker");
        }
    }
    
    private void MinWorkerTextUpdate()
    {
        minWorkerCostText.text = minWorkerCost.ToString();
        minWorkerCountText.text = minWorkerCount.ToString();
    }
    
    //---- Increases the special worker count and decreases the currency count when called
    public void IncreaseSpeWorkerCount()
    {
        if (CurrencyManager.Instance.currencyCount >= speWorkerCost)
        {
            CurrencyManager.Instance.currencyCount -= speWorkerCost;
            speWorkerCount++;
            speWorkerCost = speWorkerCost * speWorkerCostMultiplier;
            
            SpeWorkerTextUpdate();
        }
        else
        {
            Debug.Log("Not enough currency to hire a worker");
        }
    }
    
    private void SpeWorkerTextUpdate()
    {
        speWorkerCostText.text = speWorkerCost.ToString();
        speWorkerCountText.text = speWorkerCount.ToString();
    }

    public void ResetData()
    {
        strWorkerCost = 250;
        strWorkerCount = 0;
        harWorkerCost = 250;
        harWorkerCount = 0;
        minWorkerCost = 250;
        minWorkerCount = 0;
        speWorkerCost = 250;
        speWorkerCount = 0;
    }
}
