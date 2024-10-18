using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : MonoBehaviour
{
    [SerializeField] private Collider2D SpawningArea;
    [SerializeField] private List<Food> FoodObjects;
    [SerializeField] private float spawnRate;
    
    private ItemSpawner<Food> itemSpawner;
    
    void Start()
    {
        itemSpawner = new ItemSpawner<Food>(SpawningArea, this.transform, FoodObjects, spawnRate);
        
        itemSpawner.StartSpawning();
    }
    
    void Update()
    {
        if (GameManager.Instance.IsGameOver)
            return;
        
        itemSpawner.UpdateSpawner();
    }

    public List<Food> GetItems()
    {
        return itemSpawner.GetActiveItems();
    }

    public void OnItemCollect(Food item)
    {
        itemSpawner.RecycleItem(item);
    }
}