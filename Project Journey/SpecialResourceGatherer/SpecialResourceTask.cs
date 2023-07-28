using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class SpecialResourceTask : MonoBehaviour, IDataPersistence
{
    [Header("UI Elements")]
    [SerializeField] private Button[] gridSquares; //---- array of buttons representing the grid squares
    [SerializeField] private Image resourceIcon; //---- image to be displayed when player finds the correct square
    [SerializeField] private Sprite wallSprite;
    [SerializeField] private Sprite brokenWallSprite;
    
    [Header("Audio")]
    [SerializeField] private AudioClip wallBreakSound;
    [SerializeField] private AudioClip rewardSound;
    
    [Header("Upgrades")]
    [SerializeField] private Button upgradeButton;
    [SerializeField] private int upgradeCost = 10000;
    [SerializeField] private TMP_Text upgradeCostText;
    [SerializeField] private TMP_Text upgradeMultiplierText;
    
    [Header("Resource Settings")]
    [SerializeField] private int resourceIncreaseAmount = 1; //---- amount to increase resource count by when player finds the correct square
    
    [SerializeField] private int correctSquareIndex; //---- index of the square containing the resource icon
    
    [Header("Attempts Settings")]
    [SerializeField] private int attemptsLeft = 3; //---- number of attempts the player has left
    [SerializeField] private TMP_Text attemptsText; //---- text to display the number of attempts left
    private int _totalAttempts; //---- total number of attempts the player has
    
    [Header("Cooldown Settings")]
    [SerializeField] private Slider cooldownSlider; //---- slider to display cooldown progress
    [SerializeField] private bool isCooldownActive = false; //---- flag indicating whether cooldown is currently active
    [SerializeField] private int cooldownTime = 2; //---- duration of cooldown in seconds
    
    // Start is called before the first frame update
    void Start()
    {
        //---- set the total number of attempts
        _totalAttempts = attemptsLeft;
        UpdateAttemptText();
        
        //---- choose a random square to contain the resource icon
        correctSquareIndex = Random.Range(0, gridSquares.Length);

        //---- move the resource icon to the correct square's transform
        resourceIcon.transform.position = gridSquares[correctSquareIndex].transform.position;

        //---- assign a click listener to each button to call OnSquareClick when clicked
        for (int i = 0; i < gridSquares.Length; i++)
        {
            int squareIndex = i; //---- create a local variable to represent the current value of i
            gridSquares[i].onClick.AddListener(delegate { OnSquareClick(squareIndex); });
        }
    }
    
    public void LoadData(GameData data)
    {
        resourceIncreaseAmount = data.speResIncrAmount;
        upgradeCost = data.speUpCost;
        UpdateUpgradeText();
    }

    public void SaveData(GameData data)
    {
        data.speResIncrAmount = resourceIncreaseAmount;
        data.speUpCost = upgradeCost;
    }
    
    private void Update()
    {
        //---- If the player has enough currency, the upgrade button is interactable
        upgradeButton.interactable = CurrencyManager.Instance.currencyCount >= upgradeCost;
    }

    void OnSquareClick(int clickedIndex)
    {
        if (isCooldownActive) return; //---- if cooldown is active, do not allow the player to click any squares
        
        //---- disable the clicked square and check if it contains the resource icon
        //gridSquares[clickedIndex].gameObject.SetActive(false);
        gridSquares[clickedIndex].image.sprite = brokenWallSprite;
        AudioManager.Instance.PlaySound(wallBreakSound);
        
        if (clickedIndex == correctSquareIndex)
        {
            //---- player found the correct square
            GiveResource();
            AudioManager.Instance.PlaySound(rewardSound);
        }
        else
        {
            //---- player did not find the correct square
            attemptsLeft--;
            UpdateAttemptText();
            
            if (attemptsLeft <= 0)
            {
                //---- player used all their attempts, disable all squares to tease the correct location and start cooldown
                DisableAllSquares();
                StartCoroutine(StartCooldown(cooldownTime));
            }
        }
    }

    void GiveResource()
    {
        //---- give the resource to the player and start cooldown
        if (ResourceManager.Instance.specialResourcesCount < ResourceManager.Instance.specialResourcesMax)
        {
            ResourceManager.Instance.specialResourcesCount += resourceIncreaseAmount;
            ResourceManager.Instance.SpecialUpdate();
        }
        
        Debug.Log("You found the resource!");

        foreach (var gs in gridSquares)
        {
            //---- set button interactable to false
            gs.interactable = false;
        }
        
        StartCoroutine(StartCooldown(cooldownTime));
    }

    void DisableAllSquares()
    {
        //---- disable all squares
        foreach (var gs in gridSquares)
        {
            gs.image.sprite = brokenWallSprite;
            //gs.gameObject.SetActive(false);
        }
    }

    IEnumerator StartCooldown(int delay)
    {
        isCooldownActive = true;
        
        float timer = delay;
        
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            cooldownSlider.value = timer / delay;
            yield return null;
        }
        
        // reset game state and choose new location
        cooldownSlider.value = 0;
        attemptsLeft = 3;
        isCooldownActive = false;
        resourceIcon.transform.position = gridSquares[correctSquareIndex].transform.position;
        UpdateAttemptText();
        
        foreach (var gs in gridSquares)
        {
            gs.image.sprite = wallSprite;
            //gs.gameObject.SetActive(true);
            gs.interactable = true;
        }
        
        correctSquareIndex = Random.Range(0, gridSquares.Length);
        resourceIcon.transform.position = gridSquares[correctSquareIndex].transform.position;
        
    }

    //---- Lambda method to update the attempts text
    void UpdateAttemptText() => attemptsText.text = "Attempt " + attemptsLeft.ToString() + " / " + _totalAttempts.ToString();
    
    //---- Upgrades the resource count given to the player for each swipe
    public void UpgradeSpecialResourceCount()
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
        upgradeCost = 10000;
    }
    
}
