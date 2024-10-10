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
    private Vector2Int playerPosition; 
    
    public PlayerID playerID;
    public LifeStatus lifeStatus;

    [SerializeField] private BoxCollider2D WrappedArea; 

    // [Tooltip("Amount of body segments by which snake size will increase/decrease on consuming food")]
    // [Range(1, 3)]  public int segmentCnt; 
    [Tooltip("Initial snake body size")] [Range(0,5)] public int InitialSnakeSize = 1;          

    private List<Vector2Int> playerPositionList;     //list of positions where the snake has been
    private List<Transform> bodySegmentList;
    [SerializeField] private GameObject snakeBodyContainer; 
    public Transform bodySegment;           // prefab 
    [SerializeField] public Transform SnakeHead; 

    private void Start()
    {
        bodySegmentList = new List<Transform>();        //initializing the list
        initializeSnakeBody(); 
        
        moveDirection = Vector2Int.right;           // initiating the player direction 

        lifeStatus = LifeStatus.Alive; 
    }

    private void initializeSnakeBody()
    {
        for (int i = 1; i < bodySegmentList.Count; i++)
        {
            Destroy(bodySegmentList[i].gameObject);
        }

        bodySegmentList.Clear();
        bodySegmentList.Add(SnakeHead.transform);

        for (int i = 1; i < InitialSnakeSize; i++)
        {
            Transform segment = Instantiate(bodySegment);
            bodySegmentList.Add(segment);
            segment.transform.SetParent(snakeBodyContainer.transform);
        }

        SnakeHead.transform.position = Vector3.zero;
    }

    public void InputDirection()
    {
        // Only allow turning up or down while moving in the x-dir
        if (this.moveDirection.x != 0f)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                moveDirection = Vector2Int.up;
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                moveDirection = Vector2Int.down;
            }
        }

        // Only allow turning left or right while moving in the y-dir
        else if (this.moveDirection.y != 0f)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                moveDirection = Vector2Int.right;
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                moveDirection = Vector2Int.left;
            }
        }
    }

    public void Movement()
    {
        // Make the snake body move forward, each segment is following the one in front of it 
        for (int i = bodySegmentList.Count - 1; i > 0; i--)
        {
            bodySegmentList[i].position = bodySegmentList[i - 1].position;
        }
        
        // Updating the head position
        playerPosition.x = Mathf.RoundToInt(this.transform.position.x) + this.moveDirection.x;
        playerPosition.y = Mathf.RoundToInt(this.transform.position.y) + this.moveDirection.y;
        this.transform.position = new Vector2(playerPosition.x, playerPosition.y);
        
        ScreenWrap(); 
    }

    private void ScreenWrap()
    {
        Bounds bounds = this.WrappedArea.bounds;

        //screen wrapping in x dir
        if(transform.position.x >= bounds.max.x)
        {
            this.transform.position = new Vector2(bounds.min.x, this.transform.position.y);  
        }
        else if (transform.position.x <= bounds.min.x)
        {
            this.transform.position = new Vector2(bounds.max.x, this.transform.position.y);         
        }

        //screen wrapping in y dir
        if (transform.position.y >= bounds.max.y)
        {
            this.transform.position = new Vector2(this.transform.position.x, bounds.min.y);           
        }
        else if (transform.position.y <= bounds.min.y)
        {
            this.transform.position = new Vector2(this.transform.position.x, bounds.max.y);          
        }     
    }

    private void AddBodySegment(int count)
    {
        for(int i=1; i < count; i++)
        {
            Transform segment = Instantiate(bodySegment, snakeBodyContainer.transform, true);
            segment.position = bodySegmentList[^1].position;
            bodySegmentList.Add(segment);
        }
    }
    
    private void RemoveBodySegment(int count)
    {
        for (int i = 1; i < count; i++)
        {
            Destroy(bodySegmentList[^1].gameObject);
            bodySegmentList.RemoveAt(bodySegmentList.Count - 1);
        }            
    }
    
    public Vector2Int GetPlayerPosition()
    {
        return playerPosition; 
    }

    public List<Vector2Int> GetPlayerPositionList()
    {
        playerPositionList = new List<Vector2Int>() { playerPosition };
        playerPositionList.Add(playerPosition); 
       
        return playerPositionList;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag($"MassGainer"))
        {
            Food food = collision.gameObject.GetComponent<Food>();

            int count = 1;
            
            if (food != null)
            {
                count = food.lengthChangeAmt;
            }
            
            AddBodySegment(count); 
        }
    }
}
