using UnityEngine;
using System;
using System.Collections;
using Unity.Mathematics;


public class Death : MonoBehaviour
{
    public Vector3 origin;
    public float rotation; // 0-360 degrees (clockwise)
    public float gracePeriod;

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

    // SWAP THIS FUNCTION NAME WITH LIGHT IF WE WANT THE LIGHT TO KILL AND SHADOW TO BE NEUTRAL
    private void HandleEnterShadow()
    {
        Debug.Log("DeathReset: Player entered SHADOW");

        if (resetCoroutine == null)
        {
            Debug.Log($"DeathReset: Starting reset timer ( for {gracePeriod} seconds)");            
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
            Debug.Log("DeathReset: Cancelling reset timer");            
            StopCoroutine(resetCoroutine);
            resetCoroutine = null;
        }
        else
        {
            Debug.Log("DeathReset: No timer active, nothing to cancel, window passed?");
        }
    }

    // coroutine which starts running when player enters light, and stops running when it leaves the light or grace period runs out
    private IEnumerator ResetAfterGracePeriod()
    {
        Debug.Log("DeathReset: Coroutine started");        
        
        yield return new WaitForSeconds(gracePeriod);

        Debug.Log($"DeathReset: Grace period expired, now resetting player position to {origin}, rotation to {rotation}");

        controller.enabled = false;
        transform.position = origin;
        transform.rotation = Quaternion.Euler(0, rotation, 0);
        controller.enabled = true;



        resetCoroutine = null;
    }

    // void Update()
    // {
    //     // methodology/idea for the death mechanic
    //     // use a coroutine which will initialize a timer whenever the character enters light
    //     // the caller of the coroutine will cancel it if the player enters darkness again,
    //     // if the coroutine timer ends, then it will initiate the reset for the character

    // }
}
