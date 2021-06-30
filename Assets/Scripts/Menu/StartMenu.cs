using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject LevelControllerPrefab;

    [SerializeField]
    private SceneAsset StartScene;

    public GameObject _LevelController
    {
        get; private set;
    }

    public LevelController LevelController
    {
        get; private set;
    }


    public void Start()
    {
        if(!LevelControllerPrefab)
        {
            throw new MissingReferenceException("Level Controller Prefab must be set.");
        }

        if(StartScene != null && SceneManager.GetSceneByName(StartScene.name).isLoaded == false)
        {
            SceneManager.LoadScene(StartScene.name, LoadSceneMode.Additive);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(LevelControllerPrefab.GetComponent<LevelController>().Levels[0].name);
        _LevelController = Instantiate(LevelControllerPrefab);
        LevelController = _LevelController.GetComponent<LevelController>();
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
