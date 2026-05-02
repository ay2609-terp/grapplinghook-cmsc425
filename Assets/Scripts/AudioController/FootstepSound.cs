using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FootstepController : MonoBehaviour
{
    [Header("References")]
    public AudioSource audioSource;
    public AudioClip[] footstepSounds;

    [Header("Settings")]
    public float walkStepInterval = 0.5f;
    public float sprintStepInterval = 0.35f;
    public float minimumMoveSpeed = 0.1f;
    public float sprintSpeedThreshold = 6f;

    [Header("Anti-Spam")]
    public float minimumTimeBetweenSteps = 0.2f;

    private CharacterController controller;
    private float stepTimer;
    private float lastStepTime;
    private bool wasMovingLastFrame;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector3 horizontalVelocity = controller.velocity;
        horizontalVelocity.y = 0f;

        float speed = horizontalVelocity.magnitude;
        bool isMoving = controller.isGrounded && speed > minimumMoveSpeed;

        if (isMoving)
        {
            float currentInterval = speed > sprintSpeedThreshold
                ? sprintStepInterval
                : walkStepInterval;

            // Play immediately when starting movement
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
        else
        {
            stepTimer = 0f;
        }

        wasMovingLastFrame = isMoving;
    }

    void TryPlayFootstep()
    {
        if (Time.time - lastStepTime < minimumTimeBetweenSteps)
            return;

        PlayFootstep();
        lastStepTime = Time.time;
    }

    void PlayFootstep()
    {
        if (footstepSounds == null || footstepSounds.Length == 0)
            return;

        AudioClip clip = footstepSounds[
            Random.Range(0, footstepSounds.Length)
        ];

        audioSource.PlayOneShot(clip);
    }
}