using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{

    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private GameObject gameCanvas;
    [SerializeField] private GameManager gm;

    public void StartGame()
    {
		// Hides the menu and enables the game to begin running. Resets the playtime counter
        Debug.Log("Start Game");
        menuCanvas.SetActive(false);
        gameCanvas.SetActive(true);
        Time.timeScale = 1;
        gm.playTime = 0;
        
    }
    
    public void Restart()
    {
		// Reloads the game scene when pressed
        Debug.Log("Restart Pressed. Loading Scene: " + SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("GameScene");
    }

    public void Menu()
    {
		// As the menu is on the same scene and automatically opens when loaded, calls restart method
		// Otherwise would set the menu canvas to true or load a menu scene.
        Restart();
		
        //menuCanvas.SetActive(true);
		// SceneManager.LoadScene("MenuScene");
    }
    

    public void Quit()
    {
		// Closes the game process
		// In Webgl, this will just stop it running. User would have to refresh the page
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
