using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeHeadSegment : SnakeSegment
{
    private SnakeController snakeController;

    protected override void Awake()
    {
        base.Awake();

        if (!snakeController)
            snakeController = GetComponentInParent<SnakeController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ICollectible collectible = collision.GetComponent<ICollectible>();
        
        if (collectible != null)
        {
            snakeController.OnItemCollection(collectible);
        }
        
        SnakeBodySegment bodySegment = collision.GetComponent<SnakeBodySegment>();

        if (bodySegment)
        {
            snakeController.OnSelfCollision();
        }
    }
}
