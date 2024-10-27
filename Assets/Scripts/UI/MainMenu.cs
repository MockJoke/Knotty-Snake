using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Menu Containers")] 
    [SerializeField] private GameObject MainMenuCanvas;
    [SerializeField] private GameObject MainMenuDecoration;
    [SerializeField] private GameObject HelpMenuCanvas;
    [SerializeField] private SettingsMenu SettingsMenu;

    private void Start()
    {
        AudioManager.Instance.PlayMusic(true, volReduceFactorVal: 2.5f);
    }

    public void OpenHomeMenu()
    {
        MainMenuCanvas.SetActive(true);
        MainMenuDecoration.SetActive(true);
    }

    public void CloseHomeMenu()
    {
        MainMenuCanvas.SetActive(false);
        MainMenuDecoration.SetActive(false);
    }
    
    public void OnSinglePlayerGame()
    {
        GameSettings.Instance.SetGameMode(GameMode.SinglePlayer);
        AudioManager.Instance.SetVolReduceFactor(1f);
        LoadGameScene();
    }

    public void OnCoOpGame()
    {
        GameSettings.Instance.SetGameMode(GameMode.CoOp);
        AudioManager.Instance.SetVolReduceFactor(1f);
        LoadGameScene();
    }
    
    public void OnSettings()
    {
        CloseHomeMenu();
        SettingsMenu.OpenMenu();
    }

    public void OnHelp()
    {
        CloseHomeMenu();
        AudioManager.Instance.PlaySound(AudioType.SceneTransition);
        HelpMenuCanvas.SetActive(true);
    }

    public void OnBackHelpMenu()
    {
        HelpMenuCanvas.SetActive(false);
        MainMenuCanvas.SetActive(true);
        MainMenuDecoration.SetActive(true);
    }
    
    private void LoadGameScene()
    {
        AudioManager.Instance.PlaySound(AudioType.SceneTransition);
        SceneManager.LoadScene($"Game");
    }
}
