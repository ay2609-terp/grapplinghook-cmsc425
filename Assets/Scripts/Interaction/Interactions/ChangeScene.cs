using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour, IInteractable
{
    public string sceneToLoad;

    public string ActionTooltip()
    {
        return "[LMB] Next Level";
    }

    public void Interact(GameObject player)
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void InteractRelease(GameObject player)
    {

    }
}