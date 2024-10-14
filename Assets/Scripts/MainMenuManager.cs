using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button SinglePlayerGameBtn;
    [SerializeField] private Button CoOpGameBtn;
    [SerializeField] private Button SettingsBtn;
    [SerializeField] private Button HelpBtn;

    void Awake()
    {
        if (SinglePlayerGameBtn)
            SinglePlayerGameBtn.onClick.AddListener(OnSinglePlayerGame);
        
        if (CoOpGameBtn)
            CoOpGameBtn.onClick.AddListener(OnCoOpGame);
        
        if (SettingsBtn)
            SettingsBtn.onClick.AddListener(OnSettings);
        
        if (HelpBtn)
            HelpBtn.onClick.AddListener(OnHelp);
    }

    private void OnSinglePlayerGame()
    {
        GameSettings.Instance.SetGameMode(GameMode.SinglePlayer);
        LoadGameScene();
    }

    private void OnCoOpGame()
    {
        GameSettings.Instance.SetGameMode(GameMode.CoOp);
        LoadGameScene();
    }
    
    private void OnSettings()
    {
        
    }

    private void OnHelp()
    {
        
    }
    
    private void LoadGameScene()
    {
        SceneManager.LoadScene($"Game");
    }
}
