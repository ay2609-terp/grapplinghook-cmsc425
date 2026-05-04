using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject levelSelectPanel;
    public GameObject instructionsPanel;

    void Start()
    {
        BackToMainMenu();
    }

    public void Play()
    {
        OpenLevelSelect();
    }

    public void OpenLevelSelect()
    {
        mainMenuPanel.SetActive(false);
        levelSelectPanel.SetActive(true);
        instructionsPanel.SetActive(false);
    }

    public void OpenInstructions()
    {
        mainMenuPanel.SetActive(false);
        levelSelectPanel.SetActive(false);
        instructionsPanel.SetActive(true);
    }

    public void BackToMainMenu()
    {
        mainMenuPanel.SetActive(true);
        levelSelectPanel.SetActive(false);
        instructionsPanel.SetActive(false);
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void Quit()
    {
        Application.Quit();
    }
}