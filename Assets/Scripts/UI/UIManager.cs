using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private Canvas GamePlayCanvas;
    [SerializeField] private Canvas GameOverCanvas;
    
    [SerializeField] private Transform PlayerPanelContainer;
    [SerializeField] private PlayerGamePanel PlayerPanelPrefab;

    [Serializable]
    private struct ResultColor
    {
        public GameResult result;
        public Color color;
    }
    [SerializeField] private List<ResultColor> resultColors;
    
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
        for (int i = 0; i < playerPanels.Count; i++)
        {
            Destroy(playerPanels[i]);    
        }
        
        playerPanels.Clear();
        
        // evenly position the player panels
        float parentWidth = PlayerPanelContainer.GetComponent<RectTransform>().rect.width;
        float panelWidth = PlayerPanelPrefab.GetComponent<RectTransform>().rect.width;

        var spacing = players.Count == 1
            ? 0
            : (parentWidth - panelWidth) / (players.Count - 1);

        for (int i = 0; i < players.Count; i++)
        {
            PlayerGamePanel panel = Instantiate(PlayerPanelPrefab, PlayerPanelContainer, false);
            panel.SetPlayerID(players[i].PlayerID);
            panel.SetPlayerIndicator(players[i].Color.HeadColor);
            playerPanels.Add(panel);
            
            RectTransform panelRect = panel.GetComponent<RectTransform>();

            float xPos = (-parentWidth / 2) + ((i * spacing) + (panelWidth / 2));
            
            panelRect.anchoredPosition = new Vector2(xPos, 0f);
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

    public void OnGameOver(GameResult result, string message)
    {
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlaySound(AudioType.SceneTransition);
        
        AudioManager.Instance.PlaySound(AudioType.OnGameOver);
        
        GamePlayCanvas.enabled = false;
        GameOverCanvas.enabled = true;
        GameOverCanvas.GetComponent<GameOverMenu>().ShowResult(result, message, GetColorByResult(result));
    }
    
    private Color GetColorByResult(GameResult result)
    {
        ResultColor found = resultColors.Find(item => item.result == result);

        // If a match is not found, return a default color
        if (found.Equals(default(ResultColor)))
        {
            return Color.white;
        }

        return found.color;
    }
}
