using UnityEngine;
using UnityEngine.Audio;

public class ProximityHummingAdvanced : MonoBehaviour
{
    public Transform player;
    public AudioSource audioSource;

    // Distance vars
    public float maxHearingDistance = 20;
    public float minDistance = 2f;
    public float maxVolume = 0.6f;

    // Occlusion vars (if needed)
    public LayerMask occlusionMask;
    public float occludedVolumeMultiplier = 0.3f;

    // Smoothing var
    public float smoothing = 5f;

    public AudioMixerGroup mixerGroup;
    private float currentVolume;

    void Start()
    {
        if (audioSource == null) return;

        audioSource.loop = true;
        audioSource.volume = 0f;
        audioSource.pitch = 1f;

        if (mixerGroup != null)
            audioSource.outputAudioMixerGroup = mixerGroup;

        if (!audioSource.isPlaying)
            audioSource.Play();

        currentVolume = 0f;
    }

    void Update()
    {
        if (player == null || audioSource == null) return;

        float distance = Vector3.Distance(player.position, transform.position);

        // 0 – 1 based on distance
        float t = Mathf.InverseLerp(maxHearingDistance, minDistance, distance);
        t = Mathf.Clamp01(t);

        // Occlusion check
        float occlusionMultiplier = 1f;

        Vector3 dir = (player.position - transform.position).normalized;
        float dist = Vector3.Distance(player.position, transform.position);

        if (Physics.Raycast(transform.position, dir, dist, occlusionMask))
        {
            occlusionMultiplier = occludedVolumeMultiplier;
        }

        float targetVolume = t * maxVolume * occlusionMultiplier;

        // Smooth fade
        currentVolume = Mathf.Lerp(currentVolume, targetVolume, Time.deltaTime * smoothing);

        audioSource.volume = currentVolume;

        // Force constant pitch
        audioSource.pitch = 1f;
    }
}