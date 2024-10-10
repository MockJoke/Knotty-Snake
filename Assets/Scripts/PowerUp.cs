using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PowerUp : MonoBehaviour, ISpawnable
{
    public enum PowerUpType
    {
        Shield,
        ScoreBoost,
        SpeedUp
    }

    public PowerUpType powerUpType;
    [SerializeField, Range(1, 30)] private float lifeTime;
    public float LifeTime => lifeTime;
    
    [SerializeField] private Collider2D objCollider;
    public Collider2D Collider => objCollider;

    void Awake()
    {
        if (objCollider == null)
            objCollider = GetComponent<Collider2D>();
    }
}