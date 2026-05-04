using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ShadowEffects : MonoBehaviour
{
    public GameObject CurrentVolume;
    public float DesaturationFactor;
    public float ExposureDecreaseFactor;

    private bool inShadow;

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
        Volume volume = CurrentVolume.transform.GetComponent<Volume>();
        if (volume == null)
        {
            Debug.Log("CurrentVolume GameObject has no Volume component");
            return;
        }

        if (volume.profile == null)
        {
            Debug.Log("Volume has no profile assigned");
            return;
        }

        ColorAdjustments volumeColor;
        if (!volume.profile.TryGet(out volumeColor))
        {
            Debug.Log("Volume profile has no ColorAdjustments override");
            return;
        }

        if (inShadow)
        {

            volumeColor.saturation.value -= DesaturationFactor * Time.deltaTime;
            volumeColor.postExposure.value -= ExposureDecreaseFactor * Time.deltaTime;
        }
        else
        {
            float saturationChange = DesaturationFactor * Time.deltaTime * 2;
            float exposureChange = ExposureDecreaseFactor * Time.deltaTime * 2; 

            if (volumeColor.saturation.value + saturationChange > 0)
            {
                volumeColor.saturation.value = 0;
            }
            else
            {
                volumeColor.saturation.value += saturationChange;
            }

            if (volumeColor.postExposure.value + exposureChange > 0)
            {
                volumeColor.postExposure.value = 0;
            }
            else
            {
                volumeColor.postExposure.value += exposureChange;
            }
        }
    }
}
