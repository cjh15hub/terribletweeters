using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    public SceneAsset[] levels;
    
    private Scene currentLevel;
    private Monster[] monsters;

    private void Awake()
    {
        currentLevel = SceneManager.GetActiveScene();
        //var instances = GameObject.FindObjectsOfType<LevelController>();
        //if (instances.Length > 1) Destroy(this.gameObject);
        //DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        //currentLevel = SceneManager.GetActiveScene();
    }

    private void OnEnable()
    {
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
}
