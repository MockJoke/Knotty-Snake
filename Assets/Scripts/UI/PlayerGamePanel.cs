using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGamePanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI PlayerID;
    [SerializeField] private Image PlayerIndicator;
    
    [SerializeField] private TextMeshProUGUI Score;
    
    [SerializeField] private Image ShieldIndicator;
    [SerializeField] private Image SpeedBoostIndicator;
    [SerializeField] private Image ScoreBoostIndicator;

    void Start()
    {
        ShieldIndicator.gameObject.SetActive(false);
        SpeedBoostIndicator.gameObject.SetActive(false);
        ScoreBoostIndicator.gameObject.SetActive(false);
        
        SetScore(0);
    }

    public void SetPlayerID(int id)
    {
        PlayerID.text = $"P{id}";
    }

    public void SetPlayerIndicator(Color color)
    {
        PlayerIndicator.color = color;
    }
    
    public void SetScore(int score)
    {
        Score.text = $"Score: <color=#F8D205>{score}</color>";
    }

    public void ToggleShieldIndicator(bool toggle)
    {
        ShieldIndicator.gameObject.SetActive(toggle);
    }
    
    public void ToggleSpeedBoostIndicator(bool toggle)
    {
        SpeedBoostIndicator.gameObject.SetActive(toggle);
    }
    
    public void ToggleScoreBoostIndicator(bool toggle)
    {
        ScoreBoostIndicator.gameObject.SetActive(toggle);
    }
}
