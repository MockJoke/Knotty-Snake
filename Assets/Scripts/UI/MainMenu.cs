using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Menu Canvases")] 
    [SerializeField] private GameObject MainMenuCanvas;
    [SerializeField] private GameObject MainMenuDecoration;
    [SerializeField] private GameObject HelpMenuCanvas;
    [SerializeField] private GameObject SettingsMenuCanvas;

    private void Start()
    {
        AudioManager.Instance.PlayMusic(true, volReduceFactor: 2.5f);
    }

    public void OnSinglePlayerGame()
    {
        GameSettings.Instance.SetGameMode(GameMode.SinglePlayer);
        AudioManager.Instance.ReduceBgMusicSourceVolume(1f);
        LoadGameScene();
    }

    public void OnCoOpGame()
    {
        GameSettings.Instance.SetGameMode(GameMode.CoOp);
        AudioManager.Instance.ReduceBgMusicSourceVolume(1f);
        LoadGameScene();
    }
    
    public void OnSettings()
    {
        MainMenuCanvas.SetActive(false);
        SettingsMenuCanvas.SetActive(true);
    }

    public void OnHelp()
    {
        MainMenuCanvas.SetActive(false);
        MainMenuDecoration.SetActive(false);
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
