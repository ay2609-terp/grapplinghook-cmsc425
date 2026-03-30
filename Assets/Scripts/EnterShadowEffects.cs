using UnityEngine;

public class ShadowEffects : MonoBehaviour
{
    private bool inShadow;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
    }

    private void HandleEnterLight()
    {
        inShadow = false;
    }

    private void HandleEnterShadow()
    {
        inShadow = true;
    }

    // Should use coroutine instead 
    void Update()
    {
        if (inShadow)
        {
            
        }
        else
        {
            
        }
    }
}
