using UnityEngine;

public interface IInteractable
{
    string ActionTooltip();
    void Interact(GameObject player);
    void InteractRelease(GameObject player);
}