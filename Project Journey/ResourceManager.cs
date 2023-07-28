using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour, IDataPersistence
{
    public static ResourceManager Instance;

    [Header("Structural Resources")]
    public int structuralResourcesCount;
    public int structuralResourcesMax = 100;
    [SerializeField] private TMP_Text[] structuralResourcesText;
    [SerializeField] private TMP_Text structuralTitleText;
    [SerializeField] private Slider[] structuralResourcesSlider;
    
    [Header("Structural Upgrades")]
    [SerializeField] private Button structuralUpgradeButton;
    [SerializeField] private int structuralUpgradeCost = 10000;
    [SerializeField] private int structuralUpgradeMultiplier = 2;
    [SerializeField] private TMP_Text structuralUpgradeCostText;
    [SerializeField] private TMP_Text structuralUpgradeMultiplierText;
    [SerializeField] private TMP_Text structuralUpgradeCapText;

    [Header("Harvested Resources")]
    public int harvestedResourcesCount;
    public int harvestedResourcesMax = 100;
    [SerializeField] private TMP_Text[] harvestedResourcesText;
    [SerializeField] private TMP_Text harvestedTitleText;
    [SerializeField] private Slider[] harvestedResourcesSlider;

    [Header("Harvested Upgrades")]
    [SerializeField] private Button harvestedUpgradeButton;
    [SerializeField] private int harvestedUpgradeCost = 10000;
    [SerializeField] private int harvestedUpgradeMultiplier = 2;
    [SerializeField] private TMP_Text harvestedUpgradeCostText;
    [SerializeField] private TMP_Text harvestedUpgradeMultiplierText;
    [SerializeField] private TMP_Text harvestedUpgradeCapText;
    
    [Header("Mined Resources")]
    public int minedResourcesCount;
    public int minedResourcesMax = 100;
    [SerializeField] private TMP_Text[] minedResourcesText;
    [SerializeField] private TMP_Text minedTitleText;
    [SerializeField] private Slider[] minedResourcesSlider;
    
    [Header("Mined Upgrades")]
    [SerializeField] private Button minedUpgradeButton;
    [SerializeField] private int minedUpgradeCost = 10000;
    [SerializeField] private int minedUpgradeMultiplier = 2;
    [SerializeField] private TMP_Text minedUpgradeCostText;
    [SerializeField] private TMP_Text minedUpgradeMultiplierText;
    [SerializeField] private TMP_Text minedUpgradeCapText;

    [Header("Special Resources")]
    public int specialResourcesCount;
    public int specialResourcesMax = 100;
    [SerializeField] private TMP_Text[] specialResourcesText;
    [SerializeField] private TMP_Text specialTitleText;
    [SerializeField] private Slider[] specialResourcesSlider;
    
    [Header("Special Upgrades")]
    [SerializeField] private Button specialUpgradeButton;
    [SerializeField] private int specialUpgradeCost = 10000;
    [SerializeField] private int specialUpgradeMultiplier = 2;
    [SerializeField] private TMP_Text specialUpgradeCostText;
    [SerializeField] private TMP_Text specialUpgradeMultiplierText;
    [SerializeField] private TMP_Text specialUpgradeCapText;
    
    private void Awake()
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
        //---- Load the data into the resource manager from the game data
        harvestedResourcesCount = data.harvestedResourcesCount;
        harvestedResourcesMax = data.harvestedResourcesMax;
        harvestedUpgradeCost = data.harvestedUpgradeCost;
        harvestedUpgradeMultiplier = data.harvestedUpgradeMultiplier;
        
        minedResourcesCount = data.minedResourcesCount;
        minedResourcesMax = data.minedResourcesMax;
        minedUpgradeCost = data.minedUpgradeCost;
        minedUpgradeMultiplier = data.minedUpgradeMultiplier;

        structuralResourcesCount = data.structuralResourcesCount;
        structuralResourcesMax = data.structuralResourcesMax;
        structuralUpgradeCost = data.structuralUpgradeCost;
        structuralUpgradeMultiplier = data.structuralUpgradeMultiplier;

        specialResourcesCount = data.specialResourcesCount;
        specialResourcesMax = data.specialResourcesMax;
        specialUpgradeCost = data.specialUpgradeCost;
        specialUpgradeMultiplier = data.specialUpgradeMultiplier;
        
        //---- Update the resource values
        UpdateAllResources();
        
    }
    
    public void SaveData(GameData data)
    {
        //---- Save the data from the resource manager into the game data
        data.harvestedResourcesCount = harvestedResourcesCount;
        data.harvestedResourcesMax = harvestedResourcesMax;
        data.harvestedUpgradeCost = harvestedUpgradeCost;
        data.harvestedUpgradeMultiplier = harvestedUpgradeMultiplier;
        
        data.minedResourcesCount = minedResourcesCount;
        data.minedResourcesMax = minedResourcesMax;
        data.minedUpgradeCost = minedUpgradeCost;
        data.minedUpgradeMultiplier = minedUpgradeMultiplier;
        
        data.structuralResourcesCount = structuralResourcesCount;
        data.structuralResourcesMax = structuralResourcesMax;
        data.structuralUpgradeCost = structuralUpgradeCost;
        data.structuralUpgradeMultiplier = structuralUpgradeMultiplier;
        
        data.specialResourcesCount = specialResourcesCount;
        data.specialResourcesMax = specialResourcesMax;
        data.specialUpgradeCost = specialUpgradeCost;
        data.specialUpgradeMultiplier = specialUpgradeMultiplier;
        
        
    }

    private void Start()
    {

        //---- Change the title text of the resource based on the current scene
        switch (SceneManager.Instance.currentSceneIndex)
        {
            case 0: //---- Industrial Age
                structuralTitleText.text = "Logs";
                harvestedTitleText.text = "Wheat";
                minedTitleText.text = "Stone";
                specialTitleText.text = "Coal";
                break;
            case 1: //---- Machine Age
                structuralTitleText.text = "Bricks";
                harvestedTitleText.text = "Cotton";
                minedTitleText.text = "Iron";
                specialTitleText.text = "Diamonds";
                break;
            case 2: //---- Atomic Age
                structuralTitleText.text = "Steel";
                harvestedTitleText.text = "temp";
                minedTitleText.text = "temp";
                specialTitleText.text = "temp";
                break;
            case 3: //---- Space Age
                structuralTitleText.text = "Titanium";
                harvestedTitleText.text = "temp";
                minedTitleText.text = "temp";
                specialTitleText.text = "temp";
                break;
        }
        // else
        // {
        //     structuralTitleText.text = "Test Resource";
        //     harvestedTitleText.text = "Harvested Resource";
        //     minedTitleText.text = "Mined Resource";
        //     specialTitleText.text = "Special Resource";
        // }

    }

    private void Update()
    {
        //---- Toggle the upgrade buttons interactivity based on the current currency count and the upgrade cost
        structuralUpgradeButton.interactable = CurrencyManager.Instance.currencyCount >= structuralUpgradeCost;
        harvestedUpgradeButton.interactable = CurrencyManager.Instance.currencyCount >= harvestedUpgradeCost;
        minedUpgradeButton.interactable = CurrencyManager.Instance.currencyCount >= minedUpgradeCost;
        specialUpgradeButton.interactable = CurrencyManager.Instance.currencyCount >= specialUpgradeCost;
    }

    //-------------------- Called when other scripts change resource values
    private void UpdateAllResources()
    {
        StructuralUpdate();
        HarvestedUpdate();
        MinedUpdate();
        SpecialUpdate();
    }
    
    public void StructuralUpdate()
    {
        //---- Limit the resource count by the max resource count if it overflows
        if (structuralResourcesCount > structuralResourcesMax)
        {
            structuralResourcesCount = structuralResourcesMax;
        }
        
        //---- Update the text and slider values with the current resource count and limited by the max resource count
        if (structuralResourcesCount <= structuralResourcesMax)
        {
            foreach (TMP_Text text in structuralResourcesText)
            {
                text.text = structuralResourcesCount + "/" + structuralResourcesMax;
            }
            foreach (Slider slider in structuralResourcesSlider)
            {
                slider.value = structuralResourcesCount;
                slider.maxValue = structuralResourcesMax;
            }
        }
        
        //---- Update the upgrade cost and multiplier text
        structuralUpgradeCostText.text = "Cost: " + structuralUpgradeCost.ToString();
        structuralUpgradeMultiplierText.text = "X" + structuralUpgradeMultiplier.ToString();
        structuralUpgradeCapText.text = "Cap: " + structuralResourcesMax.ToString();
        
    }
    
    public void HarvestedUpdate()
    {
        //---- Limit the resource count by the max resource count if it overflows
        if (harvestedResourcesCount > harvestedResourcesMax)
        {
            harvestedResourcesCount = harvestedResourcesMax;
        }

        //---- Update the text and slider values with the current resource count and limited by the max resource count
        if (harvestedResourcesCount <= harvestedResourcesMax)
        {
            foreach (TMP_Text text in harvestedResourcesText)
            {
                text.text = harvestedResourcesCount + "/" + harvestedResourcesMax;
            }
            foreach (Slider slider in harvestedResourcesSlider)
            {
                slider.value = harvestedResourcesCount;
                slider.maxValue = harvestedResourcesMax;
            }
        }
        
        //---- Update the upgrade cost and multiplier text
        harvestedUpgradeCostText.text = "Cost: " + harvestedUpgradeCost.ToString();
        harvestedUpgradeMultiplierText.text = "X" + harvestedUpgradeMultiplier.ToString();
        harvestedUpgradeCapText.text = "Cap: " + harvestedResourcesMax.ToString();
        
    }
    
    public void MinedUpdate()
    {
        //---- Limit the resource count by the max resource count if it overflows
        if (minedResourcesCount > minedResourcesMax)
        {
            minedResourcesCount = minedResourcesMax;
        }
        
        //---- Update the text and slider values with the current resource count and limited by the max resource count
        if (minedResourcesCount <= minedResourcesMax)
        {
            foreach (TMP_Text text in minedResourcesText)
            {
                text.text = minedResourcesCount + "/" + minedResourcesMax;
            }
            foreach (Slider slider in minedResourcesSlider)
            {
                slider.value = minedResourcesCount;
                slider.maxValue = minedResourcesMax;
            }
        }
        
        //---- Update the upgrade cost and multiplier text
        minedUpgradeCostText.text = "Cost: " + minedUpgradeCost.ToString();
        minedUpgradeMultiplierText.text = "X" + minedUpgradeMultiplier.ToString();
        minedUpgradeCapText.text = "Cap: " + minedResourcesMax.ToString();
        
    }
    
    public void SpecialUpdate()
    {
        //---- Limit the resource count by the max resource count if it overflows
        if (specialResourcesCount > specialResourcesMax)
        {
            specialResourcesCount = specialResourcesMax;
        }
        
        //---- Update the text and slider values with the current resource count and limited by the max resource count
        if (specialResourcesCount <= specialResourcesMax)
        {
            foreach (TMP_Text text in specialResourcesText)
            {
                text.text = specialResourcesCount + "/" + specialResourcesMax;
            }
            foreach (Slider slider in specialResourcesSlider)
            {
                slider.value = specialResourcesCount;
                slider.maxValue = specialResourcesMax;
            }
        }
        
        //---- Update the upgrade cost and multiplier text
        specialUpgradeCostText.text = "Cost: " + specialUpgradeCost.ToString();
        specialUpgradeMultiplierText.text = "X" + specialUpgradeMultiplier.ToString();
        specialUpgradeCapText.text = "Cap: " + specialResourcesMax.ToString();
        
    }
    
    //--------------------------------------------------------------------------------
    
    //---- Increase the max resource count by the amount specified for each resource types

    public void IncreaseStructuralMaxResources()
    {
        if ( CurrencyManager.Instance.currencyCount >= structuralUpgradeCost)
        {
            CurrencyManager.Instance.currencyCount -= structuralUpgradeCost;
            structuralResourcesMax += structuralResourcesMax * structuralUpgradeMultiplier;
            structuralUpgradeCost += structuralUpgradeCost * 2;
            structuralUpgradeMultiplier = structuralUpgradeMultiplier * 2;

            StructuralUpdate();
        }
        
        
        foreach (Slider structuralResourcesSlider in structuralResourcesSlider)
        {
            structuralResourcesSlider.maxValue = structuralResourcesMax;
        }
    }
    
    public void IncreaseHarvestedMaxResources()
    {
        if ( CurrencyManager.Instance.currencyCount >= harvestedUpgradeCost)
        {
            CurrencyManager.Instance.currencyCount -= harvestedUpgradeCost;
            harvestedResourcesMax += harvestedResourcesMax * harvestedUpgradeMultiplier;
            harvestedUpgradeCost += harvestedUpgradeCost * 2;
            harvestedUpgradeMultiplier = harvestedUpgradeMultiplier * 2;

            HarvestedUpdate();
        }
        
        foreach (Slider harvestedResourcesSlider in harvestedResourcesSlider)
        {
            harvestedResourcesSlider.maxValue = harvestedResourcesMax;
        }
    }
    
    public void IncreaseMinedMaxResources()
    {
        if ( CurrencyManager.Instance.currencyCount >= minedUpgradeCost)
        {
            CurrencyManager.Instance.currencyCount -= minedUpgradeCost;
            minedResourcesMax += minedResourcesMax * minedUpgradeMultiplier;
            minedUpgradeCost += minedUpgradeCost * 2;
            minedUpgradeMultiplier = minedUpgradeMultiplier * 2;
            
            MinedUpdate();
        }
        
        foreach (Slider minedResourcesSlider in minedResourcesSlider)
        {
            minedResourcesSlider.maxValue = minedResourcesMax;
        }
    }
    
    public void IncreaseSpecialMaxResources()
    {
        if ( CurrencyManager.Instance.currencyCount >= specialUpgradeCost)
        {
            CurrencyManager.Instance.currencyCount -= specialUpgradeCost;
            specialResourcesMax += specialResourcesMax * specialUpgradeMultiplier;
            specialUpgradeCost += specialUpgradeCost * 2;
            specialUpgradeMultiplier = specialUpgradeMultiplier * 2;
            
            SpecialUpdate();
        }
        
        foreach (Slider specialResourcesSlider in specialResourcesSlider)
        {
            specialResourcesSlider.maxValue = specialResourcesMax;
        }
    }

    public void ResetData()
    {
        harvestedResourcesCount = 0;
        harvestedResourcesMax = 100;
        harvestedUpgradeCost = 1000;
        harvestedUpgradeMultiplier = 2;
        structuralResourcesCount = 0;
        structuralResourcesMax = 100;
        structuralUpgradeCost = 1000;
        structuralUpgradeMultiplier = 2;
        minedResourcesCount = 0;
        minedResourcesMax = 100;
        minedUpgradeCost = 1000;
        minedUpgradeMultiplier = 2;
        specialResourcesCount = 0;
        specialResourcesMax = 100;
        specialUpgradeCost = 1000;
        specialUpgradeMultiplier = 2;
    }


}
