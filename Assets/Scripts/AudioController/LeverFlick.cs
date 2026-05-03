using UnityEngine;

public class LeverSwitchSound : MonoBehaviour
{
    public AudioSource audioSource;

    // Interaction vars
    public float interactDistance = 3f;
    public float cooldown = 0.3f;

    private Transform player;
    private float lastInteractTime;

    void Start()
    {
        // Find player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        // Safety check
        if (audioSource != null)
        {
            audioSource.playOnAwake = false;
        }
    }

    void Update()
    {
        if (player == null || audioSource == null) return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= interactDistance && Input.GetKeyDown(KeyCode.E))
        {
            if (Time.time - lastInteractTime > cooldown)
            {
                lastInteractTime = Time.time;
                PlaySwitchSound();
            }
        }
    }

    void PlaySwitchSound()
    {
        audioSource.PlayOneShot(audioSource.clip);
    }
}