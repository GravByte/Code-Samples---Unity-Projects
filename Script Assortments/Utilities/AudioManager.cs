using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    
    [Header("Audio")]
    [SerializeField]
    private AudioSource gameMusic;

    [SerializeField] AudioMixer musicMixer;
    [SerializeField] AudioMixer SFXMixer;

    [SerializeField] float musicSliderValue, SFXSliderValue;

    private void Awake()
    {
		//-------- Gets the value of the music and SFX sliders and applies it to the mixer
        musicSliderValue = PlayerPrefs.GetFloat("MusicSlider", musicSliderValue);
        Debug.Log("musicValue: " + musicSliderValue);

        SFXSliderValue = PlayerPrefs.GetFloat("SFXSlider", SFXSliderValue);
        Debug.Log("SFXValue: " + musicSliderValue);

        MusicVolume(musicSliderValue);
        SFXVolume(SFXSliderValue);

    }

    //-------- Adjusts music mixer volume
    public void MusicVolume(float volume)
    {
		//-------- Sets value to mixer in player preferences with a logarithmic conversion
        musicMixer.SetFloat("MusicVol", Mathf.Log10(volume) * 20);
    }

    //------- adjusts sound effect mixer volume
    public void SFXVolume(float volume)
    {
        SFXMixer.SetFloat("SFXVol", Mathf.Log10(volume) * 20);
    }
    
}
