using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public SceneAsset[] levels;
    public SceneAsset pauseMenu;
    
    private Scene currentLevel;
    private Monster[] monsters;

    private void Awake()
    {
        if (pauseMenu != null && SceneManager.GetSceneByName(pauseMenu.name).isLoaded == false)
        {
            SceneManager.LoadScene(pauseMenu.name, LoadSceneMode.Additive);
        }
        var instances = GameObject.FindObjectsOfType<LevelController>();
        if (instances.Length > 1)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);

        
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnLevelStart();
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        OnLevelStart();
    }

    public void OnLevelStart()
    {
        currentLevel = SceneManager.GetActiveScene();
        monsters = FindObjectsOfType<Monster>();
    }

    // Update is called once per frame
    void Update()
    {
        if(AllMonstersAreDead())
        {
            GoToNextLevel();
        }
    }

    private bool AllMonstersAreDead()
    {
        return monsters.Length > 0 && monsters.All(monster => monster.gameObject.activeSelf == false);
    }

    private void GoToNextLevel()
    {
        var sceneIndex = levels.ToList().FindIndex(l => l.name == currentLevel.name);

        if(sceneIndex != levels.Length -1)
        {
            var nextScene = levels[sceneIndex + 1];
            SceneManager.LoadScene(nextScene.name);
        }
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(currentLevel.name);
    }

    public void LoadStartMenu()
    {
        SceneManager.LoadScene("Start-Menu");
    }

    public void LoadLevelSelection()
    {
        // TODO: Create a level selection scene and load it
    }
}
