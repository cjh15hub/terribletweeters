using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    [SerializeField]
    private SceneAsset MenuScene;


    void Start()
    {
        if (MenuScene != null && SceneManager.GetSceneByName(MenuScene.name).isLoaded == false)
        {
            SceneManager.LoadScene(MenuScene.name, LoadSceneMode.Additive);
        }
    }
}
