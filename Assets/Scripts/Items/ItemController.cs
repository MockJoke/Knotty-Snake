using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController<T> : MonoBehaviour where T : MonoBehaviour, ISpawnable
{
    [SerializeField] private Collider2D SpawningArea;
    [SerializeField] private List<T> ItemPrefabs;
    [SerializeField] private float spawnRate;
    
    protected ItemSpawner<T> itemSpawner;
    
    void Start()
    {
        itemSpawner = new ItemSpawner<T>(SpawningArea, this.transform, ItemPrefabs, spawnRate);
        
        itemSpawner.StartSpawning();
    }
    
    void Update()
    {
        if (GameManager.Instance.IsGameOver)
            return;
        
        itemSpawner.UpdateSpawner();
    }

    public List<T> GetItems()
    {
        return itemSpawner.GetActiveItems();
    }

    public void OnItemCollect(T item)
    {
        itemSpawner.RecycleItem(item);
    }
}