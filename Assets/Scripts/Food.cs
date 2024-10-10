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
    
    [SerializeField] private Collider2D objCollider;
    public Collider2D Collider => objCollider;
    
    void Awake()
    {
        if (objCollider == null)
            objCollider = GetComponent<Collider2D>();
    }
}