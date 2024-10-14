using UnityEngine;

public class SnakeSegment : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;

    public int PlayerID { get; private set;  }
        
    protected virtual void Awake()
    {
        if (!sprite)
            sprite = GetComponent<SpriteRenderer>();
    }

    public void SetColor(Color color)
    {
        sprite.color = color;
    }

    public void SetPlayerID(int id)
    {
        PlayerID = id;
    }
}