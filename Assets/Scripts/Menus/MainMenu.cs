using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject levelSelectPanel;
    public GameObject instructionsPanel;

    public void Play()
    {
        int hasPlayedBefore = PlayerPrefs.GetInt("HasPlayedBefore", 0);

        if (hasPlayedBefore == 0)
        {
            PlayerPrefs.SetInt("HasPlayedBefore", 1);
            PlayerPrefs.Save();

            SceneManager.LoadScene("LevelOne");
        }
        else
        {
            OpenLevelSelect();
        }
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}