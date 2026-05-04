using UnityEngine;
using UnityEngine.InputSystem;

public class Pivotable : MonoBehaviour, IInteractable
{
    public InputActionReference LookAction;

    public float rotationSpeed = 75f;
    public float maxYaw = 60f;
    public float maxPitch = 30f;

    PlayerMovement movementScript;
    private bool pivoting = false;
    private float yaw;
    private float pitch;

    public string ActionTooltip()
    {
        return "[LMB] Pivot";
    }

    public void Interact(GameObject player)
    {
        pivoting = true;

        movementScript = player.GetComponent<PlayerMovement>();
        movementScript.lookEnabled = false;

        LookAction.action.Enable();
    }

    public void InteractRelease(GameObject player)
    {
        pivoting = false;

        movementScript.lookEnabled = true;

        LookAction.action.Disable();
    }

    void Update()
    {
        if (!pivoting) return;

        Vector2 look = LookAction.action.ReadValue<Vector2>();
        float mouseX = look.x;
        float mouseY = look.y;

        yaw += mouseX * rotationSpeed * Time.deltaTime;
        yaw = Mathf.Clamp(yaw, -maxYaw, maxYaw);

        pitch += mouseY * rotationSpeed * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, -maxPitch, maxPitch);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}
