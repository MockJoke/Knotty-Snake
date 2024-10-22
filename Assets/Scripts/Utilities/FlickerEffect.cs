using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerEffect : MonoBehaviour
{
    [SerializeField] private Renderer objectRenderer; // Assign the GameObject's Renderer
    [SerializeField] private float flickerInterval = 0.1f; // Time between flickers

    private Coroutine flickerCoroutine;

    void Awake()
    {
        if (!objectRenderer)
            objectRenderer = GetComponent<Renderer>();
    }

    public void Play()
    {
        flickerCoroutine =  StartCoroutine(FlickerRoutine());
    }

    public void Stop()
    {
        StopCoroutine(flickerCoroutine);
    }

    private IEnumerator FlickerRoutine()
    {
        while (true)
        {
            objectRenderer.enabled = !objectRenderer.enabled;

            yield return new WaitForSeconds(flickerInterval);
        }
    }
}
