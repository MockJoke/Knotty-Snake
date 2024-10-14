using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeSegment : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;

    protected virtual void Awake()
    {
        if (!sprite)
            sprite = GetComponent<SpriteRenderer>();
    }

    public void SetColor(Color color)
    {
        sprite.color = color;
    }
}