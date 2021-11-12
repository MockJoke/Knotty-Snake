using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class InputHandler : MonoBehaviour
{  
    public SnakeHandler[] players;

    private LifeStatus lifeStatus;
    private PlayerID playerID; 

    //private void Start()
    //{
    //    SnakeHandler[] players = FindObjectsOfType<SnakeHandler>();
    //    Debug.Log(players); 
    //}
    private void Update()
    {
        switch (players[0].lifeStatus)
        {
            case LifeStatus.Alive:
                PlayerInput(); 
                break;
        }
    }

    private void FixedUpdate()
    {
        switch(playerID)
        {
            case PlayerID.player1:
                players[0].Move(); 
                break;
        }
    }
    public void PlayerInput()
    {
        if(playerID == PlayerID.player1)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
            {
                players[0].InputDirection();
            }
        }
    }
}
