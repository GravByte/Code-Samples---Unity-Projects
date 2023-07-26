using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using RDG;

public class MainMenu : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    bool _showLogs;

    [SerializeField]
    string _Prefix;

    [SerializeField]
    float loadDelay = 1f;

    [Header("Canvas")]
    [SerializeField]
    Canvas menuCanvas, settingsCanvas, upgradesCanvas, infoCanvas;

    [Header("Text")]
    [SerializeField]
    Text appVerText;

    private SceneLoader.Scene _loadScene;

    private void Awake()
    {

        //Disable logger if  not debug build
        Debug.unityLogger.logEnabled = Debug.isDebugBuild;

        //------- Disables all canvas on start except menu
        settingsCanvas.enabled = false;
        upgradesCanvas.enabled = false;
        infoCanvas.enabled = false;
        menuCanvas.enabled = true;
    }

    //----------- Loads game scene when button pressed
    public void ToGame()
    {
        Vibration.Vibrate(100);
        StartCoroutine(DelaySceneLoad());
        _loadScene = SceneLoader.Scene.Game_Scene;
        Log("Loaded Scene");
    }

    public void Menu()
    {
        Vibration.Vibrate(100);
        settingsCanvas.enabled = false;
        upgradesCanvas.enabled = false;
        infoCanvas.enabled = false;
        menuCanvas.enabled = true;
    }

    public void Settings()
    {
        Vibration.Vibrate(100);
        menuCanvas.enabled = false;
        settingsCanvas.enabled = true;
    }

    public void Upgrades()
    {
        Vibration.Vibrate(100);
        menuCanvas.enabled = false;
        settingsCanvas.enabled = false;
        upgradesCanvas.enabled = true;
    }

    public void Info()
    {
        Vibration.Vibrate(100);
        menuCanvas.enabled = false;
        settingsCanvas.enabled = false;
        upgradesCanvas.enabled = false;
        infoCanvas.enabled = true;

        appVerText.text = ("Version: " + Application.version);
    }

    //----------- Quits the game when button pressed
    public void Quit()
    {
        Vibration.Vibrate(100);
        Application.Quit();
        Log("Quit Game");
    }

    //----------- Delays loading the scene
    private IEnumerator DelaySceneLoad()
    {
        yield return new WaitForSeconds(loadDelay);
        Log("LoadDelay: " + loadDelay);
        SceneLoader.Load(_loadScene);
    }

    public void Log(object message)
    {
        if (_showLogs)
            Debug.Log(_Prefix + message);
    }
}
