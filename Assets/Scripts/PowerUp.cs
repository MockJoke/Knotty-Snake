using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class PowerUp : MonoBehaviour, ISpawnable, ICollectible
{
    public enum PowerUpType
    {
        Shield,
        ScoreBoost,
        SpeedUp
    }

    [SerializeField] private PowerUpType powerUpType;

    public PowerUpType Type => powerUpType;

    [SerializeField, Range(1, 30)] private float lifeTime;
    public float LifeTime => lifeTime;
    
    [SerializeField, Range(1, 30)] private float effectDuration;
    public float EffectDuration => effectDuration;
    
    [SerializeField, Range(1, 10)] private int scoreMultiplier = 2;
    public int ScoreMultiplier => scoreMultiplier;
    
    [SerializeField] private float speedMultiplier = 1.5f;
    public float SpeedMultiplier => speedMultiplier;
    
    public Coroutine DestroyCoroutine { get; set; }

    private Vector2Int Position;

    public void SetPosition(Vector2Int pos)
    {
        Position = pos;
        transform.position = new Vector3(pos.x, pos.y, 0f);
    }

    public Vector2Int GetPosition()
    {
        return Position;
    }
}