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

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void SetGameMode(GameMode mode)
    {
        SelectedGameMode = mode;
    }
}
