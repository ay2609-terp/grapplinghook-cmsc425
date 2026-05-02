using UnityEngine;

public class FootstepController : MonoBehaviour
{
    // Audio vars
    public AudioSource audioSource;
    public AudioClip[] footstepSounds;

    // Movement interval vars
    public float walkStepInterval = 0.5f;
    public float sprintStepInterval = 0.35f;
    public float minimumMoveSpeed = 0.1f;
    public float sprintSpeedThreshold = 6f;

    // Step timing vars
    public float minimumTimeBetweenSteps = 0.2f;
    private CharacterController controller;
    private float stepTimer;
    private float lastStepTime;
    private bool wasMovingLastFrame;

    // Repeat limiting vars
    private int lastClipIndex = -1;
    private int repeatCount = 0;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Get velocity
        Vector3 horizontalVelocity = controller.velocity;
        horizontalVelocity.y = 0f;
        float speed = horizontalVelocity.magnitude;
        bool isMoving = controller.isGrounded && speed > minimumMoveSpeed;

        // Detect movement and limit sounds
        if (isMoving)
        {
            float currentInterval = speed > sprintSpeedThreshold ? sprintStepInterval : walkStepInterval;
            if (!wasMovingLastFrame)
            {
                TryPlayFootstep();
                stepTimer = 0f;
            }

            stepTimer += Time.deltaTime;
            if (stepTimer >= currentInterval)
            {
                TryPlayFootstep();
                stepTimer = 0f;
            }
        }

        // Reset step timer
        else
        {
            stepTimer = 0f;
        }

        wasMovingLastFrame = isMoving;
    }

    // Play footsteps when possible
    void TryPlayFootstep()
    {
        if (Time.time - lastStepTime < minimumTimeBetweenSteps)
            return;

        PlayFootstep();
        lastStepTime = Time.time;
    }

    // Play footstep sounds
    void PlayFootstep()
    {
        if (footstepSounds == null || footstepSounds.Length == 0) {
            return;
        }

        // prevent more than 2 repeats
        int clipIndex;
        clipIndex = Random.Range(0, footstepSounds.Length);
        if (clipIndex == lastClipIndex)
        {
            repeatCount++;
            if (repeatCount >= 2)
            {
                // force a different audio clip
                do
                {
                    clipIndex = Random.Range(0, footstepSounds.Length);
                }
                while (clipIndex == lastClipIndex);
                repeatCount = 0;
            }
        }
        else
        {
            repeatCount = 0;
        }

        lastClipIndex = clipIndex;
        audioSource.PlayOneShot(footstepSounds[clipIndex]);
    }
}