using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public SceneAsset[] Levels;
    public SceneAsset TheEnd;
    public SceneAsset PauseMenu;
    
    private Monster[] monsters;

    public int score
    {
        get; private set;
    }

    private bool levelComplete = false;

    private void Awake()
    {
        var instances = GameObject.FindObjectsOfType<LevelController>();
        if (instances.Length > 1)
        {
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);

        if (PauseMenu != null && SceneManager.GetSceneByName(PauseMenu.name).isLoaded == false)
        {
            SceneManager.LoadScene(PauseMenu.name, LoadSceneMode.Additive);
        }
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
        levelComplete = false;
        monsters = FindObjectsOfType<Monster>();
    }

    private int getCurrentLevelIndex()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            var x = Levels.ToList().FindIndex(l => l.name == SceneManager.GetSceneAt(i).name);
            if (x != -1) return x;
        }

        return -1;
    }

    void Update()
    {
        if(!levelComplete && AllMonstersAreDead())
        {
            var currentLevelIndex = getCurrentLevelIndex();

            levelComplete = true;
            var tempScore = Random.Range(100, 1000);
            var currentLevelName = Levels[currentLevelIndex].name;
            FindObjectOfType<PauseMenu>().LevelStats(currentLevelName, true, tempScore);
        }
    }

    private bool AllMonstersAreDead()
    {
        return monsters.Length > 0 && monsters.All(monster => monster.gameObject.activeSelf == false);
    }

    public void GoToNextLevel()
    {
        var currentLevelIndex = getCurrentLevelIndex();

        if (currentLevelIndex != Levels.Length -1)
        {
            var currentLevelName = Levels[currentLevelIndex].name;
            var nextScene = Levels[currentLevelIndex + 1];
            currentLevelIndex++;
            StartCoroutine(SwapLevels(currentLevelName, nextScene.name));
        }
        else
        {
            var currentLevelName = Levels[currentLevelIndex].name;
            currentLevelIndex = -1;
            StartCoroutine(SwapLevels(currentLevelName, TheEnd.name));
        }
    }

    public void RestartLevel()
    {
        var currentLevelIndex = getCurrentLevelIndex();

        monsters = new Monster[0];
        var currentLevelName = Levels[currentLevelIndex].name;
        StartCoroutine(SwapLevels(currentLevelName, currentLevelName));
    }

    private IEnumerator SwapLevels(string unloadSceneName, string loadSceneName)
    {
        AsyncOperation unload = SceneManager.UnloadSceneAsync(unloadSceneName);
        yield return unload;

        SceneManager.LoadScene(loadSceneName, LoadSceneMode.Additive);
    }

    
    public void LoadStartMenu()
    {
        SceneManager.LoadScene("Start-Menu");
    }

    public void LoadLevelSelection()
    {
        // TODO: Create a level selection scene and load it
        // And Script within that scene will need a method like the following
        /*
            var levelIndex = 0; 
            SceneManager.LoadScene(LevelControllerPrefab.GetComponent<LevelController>().Levels[levelIndex].name);

            // not necessarily needed if all levels have a LevelController in them
            _LevelController = Instantiate(LevelControllerPrefab);
            LevelController = _LevelController.GetComponent<LevelController>();
        */
    }

}
