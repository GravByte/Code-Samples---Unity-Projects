using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencyManager : MonoBehaviour, IDataPersistence
{
    
    public static CurrencyManager Instance;

    [Header("Currency Texts")]
    [SerializeField] TMP_Text currencyText;
    [SerializeField] TMP_Text premiumCurrencyText;
    [SerializeField] TMP_Text factoryValueText;
    
    [Header("Currency")]
    public int currencyCount;
    public int premiumCurrencyCount;
    public int factoryValue;
    
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
        //---- Load the data into the refinery manager from the game data
        currencyCount = data.currencyCount;
        premiumCurrencyCount = data.premiumCurrencyCount;
        factoryValue = data.factoryValue;

        Debug.Log("currencyCount: " + currencyCount);
        Debug.Log("PremCurrencyCount: " + premiumCurrencyCount);
        Debug.Log("factoryValue: " + factoryValue);
    }
    
    public void SaveData(GameData data)
    {
        //---- Save the data from the refinery manager into the game data
        data.currencyCount = currencyCount;
        data.premiumCurrencyCount = premiumCurrencyCount;
        data.factoryValue = factoryValue;
    }

    // Update is called once per frame
    void Update()
    {
        //---- Update the text of the currency
        currencyText.text = currencyCount.ToString();
        premiumCurrencyText.text = premiumCurrencyCount.ToString();
        factoryValueText.text = "Factory Value: $" + factoryValue.ToString();
    }
    
    //---- Add and remove currency
    public void AddCurrency(int amount) => currencyCount += amount;
    public void RemoveCurrency(int amount) => currencyCount -= amount;
    
    public void AddFactoryValue(int amount) => factoryValue += amount;
    
    
    //---- Add and remove premium currency
    public void AddPremiumCurrency(int amount) => premiumCurrencyCount += amount;
    public void RemovePremiumCurrency(int amount) => premiumCurrencyCount -= amount;

    public void ResetData()
    {
        currencyCount = 0;
        factoryValue = 0;
    }
}
