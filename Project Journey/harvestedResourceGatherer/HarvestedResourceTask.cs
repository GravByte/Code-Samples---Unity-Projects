using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class HarvestedResourceTask : MonoBehaviour, IDataPersistence
{
    [Header("harvester")]
    [SerializeField] private GameObject harvester;
    private Button _harvesterButton;

    private EventTrigger _harvesterEventTrigger;
    
    [Header("Plots")]
    [SerializeField] private Plot[] plots;
    
    [Header("Audio")]
    [SerializeField] private AudioClip harvestSound;
    
    [Header("Cooldown")]
    [SerializeField] private Slider cooldownSlider;
    public int cooldownTimer = 2;
    public bool _isCooldown = false;
    

    [Header("Counters")]
    [SerializeField] private int _harvestedPlots = 0;
    [SerializeField] private int resourceIncreaseAmount = 1;
    
    [Header("Upgrades")]
    [SerializeField] private Button upgradeButton;
    [SerializeField] private int upgradeCost = 10000;
    [SerializeField] private TMP_Text upgradeCostText;
    [SerializeField] private TMP_Text upgradeMultiplierText;
    
    // Start is called before the first frame update
    void Start()
    {
        _harvesterButton = harvester.GetComponent<Button>();    
        
        // Add the EventTrigger component and get a reference to it
        _harvesterEventTrigger = harvester.AddComponent<EventTrigger>();

        // Add a listener for the OnDrag event
        var dragEvent = new EventTrigger.Entry();
        dragEvent.eventID = EventTriggerType.Drag;
        dragEvent.callback.AddListener((data) => { OnHarvesterDrag(); });
        _harvesterEventTrigger.triggers.Add(dragEvent);
    }

    public void LoadData(GameData data)
    {
        resourceIncreaseAmount = data.harResIncrAmount;
        upgradeCost = data.harUpCost;
        UpdateUpgradeText();
    }
    
    public void SaveData(GameData data)
    {
        data.harResIncrAmount = resourceIncreaseAmount;
        data.harUpCost = upgradeCost;
    }
    
    private void Update()
    {
        //---- If the player has enough currency, the upgrade button is interactable
        upgradeButton.interactable = CurrencyManager.Instance.currencyCount >= upgradeCost;
    }

    private void OnHarvesterDrag()
    {
        if (_isCooldown) return;
        
        // Check if the harvester is over a plot collider
        foreach (var plot in plots)
        {
            if (plot.plotCollider.bounds.Contains(Input.mousePosition))
            {
                // Trigger the OnHarvest method in the Plot script
                plot.OnHarvest();
            }
        }
    }

//---- When all plots are harvested, the harvester button is not interactable
    public void CheckIfAllPlotsHarvested()
    {
        if (_isCooldown) return;
        
        //---- Increases the harvestedPlots counter when called by a plot
        _harvestedPlots++;

        //---- If all plots have been harvested, the harvester button is not interactable and cooldown initiated
        if (_harvestedPlots == plots.Length)
        {
            _harvesterButton.interactable = false;
            
            //---- Reset harvester button location
            harvester.transform.position = harvester.GetComponent<Harvester>()._harvesterStartPos;

            if (ResourceManager.Instance.harvestedResourcesCount < ResourceManager.Instance.harvestedResourcesMax)
            {
                ResourceManager.Instance.harvestedResourcesCount += resourceIncreaseAmount;
                ResourceManager.Instance.HarvestedUpdate();
            }

            StartCoroutine(Cooldown(cooldownTimer));
            _harvestedPlots = 0;
        }
    }

    public IEnumerator Cooldown(int delay)
    {
        _isCooldown = true;

        float timer = delay;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            cooldownSlider.value = timer / delay;
            yield return null;
        }
        
        cooldownSlider.value = 0;
        _harvestedPlots = 0;
        _harvesterButton.interactable = true;
        _isCooldown = false;
        
        foreach (var plot in plots)
        {
            plot.ResetPlot();
        }
        
    }
    
    //---- Upgrades the resource count given to the player for each swipe
    public void UpgradeHarvestedResourceCount()
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
    
    public void PlayHarvestSound() => AudioManager.Instance.PlaySound(harvestSound);

}
