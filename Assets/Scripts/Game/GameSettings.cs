using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode
{
    SinglePlayer,
    CoOp
}

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance { get; private set; }

    public GameMode SelectedGameMode { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            Debug.Log($"{Instance.name} {gameObject.name} An instance of this singleton already exists");
        }
        else
        {
            Instance = this;
            
            DontDestroyOnLoad(this);
        }
    }

    public void SetGameMode(GameMode mode)
    {
        SelectedGameMode = mode;
    }
}
