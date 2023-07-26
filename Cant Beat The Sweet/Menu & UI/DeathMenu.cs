using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using RDG;

public class DeathMenu : MonoBehaviour
{
    //Public&Serialised Variables
    [Header("Settings")]
    [SerializeField]
    bool _showLogs;

    [SerializeField]
    string _Prefix;

    [SerializeField]
    float loadDelay = 1f;

    [Header("menuObjects")]
    [SerializeField]
    Text scoreText;

    [SerializeField]
    Text highscoreText;

    [SerializeField]
    Image backgroundImg;

    [SerializeField]
    AudioSource engineAudio;

    //Private variables
    private SceneLoader.Scene _loadScene;

    private bool isShown = false;

    private float transition = -1.5f;

    // Start is called before the first frame update
    void Start()
    {
        //Disable logger if  not debug build
        Debug.unityLogger.logEnabled = Debug.isDebugBuild;

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Transitions the menu visibility for added flair.
        if (!isShown)
            return;

        transition += Time.deltaTime;
        backgroundImg.color = Color.Lerp(new Color(0, 0, 0, 0), Color.gray, transition);
    }

    //Enables the death menu when triggered
    public void ToggleEndMenu(float score)
    {
        gameObject.SetActive(true);
        scoreText.text = "Score\n " + ((int)score).ToString();
        highscoreText.text = "Highscore\n " + ((int)PlayerPrefs.GetFloat("Highscore")).ToString();
        isShown = true;
        Vibration.Vibrate(100);
        engineAudio.Stop();
    }

    //Restart button functionality. Reloads the game scene.
    public void Restart()
    {
        Vibration.Vibrate(100);
        StartCoroutine(DelaySceneLoad());
        _loadScene = SceneLoader.Scene.Game_Scene;
    }

    //menu button functionality. Loads the main menu specified.
    public void Menu()
    {
        Vibration.Vibrate(100);
        StartCoroutine(DelaySceneLoad());
       _loadScene = SceneLoader.Scene.MainMenu_Scene;
    }

    private IEnumerator DelaySceneLoad()
    {
        yield return new WaitForSeconds(loadDelay);
        Log("LoadDelay: " + loadDelay);
        SceneLoader.Load(_loadScene);
    }

    private void Log(object message)
    {
        if (_showLogs)
            Debug.Log(_Prefix + message);
    }
}
