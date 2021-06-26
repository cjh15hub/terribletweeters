using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PausePanel, PauseButton, ResumeButton, RestartButton, SettingsButton, ExitButton;

    public void Start()
    {
        
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }
    public void Pause()
    {
        PausePanel.SetActive(true);
        PauseButton.SetActive(false);
    }

    public void Resume()
    {
        PausePanel.SetActive(false);
        PauseButton.SetActive(true);
    }

    public void Restart()
    {
        FindObjectOfType<LevelController>().ReloadLevel();
    }

    public void Exit()
    {
        FindObjectOfType<LevelController>().LoadStartMenu();
    }

    public void Levels()
    {
        FindObjectOfType<LevelController>().LoadLevelSelection();
    }
}
