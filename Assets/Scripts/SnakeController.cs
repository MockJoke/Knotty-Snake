using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    public PlayerData playerData { get; private set; }
    
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
    
    private List<SnakeSegment> segments = new List<SnakeSegment>();
    
    private Vector2Int moveDirection;
    private Vector2Int playerPosition;

    private FoodController foodController;
    private PowerUpController powerUpController;

    private List<SnakeController> otherPlayers = new List<SnakeController>();

    #region MonoBehaviour Methods

    void Awake()
    {
        foodController = FindObjectOfType<FoodController>();
        powerUpController = FindObjectOfType<PowerUpController>();
    }

    void Update()
    {
        if (playerData.IsAlive)
        {
            SetInputMoveDirection();
            HandleMovement();
            CheckCollisions();
        }
    }

    #endregion

    #region Init Setup Methods

    public void Initialize(PlayerData data, List<SnakeController> others = null)
    {
        playerData = data;
        otherPlayers = others;
        
        segments = new List<SnakeSegment>();

        switch (playerData.PlayerID)
        {
            case 1:
                moveDirection = Vector2Int.right;
                break;
            case 2:
                moveDirection = Vector2Int.left;
                break;
        }
        
        InitializeSnakeBody();
        
        currSnakeSpeed = InitSnakeSpeed;
        moveTimer = 1f / currSnakeSpeed;
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
            
            if (i == 0)     // Head Segment
            {
                segment = Instantiate(HeadSegment.transform, this.transform, true).GetComponent<SnakeSegment>();
                
                segment.SetPosition(playerData.InitPos);
                playerPosition = segment.GetPosition();
                
                segment.SetColor(playerData.Color.HeadColor);
            }
            else
            {
                segment = Instantiate(BodySegment.transform, this.transform, true).GetComponent<SnakeSegment>();
                
                // Set init pos for body segments so that at the start they don't trigger self collisions
                segment.SetPosition(new Vector2Int(playerPosition.x - moveDirection.x, playerPosition.y - moveDirection.y));
                
                segment.SetColor(playerData.Color.BodyColor);
            }
            
            segments.Add(segment);
        }
    }

    #endregion

    #region Movement
    
    private void SetInputMoveDirection()
    {
        // Only allow turning up or down while moving in the x-dir
        if (moveDirection.x != 0f)
        {
            if (Input.GetKeyDown(playerData.InputKeyBinding.UpKey))
            {
                moveDirection = Vector2Int.up;
            }
            else if (Input.GetKeyDown(playerData.InputKeyBinding.DownKey))
            {
                moveDirection = Vector2Int.down;
            }
        }

        // Only allow turning left or right while moving in the y-dir
        else if (moveDirection.y != 0f)
        {
            if (Input.GetKeyDown(playerData.InputKeyBinding.LeftKey))
            {
                moveDirection = Vector2Int.left;
            }
            else if (Input.GetKeyDown(playerData.InputKeyBinding.RightKey))
            {
                moveDirection = Vector2Int.right;
            }
        }
    }
    
    private void HandleMovement()
    {
        moveTimer -= Time.deltaTime;

        if (moveTimer <= 0f)
        {
            UpdateMovement();
            ScreenWrap();

            moveTimer = 1f / currSnakeSpeed;
        }
    }

    private void UpdateMovement()
    {
        Vector2Int prevPos = segments[0].GetPosition();
        
        // Update the head position
        playerPosition.x = segments[0].GetPosition().x + moveDirection.x;
        playerPosition.y = segments[0].GetPosition().y + moveDirection.y;
        segments[0].SetPosition(new Vector2Int(playerPosition.x, playerPosition.y));
        
        // Make the snake body move forward, each segment follows the one in front of it 
        for (int i = 1; i < segments.Count; i++)
        {
            Vector2Int currPos = segments[i].GetPosition();
            segments[i].SetPosition(prevPos);
            prevPos = currPos;
        }
    }

    private void ScreenWrap()
    {
        Bounds bounds = GameManager.Instance.GetWrappedAreaBounds();
        
        if(segments[0].GetPosition().x >= bounds.max.x)
        {
            segments[0].SetPosition(new Vector2Int((int)bounds.min.x, segments[0].GetPosition().y));  
        }
        else if (segments[0].GetPosition().x <= bounds.min.x)
        {
            segments[0].SetPosition(new Vector2Int((int)bounds.max.x, segments[0].GetPosition().y));     
        }
        
        if (segments[0].GetPosition().y >= bounds.max.y)
        {
            segments[0].SetPosition(new Vector2Int(segments[0].GetPosition().x, (int)bounds.min.y));          
        }
        else if (segments[0].GetPosition().y <= bounds.min.y)
        {
            segments[0].SetPosition(new Vector2Int(segments[0].GetPosition().x, (int)bounds.max.y));          
        }
    }

    #endregion

    #region Collision Detection

    private void CheckCollisions()
    {
        CheckForSelfCollision();

        if (otherPlayers != null)
        {
            CheckForOtherPlayerCollision();
        }
        
        CheckForItemCollision();
    }

    private void CheckForSelfCollision()
    {
        for (int i = 1; i < segments.Count; i++)
        {
            if (segments[0].GetPosition() == segments[i].GetPosition())
            {
                if (!isShieldActive)
                {
                    playerData.MarkAsDead();
                }
                
                return;
            }
        }
    }
    
    private void CheckForOtherPlayerCollision()
    {
        for (int i = 0; i < otherPlayers.Count; i++)
        {
            if (playerData.PlayerID < otherPlayers[i].playerData.PlayerID)
            {
                for (int j = 0; j < otherPlayers[i].segments.Count; j++)
                {
                    if (segments[0].GetPosition() == otherPlayers[i].segments[j].GetPosition())
                    {
                        if (j == 0)     // Head to Head collisions
                        {
                            // For head to head collisions, to avoid the duplicate reporting, only check for one of them based on which one has a lower ID
                            if (playerData.PlayerID < otherPlayers[i].playerData.PlayerID)
                            {
                                // If two players' head collides, then both of them dies
                                GameManager.Instance.OnPlayerDeath(otherPlayers[i].playerData);
                                GameManager.Instance.OnPlayerDeath(this.playerData);
                                
                                GameManager.Instance.CheckForGameOverCondition();
                            }
                        }
                        else            // Head to Body collisions
                        {
                            GameManager.Instance.OnPlayerDeath(otherPlayers[i].playerData);
                            
                            GameManager.Instance.CheckForGameOverCondition();
                        }
                        
                        return;
                    }
                }
            }
        }
    }
    
    private void CheckForItemCollision()
    {
        List<Food> foodItems = foodController.GetItems();
        for (int i = 0; i < foodItems.Count; i++)
        {
            if (foodItems[i].GetPosition() == segments[0].GetPosition())
            {
                OnFoodCollect(foodItems[i]);
            }
        }
        
        List<PowerUp> powerUpItems = powerUpController.GetItems();
        for (int i = 0; i < powerUpItems.Count; i++)
        {
            if (powerUpItems[i].GetPosition() == segments[0].GetPosition())
            {
                OnPowerUpCollect(powerUpItems[i]);
            }
        }
    }
    
    #endregion

    #region Food & Powerups

    private void OnPowerUpCollect(PowerUp powerUp)
    {
        switch (powerUp.Type)
        {
            case PowerUp.PowerUpType.Shield:
                ActivateShield(powerUp.EffectDuration);
                break;
            case PowerUp.PowerUpType.ScoreBoost:
                BoostScore(powerUp.ScoreBoostAmount);
                break;
            case PowerUp.PowerUpType.SpeedUp:
                SpeedUp(powerUp.SpeedMultiplier, powerUp.EffectDuration);
                break;
        }
        
        powerUp.gameObject.SetActive(false);
    }

    private void OnFoodCollect(Food food)
    {
        switch (food.Type)
        {
            case Food.FoodType.MassGainer:
                IncreaseLength(food.LengthChangeAmount);
                break;
            case Food.FoodType.MassBurner:
                DecreaseLength(food.LengthChangeAmount);
                break;
        }
        
        food.gameObject.SetActive(false);
    }
    
    public void IncreaseLength(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            SnakeSegment segment = Instantiate(BodySegment.transform, this.transform, true).GetComponent<SnakeSegment>();
            segment.SetPosition(segments[segments.Count - 1].GetPosition());
            segment.SetColor(playerData.Color.HeadColor);
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