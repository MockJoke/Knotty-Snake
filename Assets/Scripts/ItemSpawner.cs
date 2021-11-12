using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private BoxCollider2D spawningArea;

    public enum FoodType
    {
        ripeFood, 
        rawFood
    }

    public enum PowerUpType
    {
        Shield,
        ScoreBoost,
        SpeedUp
    }

    public void Start()
    {
        spawnFood(); 
    }
    private void spawnFood()
    {
        Bounds bounds = this.spawningArea.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        this.transform.position = new Vector2(Mathf.Round(x), Mathf.Round(y));

        Invoke("spawnFood", 3f); 
    }

}
