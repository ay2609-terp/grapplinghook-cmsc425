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
        if (SceneFader.Instance != null)
        {
            SceneFader.Instance.LoadSceneWithFade(sceneToLoad);
        }
        else
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    public void InteractRelease(GameObject player)
    {

    }
}