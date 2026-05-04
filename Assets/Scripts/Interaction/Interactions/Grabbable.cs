using System;
using UnityEngine;

public class Grabbable : MonoBehaviour, IInteractable
{
    public string ActionTooltip()
    {
        return "[Left Click] Grab";
    }

    public void Interact()
    {
        Debug.Log("INTERACT");
    }
    
    public void InteractRelease()
    {
        Debug.Log("DROP");
    }
}