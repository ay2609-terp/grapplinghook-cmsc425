using UnityEngine;
using System;
using System.Collections;
using Unity.Mathematics;


public class Death : MonoBehaviour
{
    public Vector3 origin;
    public float rotation; // 0-360 degrees (clockwise)
    public float gracePeriod;
    public float deathProgression; 
    private DateTime routineStartTime;
    

    private Coroutine resetCoroutine;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        Debug.Log("DeathReset: Subscribing to events from ExposureDetector");

        ExposureDetector.onEnterLight += HandleEnterLight;
        ExposureDetector.onEnterShadow += HandleEnterShadow;
    }

    private void OnDisable()
    {
        Debug.Log("DeathReset: Unsubscribing from ExposureDetector events");

        ExposureDetector.onEnterLight -= HandleEnterLight;
        ExposureDetector.onEnterShadow -= HandleEnterShadow;

        if (resetCoroutine != null)
        {   
            StopCoroutine(resetCoroutine);
            resetCoroutine = null;
        }
    }

    private IEnumerator ResetAfterGracePeriod()
    {
        float elapsed = 0f;
        Debug.Log("DeathReset: Coroutine started");

        while (elapsed < gracePeriod)
        {
            elapsed += Time.deltaTime;
            
            // Map elapsed time to a 0.0 - 1.0 range for progress bars/effects
            deathProgression = Mathf.Clamp01(elapsed / gracePeriod);
            
            yield return null; // Wait until the next frame
        }

        // Ensure it hits exactly 1.0 at the end
        deathProgression = 1f;

        Debug.Log("DeathReset: Grace period expired. Resetting player.");

        controller.enabled = false;
        transform.position = origin;
        transform.rotation = Quaternion.Euler(0, rotation, 0);
        controller.enabled = true;

        // Reset progression and reference
        deathProgression = 0f;
        resetCoroutine = null;
    }


    // SWAP THIS FUNCTION NAME WITH LIGHT IF WE WANT THE LIGHT TO KILL AND SHADOW TO BE NEUTRAL
    private void HandleEnterShadow()
    {
        Debug.Log("DeathReset: Player entered SHADOW");

        if (resetCoroutine == null)
        {
            Debug.Log($"DeathReset: Starting reset timer ( for {gracePeriod} seconds)");            
            routineStartTime = DateTime.Now;
            resetCoroutine = StartCoroutine(ResetAfterGracePeriod());
        }
        else
        {
            Debug.Log("DeathReset: Timer already running, ignoring duplicate event");            
        }
    }

    private void HandleEnterLight()
    {
        Debug.Log("DeathReset: Player entered LIGHT");        
        if (resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
            resetCoroutine = null;
            
            // Reset progression immediately when escaping to light
            deathProgression = 0f; 
        }
    }

}
