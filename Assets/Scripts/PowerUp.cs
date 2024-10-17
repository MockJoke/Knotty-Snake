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
    
    // [SerializeField] private Collider2D objCollider;
    // public Collider2D Collider => objCollider;
    
    [SerializeField, Range(1, 10)] private int scoreMultiplier = 2;
    public int ScoreMultiplier => scoreMultiplier;
    
    [SerializeField] private float speedMultiplier = 1.5f;
    public float SpeedMultiplier => speedMultiplier;

    private Vector2Int Position;

    // void Awake()
    // {
    //     if (objCollider == null)
    //         objCollider = GetComponent<Collider2D>();
    // }
    
    // public void OnCollect(SnakeController snake)
    // {
    //     switch (powerUpType)
    //     {
    //         case PowerUpType.Shield:
    //             snake.ActivateShield(effectDuration);
    //             break;
    //         case PowerUpType.ScoreBoost:
    //             snake.BoostScore(scoreBoostAmount);
    //             break;
    //         case PowerUpType.SpeedUp:
    //             snake.SpeedUp(speedMultiplier, effectDuration);
    //             break;
    //     }
    //     
    //     this.gameObject.SetActive(false);
    // }

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