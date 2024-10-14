using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    public PlayerData PlayerData { get; private set; }
    
    [Header("Body Parts")]
    [SerializeField] private SnakeSegment HeadSegment;
    [SerializeField] private SnakeSegment BodySegment;
    [Tooltip("Initial snake body size")] [Range(0,5)] public int InitSnakeSize = 3;          
    
    [Header("Movement Fields")]
    [Range(1, 20)] public float InitSnakeSpeed = 5f;
    
    private float currSnakeSpeed;
    private float moveTimer;
    private bool isSpeedBoosted = false;
    private Coroutine speedBoostCoroutine;

    private bool isShieldActive = false;
    
    private int score = 0;
    
    private List<SnakeSegment> segments;
    
    private Vector2Int moveDirection;
    private Vector2Int playerPosition;

    #region MonoBehaviour Methods

    void Update()
    {
        if (PlayerData.IsAlive)
        {
            SetInputDirection();
        }
    }

    void FixedUpdate()
    {
        if (PlayerData.IsAlive)
        {
            Move();
        }
    }

    #endregion

    #region Setup related Methods

    public void Initialize()
    {
        segments = new List<SnakeSegment>();
        InitializeSnakeBody();

        switch (PlayerData.PlayerID)
        {
            case 1:
                moveDirection = Vector2Int.right;
                break;
            case 2:
                moveDirection = Vector2Int.left;
                break;
        }
        
        currSnakeSpeed = InitSnakeSpeed;
        moveTimer = 1f / currSnakeSpeed;
    }

    public void SetPlayerData(PlayerData data)
    {
        PlayerData = data;
    }

    private void InitializeSnakeBody()
    {
        for (int i = 0; i < segments.Count; i++)
        {
            Destroy(segments[i].gameObject);
        }

        segments.Clear();

        for (int i = 0; i < InitSnakeSize; i++)
        {
            SnakeSegment segment;

            if (i == 0)
            {
                segment = Instantiate(HeadSegment.transform, this.transform, true).GetComponent<SnakeSegment>();
                segment.SetColor(PlayerData.Color.HeadColor);
            }
            else
            {
                segment = Instantiate(BodySegment.transform, this.transform, true).GetComponent<SnakeSegment>();
                segment.SetColor(PlayerData.Color.BodyColor);
            }
            
            segments.Add(segment);
        }
        
        segments[0].transform.position = Vector3.zero;
    }

    #endregion

    #region Movement
    
    private void SetInputDirection()
    {
        // Only allow turning up or down while moving in the x-dir
        if (moveDirection.x != 0f)
        {
            if (Input.GetKeyDown(PlayerData.InputKeyBinding.UpKey))
            {
                moveDirection = Vector2Int.up;
            }
            else if (Input.GetKeyDown(PlayerData.InputKeyBinding.DownKey))
            {
                moveDirection = Vector2Int.down;
            }
        }

        // Only allow turning left or right while moving in the y-dir
        else if (moveDirection.y != 0f)
        {
            if (Input.GetKeyDown(PlayerData.InputKeyBinding.LeftKey))
            {
                moveDirection = Vector2Int.left;
            }
            else if (Input.GetKeyDown(PlayerData.InputKeyBinding.RightKey))
            {
                moveDirection = Vector2Int.right;
            }
        }
    }
    
    private void Move()
    {
        moveTimer -= Time.fixedDeltaTime;

        if (moveTimer <= 0f)
        {
            UpdateMovement();
            ScreenWrap();

            moveTimer = 1f / currSnakeSpeed;
        }
    }

    private void UpdateMovement()
    {
        Vector3 previousPosition = segments[0].transform.position;
        
        // Updating the head position
        playerPosition.x = Mathf.RoundToInt(segments[0].transform.position.x) + this.moveDirection.x;
        playerPosition.y = Mathf.RoundToInt(segments[0].transform.position.y) + this.moveDirection.y;
        segments[0].transform.position = new Vector2(playerPosition.x, playerPosition.y);
        
        // Make the snake body move forward, each segment is following the one in front of it 
        for (int i = 1; i < segments.Count; i++)
        {
            Vector3 currentSegmentPosition = segments[i].transform.position;
            segments[i].transform.position = previousPosition;
            previousPosition = currentSegmentPosition;
        }
    }

    private void ScreenWrap()
    {
        Bounds bounds = GameManager.Instance.GetWrappedAreaBounds();

        // screen wrapping in x dir
        if(segments[0].transform.position.x >= bounds.max.x)
        {
            segments[0].transform.position = new Vector2(bounds.min.x, segments[0].transform.position.y);  
        }
        else if (transform.position.x <= bounds.min.x)
        {
            segments[0].transform.position = new Vector2(bounds.max.x, segments[0].transform.position.y);         
        }

        // screen wrapping in y dir
        if (segments[0].transform.position.y >= bounds.max.y)
        {
            segments[0].transform.position = new Vector2(segments[0].transform.position.x, bounds.min.y);           
        }
        else if (transform.position.y <= bounds.min.y)
        {
            segments[0].transform.position = new Vector2(segments[0].transform.position.x, bounds.max.y);          
        }
    }

    #endregion

    #region Food & Powerups

    public void OnItemCollection(ICollectible item)
    {
        item.OnCollect(this);
    }

    public void OnSelfCollision()
    {
        if (!isShieldActive)
        {
            PlayerData.MarkAsDead();
        }
    }
    
    public void IncreaseLength(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            SnakeSegment segment = Instantiate(BodySegment.transform, this.transform, true).GetComponent<SnakeSegment>();
            segment.transform.position = segments[segments.Count - 1].transform.position;
            segments.Add(segment);
        }
    }

    public void DecreaseLength(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Destroy(segments[segments.Count - 1].gameObject);
            segments.RemoveAt(segments.Count - 1);
        }
    }
    
    public void ActivateShield(float duration)
    {
        StartCoroutine(ShieldCoroutine(duration));
    }

    public void BoostScore(int amount)
    {
        score += amount;
        UIManager.Instance.DisplayScore(score);
    }

    public void SpeedUp(float speedMultiplier, float duration)
    {
        if (isSpeedBoosted && speedBoostCoroutine != null)
        {
            StopCoroutine(speedBoostCoroutine);
        }
        
        speedBoostCoroutine = StartCoroutine(SpeedBoostCoroutine(speedMultiplier, duration));
    }
    
    private IEnumerator ShieldCoroutine(float duration)
    {
        isShieldActive = true;
        yield return new WaitForSeconds(duration);
        isShieldActive = false;
    }
    
    private IEnumerator SpeedBoostCoroutine(float speedMultiplier, float duration)
    {
        isSpeedBoosted = true;
        currSnakeSpeed *= speedMultiplier;
        
        yield return new WaitForSeconds(duration);

        currSnakeSpeed = InitSnakeSpeed;
        isSpeedBoosted = false;
    }

    #endregion
}