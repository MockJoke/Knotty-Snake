using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemSpawner<T> where T : MonoBehaviour, ISpawnable
{
    private Collider2D spawningArea;
    private Transform parentTransform;
    private List<T> itemPrefabs;
    
    private float spawnRate;
    private float nextSpawnTime;
    
    private Queue<T> itemPool = new Queue<T>();
    private List<T> activeItems = new List<T>();

    public ItemSpawner(Collider2D area, Transform parent, List<T> prefabs, float rate)
    {
        spawningArea = area;
        parentTransform = parent;
        itemPrefabs = prefabs;
        spawnRate = rate;
        
        // Initialize the pool with at least one of each type of item to ensure diversity
        InitializeItemPool();
    }
    
    private void InitializeItemPool()
    {
        for (int i = 0; i < itemPrefabs.Count; i++)
        {
            T newItem = GameObject.Instantiate(itemPrefabs[i], parentTransform, true);
            newItem.gameObject.SetActive(false);
            itemPool.Enqueue(newItem);
        }
    }
    
    public void StartSpawning()
    {
        nextSpawnTime = Time.time + Random.Range(spawnRate - 1, spawnRate + 1);
    }

    public void UpdateSpawner()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnItem();
            nextSpawnTime = Time.time + Random.Range(spawnRate - 1, spawnRate + 1);
        }
    }

    private void SpawnItem()
    {
        T newItem = GetPooledItem();
        
        newItem.transform.SetParent(parentTransform);
        newItem.gameObject.SetActive(true);
        activeItems.Add(newItem);
        
        Vector3 spawnPosition = GetValidSpawnPosition();
        newItem.SetPosition(new Vector2Int((int)spawnPosition.x, (int)spawnPosition.y));
        
        CoroutineRunner.Instance.StartCoroutine(DestroyItemAfterLifetime(newItem, Random.Range(newItem.LifeTime - 1, newItem.LifeTime + 1)));
    }

    private T GetPooledItem()
    {
        if (itemPool.Count > 0)
        {
            return itemPool.Dequeue();
        }
        else
        {
            // If the pool is empty, refill with a balanced selection of items
            RefillPool();
            return itemPool.Dequeue();
        }
    }
    
    private void RefillPool()
    {
        List<int> usedIndexes = new List<int>();
        
        // Refill the pool with an even distribution of all item types
        for (int i = 0; i < itemPrefabs.Count; i++)
        {
            if (!usedIndexes.Contains(i))
            {
                T newItem = GameObject.Instantiate(itemPrefabs[i], parentTransform, true);
                newItem.gameObject.SetActive(false);
                itemPool.Enqueue(newItem);
                usedIndexes.Add(i);
            }
        }

        // Optionally add random duplicates to fill the pool to a desired size
        while (itemPool.Count < itemPrefabs.Count * 2)
        {
            int randomIndex = Random.Range(0, itemPrefabs.Count);
            T newItem = GameObject.Instantiate(itemPrefabs[randomIndex], parentTransform, true);
            newItem.gameObject.SetActive(false);
            itemPool.Enqueue(newItem);
        }
    }

    private IEnumerator DestroyItemAfterLifetime(T item, float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        item.gameObject.SetActive(false);
        itemPool.Enqueue(item);
        activeItems.Remove(item);
    }
    
    private Vector3 GetValidSpawnPosition()
    {
        Vector3 spawnPosition;
        int maxAttempts = 5;
        int attempts = 0;

        do
        {
            spawnPosition = GetRandomSpawnPosition();
            attempts++;
        } 
        while (IsOverlappingOtherItems(spawnPosition) && attempts < maxAttempts);

        return spawnPosition;
    }
    
    private Vector3 GetRandomSpawnPosition()
    {
        Bounds bounds = spawningArea.bounds;
        
        int x = Mathf.RoundToInt(Random.Range(bounds.min.x, bounds.max.x));
        int y = Mathf.RoundToInt(Random.Range(bounds.min.y, bounds.max.y));
        
        return new Vector3(x, y, 0f);
    }
    
    private bool IsOverlappingOtherItems(Vector3 position)
    {
        if (activeItems.Count > 0)
        {
            for (int i = 0; i < activeItems.Count; i++)
            {
                if (activeItems[i].transform.position == position)
                {
                    return true;
                }    
            }
        }

        return false;
    }

    public List<T> GetActiveItems()
    {
        List<T> items = new List<T>();
        
        for (int i = 0; i < activeItems.Count; i++)
        {
            if (activeItems[i].gameObject.activeInHierarchy)
            {
                items.Add(activeItems[i]);
            }
        }
        
        activeItems.Clear();
        activeItems = items;
        
        return activeItems;
    }
}