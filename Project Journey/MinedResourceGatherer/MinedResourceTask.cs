using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class MinedResourceTask : MonoBehaviour, IDataPersistence
{
    [Header("UI Elements")]
    [SerializeField] private Slider slider;
    [SerializeField] private Button button;
    [SerializeField] private ButtonEventHandler buttonEventHandler;
    [SerializeField] private float fillSpeed = 1f;
    [SerializeField] private float minZoneValue = 0.5f;
    [SerializeField] private float maxZoneValue = 0.7f;
    [SerializeField] private int cooldownTimer = 5;

    [SerializeField] private GameObject rockImageGO;
    [SerializeField] private GameObject brokenRockImageGO;
    
    [Header("Audio")]
    [SerializeField] private AudioClip rockBreakSound;
    
    [Header("Upgrades")]
    [SerializeField] private Button upgradeButton;
    [SerializeField] private int upgradeCost = 10000;
    [SerializeField] private TMP_Text upgradeCostText;
    [SerializeField] private TMP_Text upgradeMultiplierText;

    [SerializeField] private int resourceIncreaseAmount = 1;
    [SerializeField] private bool isHoldingButton = false;
    [SerializeField] private float fillAmount = 0f;
    private bool _isMovingUp = true;
    private bool _isIncreasingCount = false;

    private bool _isMobile;
    
    // Start is called before the first frame update
    private void Start()
    {
        slider.value = 0;
        fillAmount = 0f;
        
        // Check if we're on a mobile device
        #if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
            _isMobile = true;
        #else
            _isMobile = false;
        #endif
    }
    
    public void LoadData(GameData data)
    {
        resourceIncreaseAmount = data.minResIncrAmount;
        upgradeCost = data.minUpCost;
        UpdateUpgradeText();
    }
    
    public void SaveData(GameData data)
    {
        data.minResIncrAmount = resourceIncreaseAmount;
        data.minUpCost = upgradeCost;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isMobile)
        {
            // Handle mobile input
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began && buttonEventHandler.isButtonPressed)
                {
                    isHoldingButton = true;
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    isHoldingButton = false;
                }
            }
        }
        else
        {
            // Handle non-mobile input
            isHoldingButton = buttonEventHandler.isButtonPressed;
        }
        
        //---- If the player is holding the button, calls the method that fills the slider up
        if (isHoldingButton)
        {
            UpdateFillAmount();
        }
        else if (!isHoldingButton && fillAmount > 0f)
        {
            StartCoroutine(ResetFillAmount(cooldownTimer));
        }
        
        
        //---- If the slider is in the correct zone, increase the resource count
        if ((!isHoldingButton && !_isIncreasingCount) && (fillAmount >= minZoneValue && fillAmount <= maxZoneValue))
        {
            IncreaseMinedResourceCount();
            _isIncreasingCount = true;
            //Debug.Log("Increase mined resource count");
            
            rockImageGO.SetActive(false);
            brokenRockImageGO.SetActive(true);
            AudioManager.Instance.PlaySound(rockBreakSound);
        }
        
        //---- If the player has enough currency, the upgrade button is interactable
        upgradeButton.interactable = CurrencyManager.Instance.currencyCount >= upgradeCost;
        
    }

    //---- When the player holds the button, the slider fills up
    private void UpdateFillAmount()
    {
        if (_isMovingUp)
        {
            fillAmount += fillSpeed * Time.deltaTime;
            if (fillAmount >= 1f)
            {
                fillAmount = 1f;
                _isMovingUp = false;
            }
        }
        else
        {
            fillAmount -= fillSpeed * Time.deltaTime;
            if (fillAmount <= 0f)
            {
                fillAmount = 0f;
                _isMovingUp = true;
            }
        }
        slider.value = fillAmount;
        
    }

    //---- Increases the resource count and updates the UI
    private void IncreaseMinedResourceCount()
    {
        if (ResourceManager.Instance.minedResourcesCount < ResourceManager.Instance.minedResourcesMax)
        {
            ResourceManager.Instance.minedResourcesCount += resourceIncreaseAmount;
            ResourceManager.Instance.MinedUpdate(); 
            Debug.Log("Mined Resources Increased");
        }
        else
        {
            Debug.Log("Mined Resources Full");
        }
    }
    
    private IEnumerator ResetFillAmount(int delay)
    {
        float timer = delay;
        button.interactable = false;
        //---- while loop to add a cooldown to the button
        while (!isHoldingButton && timer > 0)
        {
            timer -= Time.deltaTime;
            
            //---- gradually reduce the fill amount to 0 for the duration of the cooldown
            slider.value = timer / delay;

            yield return null;
        }
        
        //---- reset the fill amount to 0, and enable the button
        if (timer <= 0)
        {
            fillAmount = 0f;
            slider.value = fillAmount;
            
            button.interactable = true;
            _isIncreasingCount = false;
            
            rockImageGO.SetActive(true);
            brokenRockImageGO.SetActive(false);
        }
    }
    
    //---- Upgrades the resource count given to the player for each swipe
    public void UpgradeMinedResourceCount()
    {
        //---- If the player has enough currency, the resource count is doubled and the upgrade cost is doubled
        if (CurrencyManager.Instance.currencyCount >= upgradeCost)
        {
            CurrencyManager.Instance.currencyCount -= upgradeCost;
            resourceIncreaseAmount *= 2;
            upgradeCost *= 2;
            UpdateUpgradeText();
        }
        else
        {
            Debug.Log("Not Enough Currency");
        }
    }

    private void UpdateUpgradeText()
    {
        upgradeCostText.text = upgradeCost.ToString();
        upgradeMultiplierText.text = "+" + resourceIncreaseAmount.ToString();
    }

    public void ResetData()
    {
        resourceIncreaseAmount = 1;
        upgradeCost = 1000;
    }
}
