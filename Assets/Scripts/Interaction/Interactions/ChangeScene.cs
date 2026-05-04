using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour, IInteractable
{
    public string sceneToLoad;

    public string ActionTooltip()
    {
        return "[Left Click] Next Level";
    }

    public void Interact()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void InteractRelease()
    {

    }
}