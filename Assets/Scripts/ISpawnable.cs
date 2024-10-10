using UnityEngine;

public interface ISpawnable
{
    public float LifeTime { get; }
    public Collider2D Collider { get; }
}