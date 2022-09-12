using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    
    [Header("Audio")]
    [SerializeField]
    private AudioSource gameMusic;

    [SerializeField] AudioMixer musicMixer;
    [SerializeField] AudioMixer SFXMixer;

    [SerializeField] Slider musicSlider;
    [SerializeField] Slider SFXSlider;

    private void Start()
    {
		//-------- Gets the value of the music and SFX sliders and applies it to the mixer
        
        musicSlider.value = PlayerPrefs.GetFloat("MusicSlider", musicSlider.value);
        SFXSlider.value = PlayerPrefs.GetFloat("SFXSlider", SFXSlider.value);

        Debug.Log("musicValue: " + musicSlider.value);
        Debug.Log("SFXValue: " + musicSlider.value);
        
        MusicVolume(musicSlider.value);
        SFXVolume(SFXSlider.value);
        
    }

    //-------- Adjusts music mixer volume
    public void MusicVolume(float volume)
    {
		//-------- Sets value to mixer in player preferences with a logarithmic conversion
        musicMixer.SetFloat("MusicVol", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVol", Mathf.Log10(volume) * 20);
        
        PlayerPrefs.SetFloat("MusicSlider", musicSlider.value);
        
        PlayerPrefs.Save();
        //Debug.Log("PlayerPrefs Saved");
    }

    //------- adjusts sound effect mixer volume
    public void SFXVolume(float volume)
    {
        SFXMixer.SetFloat("SFXVol", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVol", Mathf.Log10(volume) * 20);
        
        PlayerPrefs.SetFloat("SFXSlider", SFXSlider.value);
        
        PlayerPrefs.Save();
        //Debug.Log("PlayerPrefs Saved");
    }

}
