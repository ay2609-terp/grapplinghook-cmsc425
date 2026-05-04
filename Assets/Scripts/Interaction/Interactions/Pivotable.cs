using System;
using UnityEngine;

public class Pivotable : MonoBehaviour, IInteractable
{
    public string ActionTooltip()
    {
        return "[Left Click] Pivot";
    }

    public void Interact()
    {
        
    }
    
    public void InteractRelease()
    {
        
    }
}