using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public int PlayerID { get; private set; }
    public bool IsAlive { get; private set; }
    
    public KeyBinding InputKeyBinding { get; private set; }
    public PlayerColor Color { get; private set; }
    public Vector2Int InitPos { get; private set; }
    
    public SnakeController SnakeController { get; private set; }

    private List<PowerUp.PowerUpType> activePowerUps; 

    public PlayerData(int playerID, KeyBinding keys, PlayerColor color, Vector2Int pos, SnakeController snakeController)
    {
        PlayerID = playerID;
        IsAlive = true;
        InputKeyBinding = keys;
        Color = color;
        InitPos = pos;
        activePowerUps = new List<PowerUp.PowerUpType>();
        
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

    public void AddPowerUp(PowerUp.PowerUpType powerUp)
    {
        activePowerUps.Add(powerUp);
    }

    public void RemovePowerUp(PowerUp.PowerUpType powerUp)
    {
        activePowerUps.Remove(powerUp);
    }

    public bool IsShieldActive()
    {
        return activePowerUps.Contains(PowerUp.PowerUpType.Shield);
    }
    
    public bool IsSpeedBoosted()
    {
        return activePowerUps.Contains(PowerUp.PowerUpType.SpeedUp);
    }
    
    public bool IsScoreBoosted()
    {
        return activePowerUps.Contains(PowerUp.PowerUpType.ScoreBoost);
    }
}

[Serializable]
public struct PlayerColor
{
    public Color HeadColor;
    public Color BodyColor;
}