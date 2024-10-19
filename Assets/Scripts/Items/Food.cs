using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Food : MonoBehaviour, ISpawnable, ICollectible
{
    public enum FoodType
    {
        MassGainer,
        MassBurner
    }

    [SerializeField] private FoodType foodType;
    public FoodType Type => foodType;
    
    [SerializeField, Range(0f, 1f)] private float spawnProbability = 0.5f;
    public float SpawnProbability => spawnProbability;
    
    [SerializeField, Range(1, 10)] private int lengthChangeAmt = 1;
    public int LengthChangeAmount => lengthChangeAmt;
    
    [SerializeField, Range(1, 30)] private float lifeTime = 10;
    public float LifeTime => lifeTime;

    [SerializeField] private int scoreGainAmt = 10;
    public int ScoreGainAmount => scoreGainAmt;
    
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