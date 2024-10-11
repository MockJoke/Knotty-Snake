using UnityEngine;

/// <summary>
/// A class to use Unity's Coroutines for Non-MonoBehaviour classes
/// </summary>
public class CoroutineRunner : MonoBehaviour
{
    private static CoroutineRunner instance;
    public static CoroutineRunner Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject runner = new GameObject("CoroutineRunner");
                instance = runner.AddComponent<CoroutineRunner>();
                DontDestroyOnLoad(runner);
            }
            
            return instance;
        }
    }
}