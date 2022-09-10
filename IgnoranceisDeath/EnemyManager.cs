using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

// Typescript motherfucker, do you speak it?
// Language: typescript

public class EnemyManager : MonoBehaviour {
    
    [Header("General")]
    private Rigidbody2D rb;
    public GameManager gm;
    private LightManager lm;
    public GameObject player;

    [Header("Text")]
    public GameObject hesHereText;
    
    [Header(("Monster"))]
    public bool isMonsterFree = false;
    public AudioSource monsterSFX;
    
    [Header("Door")]
    public GameObject door;
    public AudioSource doorUnlockSFX;

    [Header("Windows")]
    public GameObject[] windows;
    public GameObject chosenWindow;

    public GameObject postProcesser;

    private bool canZoom = true;
    public Slider AnxietyText;


    
    private void Awake() {
        // Find all the windows
        windows = GameObject.FindGameObjectsWithTag("Window");

        // Get the light manager
        lm = gm.GetComponent<LightManager>();
    }

    private void FixedUpdate()
    {
        // if the monster is free, move towards the player
        if (isMonsterFree)
        {
            transform.position = Vector2.MoveTowards(gameObject.transform.position, player.transform.position , 0.05f);
            player.GetComponent<AnxietyController>().raiseAnxiety(1f);


            // For zooming in when the monster arrives
            if(canZoom){
                canZoom = false;

                if(player.GetComponent<AnxietyController>().hasAttacked) return;
                StartCoroutine(postProcesser.GetComponent<PostProcessController>().DecreaseVision());
            }
        }

		// Test for monster spawn (or prank)
        if (!isMonsterFree)
        {
            if (Input.GetKeyDown(KeyCode.Keypad6))
            {
                ReleaseMonster();
            }
        }
        
    }

    public void HandleInteraction ( int interaction ) {

        // If the interaction is 0, go for a door
        if( interaction == 0 ) {
            AttackDoor();
        }
        else{
            AttackWindow();
        }
    }

    private void AttackDoor () { 
        // If the door is locked, unlock it
        if(door.GetComponent<DoorState>().isDoorLocked){
            door.GetComponent<DoorState>().isDoorLocked = false;
            
            // Play the door unlock sound
            doorUnlockSFX.Play();

            // Enable the lock icon
            door.transform.GetChild(0).gameObject.SetActive(true);

            // Change the sprite on the door
            door.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/DoorOpen");
        }
        else{
            // If the door is unlocked, open it and end it
            door.GetComponent<DoorState>().isDoorOpen = true;
            
            // Play the door unlock sound
            ReleaseMonster();
        }
    }

    private void AttackWindow () {
        chosenWindow = windows[Random.Range(0, windows.Length)];
        transform.position = chosenWindow.transform.position;
        
        // If the window is locked, unlock it
        if (chosenWindow.GetComponent<WindowState>().isWindowLocked)
        {
            // Unlock the window
            chosenWindow.GetComponent<WindowState>().UnlockWindow();
            
            // Play lighting effect
            StartCoroutine(lm.Lightning(chosenWindow));
        }

        // If the window is unlocked but closed, open it
        else if (!chosenWindow.GetComponent<WindowState>().isWindowOpen)
        {
            // Open the window
            chosenWindow.GetComponent<WindowState>().OpenWindow();

            // Play lightning
            StartCoroutine(lm.Lightning(chosenWindow));

            // Other effects
        }


        // Else means the monster has entered
        else {

            // End condition here
            ReleaseMonster();
        }
    }

    public void ReleaseMonster(){
                    
        // get first child object of the enemy and enable warning text
        transform.GetChild(0).gameObject.SetActive(true);
        hesHereText.SetActive(true);
            
        isMonsterFree = true;
        AnxietyText.gameObject.SetActive(false);
            
        monsterSFX.Play();
        
    }
}
