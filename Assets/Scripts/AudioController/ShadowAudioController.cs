using UnityEngine;

public class ShadowAudioController : MonoBehaviour
{
    public AudioSource shadowAudioSource;

    // Volume vars
    public float minVolume = 0.001f;
    public float maxVolume = 1f;
    private float targetVolume;

    // Fade vars
    public float fadeInTime = 0.75f;
    public float fadeOutTime = 5f;

    // Shadow var
    private bool inShadow = false;

    void Start()
    {
        if (shadowAudioSource == null) return;

        shadowAudioSource.volume = minVolume;
        targetVolume = minVolume;

        shadowAudioSource.loop = true;
        if (!shadowAudioSource.isPlaying)
            shadowAudioSource.Play();
    }

    void OnEnable()
    {
        ExposureDetector.onEnterShadow += EnterShadow;
        ExposureDetector.onEnterLight += EnterLight;
    }

    void OnDisable()
    {
        ExposureDetector.onEnterShadow -= EnterShadow;
        ExposureDetector.onEnterLight -= EnterLight;
    }

    void Update()
    {
        if (shadowAudioSource == null) return;

        float fadeTime = inShadow ? fadeInTime : fadeOutTime;

        // Base linear speed
        float baseSpeed = (maxVolume - minVolume) / fadeTime;

        // Normalize current volume
        float t = Mathf.InverseLerp(minVolume, maxVolume, shadowAudioSource.volume);

        // Slowly increase volume at start
        float curved = Mathf.Lerp(0.2f, 1f, t * t);
        shadowAudioSource.volume = Mathf.MoveTowards(
            shadowAudioSource.volume,
            targetVolume,
            baseSpeed * curved * Time.deltaTime
        );
    }

    void EnterShadow()
    {
        if (inShadow) return;
        inShadow = true;
        targetVolume = maxVolume;
    }

    void EnterLight()
    {
        if (!inShadow) return;
        inShadow = false;
        targetVolume = minVolume;
    }
}