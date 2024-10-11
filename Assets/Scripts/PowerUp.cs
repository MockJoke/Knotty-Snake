using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    [SerializeField, Range(1, 30)] private float lifeTime;
    public float LifeTime => lifeTime;
    
    [SerializeField, Range(1, 30)] private float effectDuration;
    public float EffectDuration => effectDuration;
    
    [SerializeField] private Collider2D objCollider;
    public Collider2D Collider => objCollider;
    
    [SerializeField] private int scoreBoostAmount = 50;
    [SerializeField] private float speedMultiplier = 1.5f;

    void Awake()
    {
        if (objCollider == null)
            objCollider = GetComponent<Collider2D>();
    }
    
    public void OnCollect(SnakeController snake)
    {
        switch (powerUpType)
        {
            case PowerUpType.Shield:
                snake.ActivateShield(effectDuration);
                break;
            case PowerUpType.ScoreBoost:
                snake.BoostScore(scoreBoostAmount);
                break;
            case PowerUpType.SpeedUp:
                snake.SpeedUp(speedMultiplier, effectDuration);
                break;
        }
        
        this.gameObject.SetActive(false);
    }
}