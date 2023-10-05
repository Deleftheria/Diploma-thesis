using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public Slider volumeSlider;
    public AudioMixer audioMixer;
    private float value;

    private void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("saveVolume", value);
    }

    public void SetVolume()
    {
        audioMixer.SetFloat("volume", volumeSlider.value);
        value = volumeSlider.value;
        PlayerPrefs.SetFloat("saveVolume", value);
        print(value);
    }
    
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
