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
}