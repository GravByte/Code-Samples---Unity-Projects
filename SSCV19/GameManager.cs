using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    // holds lock values to manage the Windows cursor
    CursorLockMode _lockMode;

    [Header("Settings")]
    [SerializeField]
    bool _showLogs;

    [SerializeField]
    string _prefix;

    //[SerializeField]
    //private GameObject player;

    //[SerializeField]
    //private Text interactText;

    private bool _gameOver = false;

    [Header("Audio")]
    [SerializeField]
    float _audioDelay = 0.1f;

    [SerializeField]
    private AudioSource _gameMusic;

    [SerializeField] AudioMixer _musicMixer;
    [SerializeField] AudioMixer _sfxMixer;

    [SerializeField] float _musicSliderValue, _sfxSliderValue;
    [SerializeField] int _quality;

    public bool GameOver
    {
        get { return _gameOver; }
    }

    //public GameObject Player
    //{
    //    get { return player; }
    //}

    private void Awake()
    {
        //Instancing
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        LockCursor();

        SetQuality(_quality);

        _musicSliderValue = PlayerPrefs.GetFloat("MusicSlider", _musicSliderValue);
        Log("musicValue: " + _musicSliderValue);

        _sfxSliderValue = PlayerPrefs.GetFloat("SFXSlider", _sfxSliderValue);
        Log("SFXValue: " + _musicSliderValue);

        MusicVolume(_musicSliderValue);
        SfxVolume(_sfxSliderValue);

        StartCoroutine(DelayAudioStart());

        //DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && _lockMode == CursorLockMode.Locked){
            UnlockCursor();
        }
        else if (Input.GetKeyDown(KeyCode.Q) && _lockMode == CursorLockMode.Confined)
        {
            LockCursor();
        }
    }

    public void LockCursor()
    {
        //Locks the cursor
        _lockMode = CursorLockMode.Locked;
        Cursor.lockState = _lockMode;
    }

    public void UnlockCursor()
    {
        //Locks the cursor
        _lockMode = CursorLockMode.Confined;
        Cursor.lockState = _lockMode;
    }

    //-------- Adjusts music mixer volume
    public void MusicVolume(float volume)
    {
        _musicMixer.SetFloat("MusicVol", Mathf.Log10(volume) * 20);
    }

    //------- adjusts sound effect mixer volume
    public void SfxVolume(float volume)
    {
        _sfxMixer.SetFloat("SFXVol", Mathf.Log10(volume) * 20);
    }

    //--------- QualitySettings
    public void SetQuality(int qualityIndex)
    {
        _quality = PlayerPrefs.GetInt("QualitySetting", qualityIndex);
        QualitySettings.SetQualityLevel(_quality);
        Log("Quality: " + _quality);
    }

    private IEnumerator DelayAudioStart()
    {
        yield return new WaitForSeconds(_audioDelay);
        _gameMusic.Play();
    }

    //Is it game over?
    public void PlayerHurt(int currentHealthPoints)
    {
        if (currentHealthPoints > 0)
        {
            _gameOver = false;
        }
        else
        {
            _gameOver = true;
        }
    }

    //-------- Logging Control Method
    public void Log(object message)
    {
        if (_showLogs)
            Debug.Log(_prefix + message);
    }

}
