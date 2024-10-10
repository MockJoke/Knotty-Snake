using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Food : MonoBehaviour, ISpawnable
{
    public enum FoodType
    {
        MassGainer,
        MassBurner
    }

    public FoodType foodType;
    [Range(1, 10)] public int lengthChangeAmt;
    [SerializeField, Range(1, 30)] private float lifeTime;

    public float LifeTime => lifeTime;
}