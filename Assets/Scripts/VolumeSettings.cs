using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;
    [SerializeField] private TextMeshProUGUI musicVolumeText;
    [SerializeField] private TextMeshProUGUI SFXVolumeText;

    private const string MusicVolumeKey = "musicVolume";
    private const string SFXVolumeKey = "SFXVolume";
    private const float VolumeThreshold = 0.0001f; // Valor epsilon para evitar problemas de logaritmo

    private void Start()
    {
        LoadVolume();

        // Eventos para los sliders
        musicSlider.onValueChanged.AddListener(delegate { SetVolume("music", MusicVolumeKey, musicSlider.value, musicVolumeText); });
        SFXSlider.onValueChanged.AddListener(delegate { SetVolume("SFX", SFXVolumeKey, SFXSlider.value, SFXVolumeText); });
    }

    private void SetVolume(string parameterName, string prefsKey, float sliderValue, TextMeshProUGUI volumeText)
    {
        float volume = sliderValue > VolumeThreshold ? Mathf.Log10(sliderValue) * 20 : -80;
        myMixer.SetFloat(parameterName, volume);
        PlayerPrefs.SetFloat(prefsKey, sliderValue);
        volumeText.text = Mathf.RoundToInt(sliderValue * 100).ToString();
    }

    private void LoadVolume()
    {
        if (PlayerPrefs.HasKey(MusicVolumeKey))
        {
            float musicVolume = PlayerPrefs.GetFloat(MusicVolumeKey);
            musicSlider.value = musicVolume;
            SetVolume("music", MusicVolumeKey, musicVolume, musicVolumeText);
        }

        if (PlayerPrefs.HasKey(SFXVolumeKey))
        {
            float sfxVolume = PlayerPrefs.GetFloat(SFXVolumeKey);
            SFXSlider.value = sfxVolume;
            SetVolume("SFX", SFXVolumeKey, sfxVolume, SFXVolumeText);
        }
    }
}
