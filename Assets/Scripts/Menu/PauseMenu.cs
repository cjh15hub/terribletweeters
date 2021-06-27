using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PauseButton, PausePanel, PauseMessage, SettingsButton, LevelsButton, RestartButton, ResumeButton;
    public GameObject CompleteMessage, LevelName, Score, ContinueButton; 

    private LevelController LevelController;

    public void Awake()
    {
        // Turn Panel off, Pause Button On
        PausePanel.SetActive(false);
        PauseButton.SetActive(true);
    }

    public void Start()
    {
        LevelController = FindObjectOfType<LevelController>();
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
        // Turn Panel on, Pause Button Off
        PausePanel.SetActive(true);
        PauseButton.SetActive(false);

        // Make Sure Pause UI is on
        PauseMessage.SetActive(true);
        ResumeButton.SetActive(true);

        // Make Sure Complete UI is off
        CompleteMessage.SetActive(false);
        LevelName.SetActive(false);
        Score.SetActive(false);
        ContinueButton.SetActive(false);
    }

    public void Resume()
    {
        // Turn Panel off, Pause Button On
        PausePanel.SetActive(false);
        PauseButton.SetActive(true);
    }

    public void LevelStats(string levelName, bool complete, int score)
    {
        // Turn Panel on, Pause Button Off
        PausePanel.SetActive(true);
        PauseButton.SetActive(false);

        // Make Sure Complete UI is on
        CompleteMessage.SetActive(true);
        LevelName.SetActive(true);
        Score.SetActive(true);
        ContinueButton.SetActive(true);

        // Make Sure Pause UI is off
        PauseMessage.SetActive(false);
        ResumeButton.SetActive(false);

        // Set Labels
        LevelName.GetComponent<TextMeshProUGUI>().SetText(levelName);
        Score.GetComponent<TextMeshProUGUI>().SetText(score.ToString());
    }

    public void Continue()
    {
        LevelController.GoToNextLevel();

        // Turn Panel off, Pause Button On
        PausePanel.SetActive(false);
        PauseButton.SetActive(true);
    }

    public void Restart()
    {
        LevelController.RestartLevel();
        Resume();
    }

    public void Exit()
    {
        LevelController.LoadStartMenu();
    }

    public void Levels()
    {
        LevelController.LoadLevelSelection();
    }
}
