using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public BoxCollider2D SpawningArea;
    [SerializeField] private SnakeHandler[] player; 

    private Vector2Int itemPosition; 
    public enum FoodType
    {
        massGainer, 
        massBurner
    }

    public enum PowerUpType
    {
        Shield,
        ScoreBoost,
        SpeedUp
    }

    public void Start()
    {
        spawnItem(); 
    }
    private void spawnItem()
    {
        Bounds bounds = this.SpawningArea.bounds;

        // item shouldn't be spawned at the player position
        do
        {
            itemPosition.x = Mathf.RoundToInt(Random.Range(bounds.min.x, bounds.max.x));
            itemPosition.y = Mathf.RoundToInt(Random.Range(bounds.min.y, bounds.max.y));
        }
        while (player[0].GetPlayerPositionList().IndexOf(itemPosition) != -1); 
        
        this.transform.position = new Vector2(itemPosition.x, itemPosition.y);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        spawnItem(); 
    }
}
