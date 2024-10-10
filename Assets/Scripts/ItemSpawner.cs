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

    public ItemSpawner(Collider2D area, Transform parent, List<T> prefabs, float rate)
    {
        spawningArea = area;
        parentTransform = parent;
        itemPrefabs = prefabs;
        spawnRate = rate;
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
        newItem.transform.position = GetRandomSpawnPosition();
        newItem.gameObject.SetActive(true);

        // Use the item's specific LifeTime property
        CoroutineRunner.Instance.StartCoroutine(DestroyItemAfterLifetime(newItem, Random.Range(newItem.LifeTime - 1, newItem.LifeTime + 1)));
    }

    private T GetPooledItem()
    {
        if (itemPool.Count >= itemPrefabs.Count)
        {
            return itemPool.Dequeue();
        }
        else
        {
            int randomIndex = Random.Range(0, itemPrefabs.Count);
            return GameObject.Instantiate(itemPrefabs[randomIndex]);
        }
    }

    private IEnumerator DestroyItemAfterLifetime(T item, float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        item.gameObject.SetActive(false);
        itemPool.Enqueue(item);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Bounds bounds = this.spawningArea.bounds;
        
        int x = Mathf.RoundToInt(Random.Range(bounds.min.x, bounds.max.x));
        int y = Mathf.RoundToInt(Random.Range(bounds.min.y, bounds.max.y));
        
        return new Vector3(x, y, 0f);
    }
}