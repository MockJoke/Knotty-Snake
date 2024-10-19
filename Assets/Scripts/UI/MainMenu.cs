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

    public void OnSinglePlayerGame()
    {
        GameSettings.Instance.SetGameMode(GameMode.SinglePlayer);
        LoadGameScene();
    }

    public void OnCoOpGame()
    {
        GameSettings.Instance.SetGameMode(GameMode.CoOp);
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
        SceneManager.LoadScene($"Game");
    }
}
