using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LifeStatus
{
    Alive,
    Dead
}

public enum PlayerID
{
    player1, 
    player2
}

public class SnakeHandler : MonoBehaviour
{
    private Vector2Int moveDirection;
    
    public PlayerID playerID;
    public LifeStatus lifeStatus;

    public List<Transform> bodySegments;
    public Transform bodySegment; 

    public void InputDirection()
    {
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            moveDirection = Vector2Int.up; 
        }
        else if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            moveDirection = Vector2Int.down; 
        }
        else if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveDirection = Vector2Int.right; 
        }
        else if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            moveDirection = Vector2Int.left; 
        }
    }

    public void Move()
    {
        float x = Mathf.Round(this.transform.position.x) + this.moveDirection.x;
        float y = Mathf.Round(this.transform.position.y) + this.moveDirection.y;
        this.transform.position = new Vector2(x, y);
    }
}
