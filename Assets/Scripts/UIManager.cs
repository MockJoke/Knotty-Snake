using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance => instance;

    [SerializeField] private TextMeshProUGUI ScoreText;
    
    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void DisplayScore(int score)
    {
        ScoreText.text = $"Score: {score}";
    }
}
