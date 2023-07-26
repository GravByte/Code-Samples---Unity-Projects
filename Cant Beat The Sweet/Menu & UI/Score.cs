using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    //----------- Private Variables

    [SerializeField]
    bool _showLogs;

    [SerializeField]
    string _Prefix;

    private float _distanceScore = 0.0f;
    public int _currencyScore = 0;
    private int coinsCollected;
    private float _score = 0.0f;
    private float dispSpeedModifier = 0.5f;

    private int difficultyLevel = 1;
    private readonly int maxDifficultyLevel = 8;
   [SerializeField] private int scoreToNextlevel = 20;

    private float speedModifier = 0.5f;

    private bool isDead = false;

    private readonly float startingPitch = 1;

    //private UpgradeMenu _upgradeMenu;
    private float _multiplierValue = 1f;


    //----------- Public&Serialised Variables
    [SerializeField]
    Text distanceText;

    [SerializeField]
    Text scoreText;

    [SerializeField]
    Text highscoreText;

    [SerializeField]
    Text levelText;

    [SerializeField]
    DeathMenu deathMenu;

    [SerializeField]
    AudioSource engineAudio;


    private void Start()
    {
        //----------- Updates high score text
        highscoreText.text = "Highscore\n " + ((int)PlayerPrefs.GetFloat("Highscore")).ToString();
        engineAudio.pitch = startingPitch;
        _multiplierValue += PlayerPrefs.GetInt("scoreMultiplier");
    }

    private void Awake()
    {
        //_upgradeMenu = GetComponent<UpgradeMenu>();
    }


     

    //----------- Update is called once per frame
    void Update()
    {
        //----------- stops updating score when player is killed
        if (isDead)
        {
            return;
        }

        //----------- Increases difficulty level when distance score has surpassed score to next level value.
        if (_distanceScore >= scoreToNextlevel)
        {
            LevelUp();
        }


        //---------- Score/distance Value calculation and representation
        
        _score += (Time.deltaTime * _multiplierValue);
        scoreText.text = ("Score\n " + (int)_score);

        _distanceScore += Time.deltaTime * dispSpeedModifier * GetComponent<PlayerControl>().boost;
        distanceText.text = ("Distance\n " + (int)_distanceScore).ToString();

    }

    //----------- Increases player speed when difficulty level is increased until it reaches the max difficulty.
    private void LevelUp()
    {
        if (difficultyLevel == maxDifficultyLevel)
            return;

        scoreToNextlevel *= 2 + (int)dispSpeedModifier;
        difficultyLevel++;

        GetComponent<PlayerControl>().SetSpeed(speedModifier);
        dispSpeedModifier += speedModifier;
        levelText.text = ("Diff Level: " + (int)difficultyLevel).ToString();
        Debug.Log("Diff: " + difficultyLevel + " speedMod: " + speedModifier);

        //----------- Increases engine pitch
        engineAudio.pitch += (float)0.1;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Coin":

                //--------- When a coin is collected, increases value and adds to score by the amount of coins collected that session each time
                coinsCollected = gameObject.GetComponent<PlayerControl>().currency;
                _score += coinsCollected;
                Log("Coins Collected: " + coinsCollected);

                break;
        }
    }

    //----------- Sets death value to true, updates high score value when the player beats their previous high score and laods the death menu UI.
    public void OnDeath()
    {
        isDead = true;
        if(PlayerPrefs.GetFloat("Highscore") < _score)
            PlayerPrefs.SetFloat("Highscore", _score);
            Debug.Log("New Highscore");

        //_currencyScore = _currencyScore + GetComponent<PlayerControl>()._currency;
        _currencyScore = PlayerPrefs.GetInt("CurrencyScore") + GetComponent<PlayerControl>().currency;

        PlayerPrefs.SetInt("CurrencyScore", _currencyScore);
        
        deathMenu.ToggleEndMenu(_score);
    }

    //-------- Logging Control Method
    private void Log(object message)
    {
        if (_showLogs)
            Debug.Log(_Prefix + message);
    }
}
