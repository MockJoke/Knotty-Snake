using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private GameObject SettingsCanvas;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider effectsVolumeSlider;
    [Space]
    [SerializeField] private UnityEvent onClose;

    public void OnMusicVolumeChange()
    {
        AudioManager.Instance.OnMusicVolumeChanged(musicVolumeSlider.value);
    }
    
    public void OnEffectsVolumeChange()
    {
        AudioManager.Instance.OnEffectVolumeChanged(effectsVolumeSlider.value);
    }

    private void SetVolumeSliderValues()
    {
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        effectsVolumeSlider.value = PlayerPrefs.GetFloat("EffectsVolume", 1f);
    }
    
    public void OpenMenu()
    {
        AudioManager.Instance.PlaySound(AudioType.SceneTransition);
        SetVolumeSliderValues();
        SettingsCanvas.SetActive(true);
    }

    public void CloseMenu()
    {
        SettingsCanvas.SetActive(false);
        onClose?.Invoke();
    }
}