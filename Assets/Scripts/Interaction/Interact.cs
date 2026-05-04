using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    public InputActionReference InteractAction;

    public Transform CameraTransform;
    public TextMeshProUGUI TooltipUI;

    public float Range;
    public LayerMask OccluderLayers;

    private GameObject player;
    private IInteractable currentInteractable;
    private bool interacting = false;

    private void OnEnable()
    {
        InteractAction.action.Enable();
    }

    private void OnDisable()
    {
        InteractAction.action.Disable();
    }

    void Awake()
    {
        player = transform.gameObject;
    }

    void Update()
    {
        Vector3 cameraPosition = CameraTransform.position;

        if (interacting)
        {
            // always display tooltip
            TooltipUI.text = currentInteractable.ActionTooltip();
            TooltipUI.enabled = true;

            if (InteractAction.action.WasReleasedThisFrame())
            {
                currentInteractable.InteractRelease(player);
                interacting = false;
            }
        }
        else
        {
            // check for interactable
            currentInteractable = null; 
            if(Physics.Raycast(cameraPosition, CameraTransform.forward, out RaycastHit hit, Range, OccluderLayers))
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    currentInteractable = interactable;
                }
            }

            // tooltip and interact check
            TooltipUI.enabled = false;
            if (currentInteractable != null)
            {
                TooltipUI.text = currentInteractable.ActionTooltip();
                TooltipUI.enabled = true;

                if (InteractAction.action.WasPressedThisFrame())
                {
                    currentInteractable.Interact(player);
                    interacting = true;
                }
            }

        }
    }
}
