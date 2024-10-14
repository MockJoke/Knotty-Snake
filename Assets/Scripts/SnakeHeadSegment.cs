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
            if (bodySegment.PlayerID == this.PlayerID)
            {
                Debug.Log(bodySegment);
                snakeController.OnSelfCollision();
            }
        }
    }
}
