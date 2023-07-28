using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StructuralResourceTask : MonoBehaviour, IDataPersistence
{
    [Header("Slider")]
    [SerializeField] private Slider slider;
    private float _sliderValue;
    [SerializeField] private float sliderResetSpeed = 0.001f;
    
    [Header("Structural Resource")]
    [SerializeField] private int resourceIncreaseAmount = 1;
    private bool _canResourceBeGathered;
    [SerializeField] private TMP_Text resourceNameText;
    //[SerializeField] private string resourceName;
    [SerializeField] private TMP_Text resourceDescriptionText;
    //[SerializeField] private string resourceDescription;
    
    [Header("Upgrades")]
    [SerializeField] private Button upgradeButton;
    [SerializeField] private int upgradeCost = 10000;
    [SerializeField] private TMP_Text upgradeCostText;
    [SerializeField] private TMP_Text upgradeMultiplierText;
    
    [Header("Audio")]
    [SerializeField] private AudioClip cutAudioClip;

    [SerializeField] private GameObject halvePiecesRoot;
    void Start()
    {
        _sliderValue = 0;
        _canResourceBeGathered = true;
        
        //---- Set the resource name and description based on the scene index (Not Needed)
        /*resourceNameText.text = SceneManager.Instance.currentSceneIndex switch
        {
            0 => "Log Gatherer",
            1 => "Iron Gatherer",
            2 => "Steel Gatherer",
            3 => "Titanium Gatherer",
            _ => resourceNameText.text
        };
        
        resourceDescriptionText.text = SceneManager.Instance.currentSceneIndex switch
        {
            0 => "Swipe to the end to slice Logs",
            1 => "to gather iron",
            2 => "to gather steel",
            3 => "to gather titanium",
            _ => resourceDescriptionText.text
        };*/
        
    }

    public void LoadData(GameData data)
    {
        resourceIncreaseAmount = data.strResIncrAmount;
        upgradeCost = data.strUpCost;
        UpdateUpgradeText();
    }

    public void SaveData(GameData data)
    {
        data.strResIncrAmount = resourceIncreaseAmount;
        data.strUpCost = upgradeCost;
    }

    //---- Updates the slider value when changed
    public void OnSliderValueChanged()
    {
        _sliderValue = slider.value;
    }

    void Update()
    {
        //---- When the player swipes the slider to the right, the resource count is increased and the slider resets to 0 gradually
        if (_canResourceBeGathered && _sliderValue >= 1f)
        {
            //---- Increase the resource count
            IncreaseStructuralResourceCount();
            AudioManager.Instance.PlaySound(cutAudioClip);
            _canResourceBeGathered = false;
            halvePiecesRoot.SetActive(true);
        }
        
        //---- When the slider has been swiped to the right, the slider is disabled and the slider value is gradually reset to 0
        if (_sliderValue > 0f)
        {
            StartCoroutine(ResetSlider());
        }
        else
        {
            slider.interactable = true;
            _canResourceBeGathered = true;
            halvePiecesRoot.SetActive(false);
        }
        
        //---- If the player has enough currency, the upgrade button is interactable
        upgradeButton.interactable = CurrencyManager.Instance.currencyCount >= upgradeCost;

    }

    //---- Gradually resets the slider to 0 when called
    IEnumerator ResetSlider()
    {
        while (slider.value > 0f)
        {
            slider.value -= sliderResetSpeed * Time.deltaTime;
            yield return null;
        }
    }
    
    private void IncreaseStructuralResourceCount()
    {
        if (ResourceManager.Instance.structuralResourcesCount < ResourceManager.Instance.structuralResourcesMax)
        {
            ResourceManager.Instance.structuralResourcesCount += resourceIncreaseAmount;
            ResourceManager.Instance.StructuralUpdate();
        }
        else
        {
            Debug.Log("Structural Resources Full");
        }
    }

    //---- Upgrades the resource count given to the player for each swipe
    public void UpgradeStructuralResourceCount()
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
