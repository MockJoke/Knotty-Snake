using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [SerializeField] private BoxCollider2D WrappedArea;
    [SerializeField] private SnakeController snakePrefab;

    [SerializeField] private KeyBinding[] InputKeyBindings;
    [SerializeField] private PlayerColor[] PlayerColors;
    
    private List<Vector3> playerStartPositions = new List<Vector3>();
    private List<PlayerData> players = new List<PlayerData>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        int playerCount = 0;
        
        switch (GameSettings.Instance.SelectedGameMode)
        {
            case GameMode.SinglePlayer:
                playerCount = 1;
                break;
            case GameMode.CoOp:
                playerCount = 2;
                break;
        }
        
        SpawnPlayers(playerCount);
    }

    private void SpawnPlayers(int count)
    {
        players.Clear();
        
        SetPlayerPositions(count);
        
        for (int i = 0; i < count; i++)
        {
            GameObject snakeInstance = Instantiate(snakePrefab.gameObject, playerStartPositions[i], Quaternion.identity);
            PlayerData playerData = new PlayerData(i + 1, InputKeyBindings[i], PlayerColors[i], snakeInstance.GetComponent<SnakeController>());
            snakeInstance.GetComponent<SnakeController>().Initialize(playerData);
            
            players.Add(playerData);
        }

        // if (players.Count > 1)
        // {
        //     List<SnakeController> otherPlayers = new List<SnakeController>();
        //     
        //     for (int i = 0; i < count; i++)
        //     {
        //         otherPlayers.Clear();
        //         
        //         for (int j = 0; j < players.Count; j++)
        //         {
        //             if (i != j)
        //             {
        //                 otherPlayers.Add(players[j].SnakeController);
        //             }    
        //         }
        //         
        //         players[i].SnakeController.Initialize(players[i], otherPlayers);
        //     }
        // }
        // else
        // {
        //     players[0].SnakeController.Initialize(players[0]);
        // }
    }
    
    private void SetPlayerPositions(int count)
    {
        playerStartPositions.Clear();
        
        Vector3 wrappedCenter = WrappedArea.bounds.center;
        
        if (count == 1)
        {
            Vector3 pos = new Vector3(wrappedCenter.x, wrappedCenter.y, 0);
            playerStartPositions.Add(pos);
        }
        else if (count == 2)
        {
            float xOffset = WrappedArea.bounds.extents.x / 2;
            
            Vector3 pos1 = new Vector3(wrappedCenter.x - xOffset, wrappedCenter.y, 0);
            Vector3 pos2 = new Vector3(wrappedCenter.x + xOffset, wrappedCenter.y, 0);
            
            playerStartPositions.Add(pos1);
            playerStartPositions.Add(pos2);
        }
    }

    public void PlayerDied(PlayerData deadPlayerData)
    {
        deadPlayerData.MarkAsDead();
        CheckForWinCondition();
    }

    private void CheckForWinCondition()
    {
        List<PlayerData> alivePlayers = players.Where(p => p.IsAlive).ToList();
        
        if (alivePlayers.Count == 1)
        {
            DeclareWinner(alivePlayers[0]);
        }
        else if (alivePlayers.Count == 0)
        {
            DeclareDraw();
        }
    }

    private void DeclareWinner(PlayerData winner)
    {
        Debug.Log($"Player {winner.PlayerID} wins!");
    }

    private void DeclareDraw()
    {
        Debug.Log("It's a draw!");
    }

    public Bounds GetWrappedAreaBounds()
    {
        return WrappedArea.bounds;
    }
}
