using UnityEngine;

public class LeverInteractable : MonoBehaviour
{
    public LeverSwitch lever;

    public void Interact()
    {
        if (lever != null)
        {
            lever.PullLever();
        }
    }
}