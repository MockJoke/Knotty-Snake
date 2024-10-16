using System;
using UnityEngine;

public class PlayerData
{
    public int PlayerID { get; private set; }
    public bool IsAlive { get; private set; }
    
    public KeyBinding InputKeyBinding { get; private set; }
    public PlayerColor Color { get; private set; }
    public Vector2Int InitPos { get; private set; }
    
    public SnakeController SnakeController { get; private set; }

    public PlayerData(int playerID, KeyBinding keys, PlayerColor color, Vector2Int pos, SnakeController snakeController)
    {
        PlayerID = playerID;
        IsAlive = true;
        InputKeyBinding = keys;
        Color = color;
        InitPos = pos;
        
        SnakeController = snakeController;
    }

    public void MarkAsDead()
    {
        IsAlive = false;
    }
    
    public void MarkAsAlive()
    {
        IsAlive = true;
    }
}

[Serializable]
public struct PlayerColor
{
    public Color HeadColor;
    public Color BodyColor;
}