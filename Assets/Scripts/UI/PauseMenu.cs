using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("Menu Containers")]
    [SerializeField] private GameObject PauseMenuCanvas;
    [SerializeField] private GameObject HelpMenuCanvas;
    [SerializeField] private SettingsMenu settingsMenu;
    [SerializeField] private GameObject GameplayCanvas;
    
    [Space] 
    [SerializeField] private GameObject PlayerContainer;
    [SerializeField] private GameObject FoodContainer;
    [SerializeField] private GameObject PowerUpContainer;
    
    
    // [Header("Buttons")]
    // [SerializeField] private Button 
    
    [SerializeField] private UnityEvent onClose;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !GameManager.Instance.IsGamePaused)
        {
            OpenMenu();
        }
    }

    public void OpenMenu()
    {
        AudioManager.Instance.ReduceCurrBgVolumeInMenu();
        
        GameplayCanvas.SetActive(false);
        PlayerContainer.SetActive(false);
        FoodContainer.SetActive(false);
        PowerUpContainer.SetActive(false);
        
        AudioManager.Instance.PlaySound(AudioType.SceneTransition);
        GameManager.Instance.PauseGame(true);
        PauseMenuCanvas.SetActive(true);
    }

    public void CloseMenu()
    {
        PauseMenuCanvas.SetActive(false);
        GameManager.Instance.PauseGame(false);
        onClose?.Invoke();
        
        AudioManager.Instance.PlaySound(AudioType.SceneTransition);
        AudioManager.Instance.ResetCurrBgVolumeAfterMenu();
        
        GameplayCanvas.SetActive(true);
        PlayerContainer.SetActive(true);
        FoodContainer.SetActive(true);
        PowerUpContainer.SetActive(true);
    }
    
    public void OnHome()
    {
        AudioManager.Instance.PlaySound(AudioType.SceneTransition);
        
        SceneManager.LoadScene("Home");
    }

    public void OnRestart()
    {
        AudioManager.Instance.PlaySound(AudioType.SceneTransition);
        
        AudioManager.Instance.PlayMusic(true, volReduceFactorVal: 1f);
        
        SceneManager.LoadScene("Game");
    }

    public void OnSettings()
    {
        PauseMenuCanvas.SetActive(false);
        
        settingsMenu.OpenMenu();
    }

    public void OnHelp()
    {
        PauseMenuCanvas.SetActive(false);
        
        AudioManager.Instance.PlaySound(AudioType.SceneTransition);
        HelpMenuCanvas.SetActive(true);
    }
    
    public void OnBackHelpMenu()
    {
        HelpMenuCanvas.SetActive(false);
        
        OnBackFromAnotherMenu();
    }

    public void OnBackFromAnotherMenu()
    {
        AudioManager.Instance.PlaySound(AudioType.SceneTransition);
        PauseMenuCanvas.SetActive(true);
    }

    public void OnBack()
    {
        CloseMenu();
    }
}
