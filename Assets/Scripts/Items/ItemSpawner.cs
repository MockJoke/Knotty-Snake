using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemSpawner<T> where T : MonoBehaviour, ISpawnable
{
    private Collider2D spawningArea;
    private Transform parentTransform;
    
    private float spawnRate;
    private float nextSpawnTime;
    
    private List<T> itemPrefabs;
    private List<T> itemPool = new List<T>();
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
            itemPool.Add(newItem);
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
        
        newItem.gameObject.SetActive(true);
        itemPool.Remove(newItem);
        activeItems.Add(newItem);
        
        Vector3 spawnPosition = GetValidSpawnPosition();
        newItem.SetPosition(new Vector2Int((int)spawnPosition.x, (int)spawnPosition.y));
        
        AudioManager.Instance.PlaySound(AudioType.ItemSpawn);
        
        newItem.DestroyCoroutine = CoroutineRunner.Instance.StartCoroutine(DestroyItemAfterLifetime(newItem, Random.Range(newItem.LifeTime - 1, newItem.LifeTime + 1)));
    }

    private T GetPooledItem()
    {
        if (itemPool.Count == 0)
        {
            RefillPool();
        }

        T item = SelectItemByProbability(itemPool);

        if (item)
        {
            return item;
        }

        // Fallback (in case of rounding issues), return the first item
        return itemPool[0];
    }
    
    private T SelectItemByProbability(List<T> itemsList)
    {
        float totalProbability = 0f;
    
        // Calculate total probability
        for (int i = 0; i < itemsList.Count; i++)
        {
            totalProbability += itemsList[i].SpawnProbability;
        }

        // Generate a random number within the total probability range
        float randomValue = Random.Range(0, totalProbability);
        float cumulativeProbability = 0f;
    
        // Select the item based on weighted probability
        for (int i = 0; i < itemsList.Count; i++)
        {
            cumulativeProbability += itemsList[i].SpawnProbability;
            if (randomValue <= cumulativeProbability)
            {
                return itemsList[i];
            }
        }

        return null;
    }
    
    private void RefillPool()
    {
        InitializeItemPool();
        
        // while (itemPool.Count < itemPrefabs.Count * 2)
        // {
        //     T newItem = SelectItemByProbability(itemPrefabs);
        //
        //     if (!newItem)
        //     {
        //         newItem = itemPrefabs[0];
        //     }
        //     
        //     newItem = GameObject.Instantiate(newItem, parentTransform, true);
        //     newItem.gameObject.SetActive(false);
        //     itemPool.Add(newItem);
        // }
    }

    private IEnumerator DestroyItemAfterLifetime(T item, float lifetime)
    {
        yield return new WaitForSeconds(lifetime);

        if (item)
        {
            item.gameObject.SetActive(false);
            activeItems.Remove(item);
            itemPool.Add(item);
        }
    }
    
    public void RecycleItem(T item)
    {
        if (activeItems.Contains(item))
        {
            CoroutineRunner.Instance.StopCoroutine(item.DestroyCoroutine);
            item.gameObject.SetActive(false);
            activeItems.Remove(item);
            itemPool.Add(item);
        }
    }

    #region Methods for Selecting Item SpawnPosition

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

    #endregion

    #region Getters

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

    #endregion
}