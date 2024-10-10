using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : MonoBehaviour
{
    [SerializeField] private Collider2D SpawningArea;
    [SerializeField] private List<Food> FoodObjects;
    [SerializeField] private float spawnRate;
    // public float SpawnRate => spawnRate;
    
    private ItemSpawner<Food> itemSpawner;

    void Start()
    {
        itemSpawner = new ItemSpawner<Food>(SpawningArea, this.transform, FoodObjects, spawnRate);
        
        itemSpawner.StartSpawning();
    }
    
    void Update()
    {
        itemSpawner.UpdateSpawner();
    }
}