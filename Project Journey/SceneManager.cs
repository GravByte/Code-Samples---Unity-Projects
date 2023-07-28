using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    
    public static SceneManager Instance;
    
    public int currentSceneIndex;
    public string currentSceneName;
    
    public int intendedSceneIndex;
    public string nextScene;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        //---- change current scene index to the active scene index
        currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        Debug.Log("Current Scene Index: " + currentSceneIndex);
        currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        Debug.Log("Current Scene Name: " + currentSceneName);

        nextScene = currentSceneName switch
        {
            //---- Detect which scene is the next scene
            "IndustrialAgeScene" => "MachineAgeScene",
            "MachineAgeScene" => "AtomicAgeScene",
            "AtomicAgeScene" => "SpaceAgeScene",
            _ => nextScene
        };
    }
    
    public void LoadScene(string sceneName)
    {
        intendedSceneIndex = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName).buildIndex;
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
    
}
