using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ShadowEffects : MonoBehaviour
{
    private Death death;

    public GameObject CurrentVolume;

    public float minSaturationAtFullDeath = -20f;

    public float minPostExposureAtFullDeath = -3f;

    public float responseSpeed = 10f;

    void Awake()
    {
        death = GetComponentInParent<Death>();
    }

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

        float progress = death != null ? Mathf.Clamp01(death.deathProgression) : 0f;

        float targetSaturation = Mathf.Lerp(0f, minSaturationAtFullDeath, progress);
        float targetPostExposure = Mathf.Lerp(0f, minPostExposureAtFullDeath, progress);

        float t = Mathf.Clamp01(Time.deltaTime * responseSpeed);
        volumeColor.saturation.value = Mathf.Lerp(volumeColor.saturation.value, targetSaturation, t);
        volumeColor.postExposure.value = Mathf.Lerp(volumeColor.postExposure.value, targetPostExposure, t);
    }
}
