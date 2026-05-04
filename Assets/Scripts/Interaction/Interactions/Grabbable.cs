using UnityEngine;

public class Grabbable : MonoBehaviour, IInteractable
{
    public float HoldDistance = 2;
    public float GrabForce = 50;
    public float GrabTorque = 20;
    public float ForceDamping = 10;
    public float TorqueDamping = 20;

    private Rigidbody rigidBody;
    private bool grabbing = false;
    private GameObject player;
    private float rotationOffset;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    public string ActionTooltip()
    {
        return "[LMB] Grab";
    }

    public void Interact(GameObject player)
    {
        grabbing = true;
        this.player = player;

        rotationOffset = Mathf.DeltaAngle(player.transform.eulerAngles.y, -90);

        rigidBody.useGravity = false;
        rigidBody.excludeLayers = LayerMask.GetMask("Player");
    }

    public void InteractRelease(GameObject player)
    {
        grabbing = false;
        
        rigidBody.useGravity = true;
        rigidBody.excludeLayers = LayerMask.GetMask("Nothing");
    }

    void FixedUpdate()
    {
        if (!grabbing) return;

        Transform camera = player.GetComponentInChildren<Camera>().transform;
        Vector3 grabPosition = camera.position + camera.forward * HoldDistance;

        // position 
        Vector3 positionDelta = grabPosition - transform.position;
        rigidBody.AddForce(positionDelta * GrabForce - rigidBody.linearVelocity * ForceDamping, ForceMode.Acceleration);

        // rotation
        Quaternion targetRotation = Quaternion.Euler(0f, player.transform.rotation.eulerAngles.y + rotationOffset, 0f);
        Quaternion rotationDelta = targetRotation * Quaternion.Inverse(rigidBody.rotation);
        rotationDelta.ToAngleAxis(out float angle, out Vector3 axis);
        if (angle > 180f) angle -= 360f;
        rigidBody.AddTorque(axis * angle * GrabTorque - rigidBody.angularVelocity * TorqueDamping, ForceMode.Acceleration);
    }
}