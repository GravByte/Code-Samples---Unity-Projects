using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RDG;

public class UpgradeMenu : MonoBehaviour
{
    public float shieldUpgrade, magnetUpgrade;
    [SerializeField]public Text currencyText, scoreBTNText, shieldBTNText, magnetBTNText, magnetUpText, shieldUpText, scoreUpText;
    public int scoreMultiplier, magCost, shieldCost, multCost, startCost, currencyScore;

    public void Cheat()
    {
        Vibration.Vibrate(100);
        currencyScore = 1000000;
        PlayerPrefs.SetInt("CurrencyScore", currencyScore);
        Refresh();
    }
    public void muliplierBTN()
    {
        Vibration.Vibrate(100);
        if (currencyScore < multCost) return;
        scoreMultiplier += 1;
        currencyScore -= multCost;
        PlayerPrefs.SetInt("CurrencyScore", currencyScore);
        PlayerPrefs.SetInt("scoreMultiplier", scoreMultiplier);
        Refresh();
    }
    public void MagnetBTN()
    {
        Vibration.Vibrate(100);
        if (currencyScore < magCost) return;
        magnetUpgrade += 0.5f;
        currencyScore -= magCost;
        PlayerPrefs.SetInt("CurrencyScore", currencyScore);
        PlayerPrefs.SetFloat("MagnetUp", magnetUpgrade);
        Refresh();
    }

    public void ShieldBTN()
    {
        Vibration.Vibrate(100);
        if (currencyScore < shieldCost) return;
        shieldUpgrade += 0.5f;
        currencyScore -= shieldCost;
        PlayerPrefs.SetInt("CurrencyScore", currencyScore);
        PlayerPrefs.SetFloat("ShieldUp", shieldUpgrade);
        Refresh();
    }
    //----------- Start is called before the first frame update
    void Start()
    {
        startCost = 50;
        Refresh();
    }

    public void Refresh()
    {
        currencyScore = PlayerPrefs.GetInt("CurrencyScore");
        scoreMultiplier = PlayerPrefs.GetInt("scoreMultiplier");
        magnetUpgrade = PlayerPrefs.GetFloat("MagnetUp");
        shieldUpgrade = PlayerPrefs.GetFloat("ShieldUp");

        multCost = ((1 + scoreMultiplier) * (1 + scoreMultiplier) * startCost);
        magCost = (int)((1 + magnetUpgrade) * (1 + magnetUpgrade) * startCost);
        shieldCost = (int)((1 + shieldUpgrade) * (1 + shieldUpgrade) * startCost);

        magnetUpText.text = ("Magnet bonus: +" + magnetUpgrade + "s");
        shieldUpText.text = ("Shield bonus: +" + shieldUpgrade + "s");
        scoreUpText.text = ("Score multiplier: " + (scoreMultiplier + 1) + "x");
        shieldBTNText.text = ("Cost: " + shieldCost);
        magnetBTNText.text = ("Cost: " + magCost);
        scoreBTNText.text = ("Cost: " + multCost);
        currencyText.text = "Currency: " + currencyScore;
    }
    //----------- Update is called once per frame
    //void Update()
    //{

    //    currencyText.text = "Currency: " + currencyScore.ToString();


    //}

}

