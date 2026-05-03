using UnityEngine;
using UnityEngine.InputSystem;

public class LeverInteractable : MonoBehaviour
{
    public LeverSwitch lever;
    private bool playerNear = false;

    // Update is called once per frame
    void Update()
    {
        // press e AND player is near, lever interacted with
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            lever.PullLever();
        }
    }
}
