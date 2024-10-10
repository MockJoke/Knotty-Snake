using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class InputHandler : MonoBehaviour
{  
    public SnakeHandler[] player;

    private LifeStatus lifeStatus;
    private PlayerID playerID;

    void Start()
    {
        player = FindObjectsOfType<SnakeHandler>();
        // Debug.Log(player.Length);
    }
    
    void Update()
    {
        switch (player[0].lifeStatus)
        {
            case LifeStatus.Alive:
                PlayerInput(); 
                break;
        }
    }

    void FixedUpdate()
    {
        switch(playerID)
        {
            case PlayerID.player1:
                player[0].Movement(); 
                break;
        }
    }
    
    public void PlayerInput()
    {
        //if(playerID == PlayerID.player1)
        //{
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
            {
                player[0].InputDirection();
            }
            // else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
            // {
            //     Debug.Log(player.Length);
            //     player[1].InputDirection();
            // }
        //}
    }
}
