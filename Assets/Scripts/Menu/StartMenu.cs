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
    private SceneAsset MenuScene;

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

        if(MenuScene)
        {
            SceneManager.LoadScene(MenuScene.name, LoadSceneMode.Additive);
        }
    }

    public void StartGame()
    {
        _LevelController = Instantiate(LevelControllerPrefab);
        LevelController = _LevelController.GetComponent<LevelController>();
        SceneManager.LoadScene(LevelController.levels[0].name);
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
