using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private Image bgImage;
    [SerializeField] private TextMeshProUGUI Result;
    
    [Header("Buttons")]
    [SerializeField] private Button HomeBtn;
    [SerializeField] private Button RestartBtn;

    private GameResult gameResult;

    void Awake()
    {
        if (HomeBtn)
            HomeBtn.onClick.AddListener(OnHome);
        
        if (RestartBtn)
            RestartBtn.onClick.AddListener(OnRestart);
    }

    public void OnHome()
    {
        StopGameOverSound();
        AudioManager.Instance.PlaySound(AudioType.SceneTransition);
        AudioManager.Instance.PlayMusic(true, volReduceFactorVal: 2.5f);
        SceneManager.LoadScene("Home");
    }

    public void OnRestart()
    {
        StopGameOverSound();
        AudioManager.Instance.PlaySound(AudioType.SceneTransition);
        AudioManager.Instance.PlayMusic(true);
        SceneManager.LoadScene("Game");
    }

    public void ShowResult(GameResult result, string msg, Color color)
    {
        gameResult = result;
        
        Result.text = $"{msg}";
        Result.color = new Color(color.r, color.g, color.g, 1);
        bgImage.color = color;
    }

    private void StopGameOverSound()
    {
        AudioManager.Instance.StopSound(AudioType.OnGameOver);
    }
}