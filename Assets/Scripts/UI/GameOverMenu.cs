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

    void Awake()
    {
        if (HomeBtn)
            HomeBtn.onClick.AddListener(OnHome);
        
        if (RestartBtn)
            RestartBtn.onClick.AddListener(OnRestart);
    }

    private void OnHome()
    {
        SceneManager.LoadScene("Home");
    }

    private void OnRestart()
    {
        SceneManager.LoadScene("Game");
    }

    public void SetBackgroundColor(Color color)
    {
        bgImage.color = color;
    }

    public void SetResult(string result)
    {
        Result.text = $"{result}";
    }
}
