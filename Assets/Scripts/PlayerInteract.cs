using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    public float interactDistance = 5f;

    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * interactDistance, Color.red);

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Ray ray = new Ray(transform.position, transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
            {
                Debug.Log("Hit: " + hit.collider.name);

                LeverInteractable lever = hit.collider.GetComponentInParent<LeverInteractable>();

                if (lever != null)
                {
                    lever.Interact();
                }
            }
            else
            {
                Debug.Log("Ray hit nothing");
            }
        }
    }
}