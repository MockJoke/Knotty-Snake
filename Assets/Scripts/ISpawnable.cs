using UnityEngine;

public interface ISpawnable
{
    public float LifeTime { get; }
    public Vector2Int GetPosition();
    public void SetPosition(Vector2Int pos);
}