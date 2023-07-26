using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    bool _showLogs;

    [SerializeField]
    string _Prefix;

    [SerializeField]
    float loadDelay = 1f;
    float audioDelay = 0.1f;

    [Header("Audio")]
    [SerializeField]
    private AudioSource gameMusic;

    [SerializeField] AudioMixer musicMixer;
    [SerializeField] AudioMixer SFXMixer;

    [SerializeField] float musicSliderValue, SFXSliderValue;
    [SerializeField] int _quality;

    private SceneLoader.Scene _loadScene;



    //----------- Start is called before the first frame update
    void Start()
    {
        //Disable logger if  not debug build
        Debug.unityLogger.logEnabled = Debug.isDebugBuild;

        Application.targetFrameRate = 60;

    }

    private void Awake()
    {
        SetQuality(_quality);

        musicSliderValue = PlayerPrefs.GetFloat("MusicSlider", musicSliderValue);
        Log("musicValue: " + musicSliderValue);

        SFXSliderValue = PlayerPrefs.GetFloat("SFXSlider", SFXSliderValue);
        Log("SFXValue: " + musicSliderValue);

        MusicVolume(musicSliderValue);
        SFXVolume(SFXSliderValue);

        StartCoroutine(DelayAudioStart());

    }

    //-------- Adjusts music mixer volume
    public void MusicVolume(float volume)
    {
        musicMixer.SetFloat("MusicVol", Mathf.Log10(volume) * 20);
    }

    //------- adjusts sound effect mixer volume
    public void SFXVolume(float volume)
    {
        SFXMixer.SetFloat("SFXVol", Mathf.Log10(volume) * 20);
    }

    //--------- QualitySettings
    public void SetQuality(int qualityIndex)
    {
        _quality = PlayerPrefs.GetInt("QualitySetting", qualityIndex);
        QualitySettings.SetQualityLevel(_quality);
        Log("Quality: " + _quality);
    }

    private void Update()
    {
        if (Application.platform != RuntimePlatform.Android) return;
        if (!Input.GetKey(KeyCode.Escape)) return;
        StartCoroutine(DelaySceneLoad());
        _loadScene = SceneLoader.Scene.MainMenu_Scene;

        return;
    }

    private IEnumerator DelayAudioStart()
    {
        yield return new WaitForSeconds(audioDelay);
        gameMusic.Play();
    }

    private IEnumerator DelaySceneLoad()
    {
        yield return new WaitForSeconds(loadDelay);
        Log("LoadDelay: " + loadDelay);
        SceneLoader.Load(_loadScene);
    }

    //-------- Logging Control Method
    private void Log(object message)
    {
        if (_showLogs)
            Debug.Log(_Prefix + message);
    }
}
