using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    public InputActionReference InteractAction;

    public Transform CameraTransform;
    public TextMeshProUGUI TooltipUI;

    public float Range;
    public LayerMask OccluderLayers;

    private void OnEnable()
    {
        InteractAction.action.Enable();
    }

    private void OnDisable()
    {
        InteractAction.action.Disable();
    }

    void Update()
    {
        IInteractable currentInteractable = null;
        Vector3 cameraPosition = CameraTransform.position;


        if(Physics.Raycast(cameraPosition, CameraTransform.forward, out RaycastHit hit, Range, OccluderLayers))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                currentInteractable = interactable;
            }
        }

        TooltipUI.enabled = false;
        if (currentInteractable != null)
        {
            TooltipUI.text = currentInteractable.ActionTooltip();
            TooltipUI.enabled = true;

            if (InteractAction.action.WasPressedThisFrame())
            {
                currentInteractable.Interact();
            }

            if (InteractAction.action.WasReleasedThisFrame())
            {
                currentInteractable.InteractRelease();
            }
        }
    }
}
