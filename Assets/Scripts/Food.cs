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
    
    [SerializeField, Range(1, 10)] private int lengthChangeAmt;
    public int LengthChangeAmount => lengthChangeAmt;
    
    [SerializeField, Range(1, 30)] private float lifeTime;
    public float LifeTime => lifeTime;
    
    private Vector2Int Position;
    
    // [SerializeField] private Collider2D objCollider;
    // public Collider2D Collider => objCollider;
    
    // void Awake()
    // {
    //     if (objCollider == null)
    //         objCollider = GetComponent<Collider2D>();
    // }
    
    // public void OnCollect(SnakeController snake)
    // {
    //     switch (foodType)
    //     {
    //         case FoodType.MassGainer:
    //             snake.IncreaseLength(lengthChangeAmt);
    //             break;
    //         case FoodType.MassBurner:
    //             snake.DecreaseLength(lengthChangeAmt);
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