using UnityEngine;

public class SnakeSegment : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    
    public FlickerEffect flickerEffect;
    
    protected Vector2Int Position;
        
    protected virtual void Awake()
    {
        if (!sprite)
            sprite = GetComponent<SpriteRenderer>();
    }

    public void SetPosition(Vector2Int pos)
    {
        Position = pos;
        transform.localPosition = new Vector3(Position.x, Position.y, 0f);
    }
    
    public Vector2Int GetPosition()
    {
        return Position;
    }

    public void SetColor(Color color)
    {
        sprite.color = color;
    }
}