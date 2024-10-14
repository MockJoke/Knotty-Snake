using System;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class QuitOption : MonoBehaviour
{
    [SerializeField] private Button QuitBtn;

    private void Awake()
    {
        if (!QuitBtn)
            QuitBtn = GetComponent<Button>();
        
        if (QuitBtn)
            QuitBtn.onClick.AddListener(OnQuit);
    }

    private void OnQuit()
    {
#if UNITY_EDITOR 
        EditorApplication.isPlaying = false;
#else 
		Application.Quit();
#endif
    }
}
