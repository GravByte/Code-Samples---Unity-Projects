using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour, IDataPersistence
{
    
    public static AudioManager Instance;

    [SerializeField] private AudioSource musicSource, sfxSource;
    
    [SerializeField] private Slider sfxSlider, musicSlider;
    
    [SerializeField] private ToggleImage sfxImage, musicImage;
    [SerializeField] bool _sfxImageEnabled, _musicImageEnabled;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void LoadData(GameData data)
    {
        sfxSource.mute = data.sfxDisabled;
        musicSource.mute = data.musicDisabled;
        
        sfxSource.volume = data.sfxVolume;
        sfxSlider.value = data.sfxVolume;
        
        musicSource.volume = data.musicVolume;
        musicSlider.value = data.musicVolume;
        
        
        _sfxImageEnabled = data.sfxImageEnabled;
        sfxImage.enableImage = _sfxImageEnabled;
        
        _musicImageEnabled = data.musicImageEnabled;
        musicImage.enableImage = _musicImageEnabled;
    }
    
    public void SaveData(GameData data)
    {
        data.sfxDisabled = sfxSource.mute;
        data.musicDisabled = musicSource.mute;
        
        data.sfxVolume = sfxSource.volume;
        data.sfxVolume = sfxSlider.value;
        
        data.musicVolume = musicSource.volume;
        data.musicVolume = musicSlider.value;
        
        data.sfxImageEnabled = _sfxImageEnabled;
        data.musicImageEnabled = _musicImageEnabled;
    }

    public void PlaySound(AudioClip clip) => sfxSource.PlayOneShot(clip);

    public void ChangeSfxVolume(float value) => sfxSource.volume = value;

    public void ChangeMusicVolume(float value) => musicSource.volume = value;

    public void ToggleSfx()
    {
        sfxSource.mute = !sfxSource.mute;
        
        if (sfxImage != null)
        {
            _sfxImageEnabled = sfxImage.enableImage;
        }
    }
    
    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
        
        if (musicImage != null)
        {
            _musicImageEnabled = musicImage.enableImage;
        }
    }
    
}
