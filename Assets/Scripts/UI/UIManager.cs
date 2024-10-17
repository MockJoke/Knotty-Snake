using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [SerializeField] private Transform PlayerPanelContainer;
    [SerializeField] private PlayerGamePanel playerPanelPrefab;

    private List<PlayerGamePanel> playerPanels = new List<PlayerGamePanel>();
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void Initialize(List<PlayerData> players)
    {
        playerPanels.Clear();
        
        for (int i = 0; i < players.Count; i++)
        {
            PlayerGamePanel panel = Instantiate(playerPanelPrefab, PlayerPanelContainer, true);
            panel.SetPlayerID(players[i].PlayerID);
            panel.SetPlayerIndicator(players[i].Color.HeadColor);
            playerPanels.Add(panel);
        }
    }

    public void DisplayScore(int id, int score)
    {
        playerPanels[id-1].SetScore(score);
    }
    
    public void DisplayShieldIndicator(int id, bool toggle)
    {
        playerPanels[id-1].ToggleShieldIndicator(toggle);
    }
    
    public void DisplaySpeedBoostIndicator(int id, bool toggle)
    {
        playerPanels[id-1].ToggleSpeedBoostIndicator(toggle);
    }
    
    public void DisplayScoreBoostIndicator(int id, bool toggle)
    {
        playerPanels[id-1].ToggleScoreBoostIndicator(toggle);
    }
}
