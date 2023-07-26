using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using RDG;

public class SettingsMenu : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    bool _showLogs;

    [SerializeField]
    string _Prefix;

    [Header("Audio")]
    [SerializeField] AudioMixer musicMixer;
    [SerializeField] AudioMixer SFXMixer;

    [SerializeField] Slider musicSlider;
    [SerializeField] Slider SFXSlider;

    [Header("Quality")]
    [SerializeField] Dropdown qualityDropdown;

    [Header("Vibration")]
    [SerializeField] private int vibToggleVal = 1;

    [SerializeField] private Toggle vibToggle;
    
    [Header("Panel")]
    [SerializeField]
    GameObject settingsPanel, warningPanel;


    private void Start()
    {

        //Disable logger if  not debug build
        Debug.unityLogger.logEnabled = Debug.isDebugBuild;

        settingsPanel.SetActive(true);
        warningPanel.SetActive(false);

        //-------- Saves and sets audio slider positions
        musicSlider.value = PlayerPrefs.GetFloat("MusicSlider", musicSlider.value);
        SFXSlider.value = PlayerPrefs.GetFloat("SFXSlider", SFXSlider.value);

        vibToggleVal = PlayerPrefs.GetInt("VibTogValue");
        vibToggle.isOn = vibToggleVal == 1;
    }

    //------- Adjusts music slider value
    public void MusicChange()
    {
        PlayerPrefs.SetFloat("MusicSlider", musicSlider.value);
    }

    //----------PlayerPrefs-----------------
    //------- Clears all saved data in prefs
    public void ClearDataButton()
    {
        Vibration.Vibrate(100);
        PlayerPrefs.DeleteAll();
        Log("All Player Data Deleted");
        warningPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void VibrationToggle()
    {
        vibToggleVal = vibToggle.isOn ? 1 : 0;
        
        PlayerPrefs.SetInt("VibTogValue", vibToggleVal);
        
        Log("Vibration: " + vibToggleVal);
    }
    
    //------- adjusts sound effect slider value
    public void SFXSliderChange()
    {
        PlayerPrefs.SetFloat("SFXSlider", SFXSlider.value);
    }



    //----------AudioSliders---------------
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
    //----------AudioSliders---------------


    //---------QualitySettings--------------
    public void SetQuality(int qualityIndex)
    {
        Vibration.Vibrate(100);
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetFloat("QualitySetting", qualityIndex);
        Log("Quality Change");
    }
    
    public void WarningButton()
    {
        Vibration.Vibrate(100);
        settingsPanel.SetActive(false);
        warningPanel.SetActive(true);
    }

    public void ReturnButton()
    {
        Vibration.Vibrate(100);
        warningPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    //-------- Logging Control Method
    private void Log(object message)
    {
        if (_showLogs)
            Debug.Log(_Prefix + message);
    }
}
