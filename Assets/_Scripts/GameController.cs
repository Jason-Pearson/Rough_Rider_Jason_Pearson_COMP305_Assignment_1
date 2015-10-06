/* Author: Jason Pearson
 * File: GameController.cs
 * Creation Date: Sept 29, 2015
 * Description: This script controls the aspects of the scene by instantiating the pickups and enemies (being able to set how many as well), and regulate the End Game State via Player Health = 0
 * Last Modified Date: Oct 5, 2015
 */
using UnityEngine;
using System.Collections;

[System.Serializable]
public class Pickups
{
    public int goldCount; //the number of gold pickup objects to instantiate
    public GameObject gold; //GameObject reference for the pickup gameObject (gold) to be instantiated via this script

    public int silverCount; //the number of silver pickup objects to instantiate
    public GameObject silver; //GameObject reference for the pickup gameObject (silver) to be instantiated via this script

    public int wrenchCount; //the number of wrench pickup objects to instantiate
    public GameObject wrench; //GameObject reference for the pickup gameObject (wrench) to be instantiated via this script

}
public class GameController : MonoBehaviour {
    //PUBLIC INSTANCE VARIABLES
    public Pickups pickups; // reference of the Pickups class in order to modify its properties
    public GameObject enemy; // reference for the enemy gameobject for instantiation
    public int enemyCount; // to set how many enemies to instantiate
    public PlayerController playerControllerScript; // reference for the Player Controller script of type PlayerController class
    public bool restart; // boolean for when it is okay to track the 'R' key press to restart the scene - if health = 0

    //PRIVATE INSTANCE VARIABLES
    private AudioSource[] _audioSources; // audio sources array to store all audio source associated with the GameController gameobject through this script
    private AudioSource _backgroundAudioSource,_gameOverAudioSource; // audio source reference vaiables to hold background music and gameover music 

    // Use this for initialization
	void Start () {
        this._audioSources = this.GetComponents<AudioSource>(); // at start, have array hold all current audio sources associated with the GameController object
        this._backgroundAudioSource = this._audioSources[0]; // this reference holds background music
        this._gameOverAudioSource = this._audioSources[1];// this reference holds game over music
        this._backgroundAudioSource.Play(); // at start - play background music
        restart = false; // set restart to false at start

        GameObject playerObject = GameObject.FindWithTag("Player"); //create reference for Player gameobject, and assign the variable via FindWithTag at start
        if (playerObject != null) // if the playerObject gameObject-reference is not null - assigning the reference via FindWithTag at first frame -
        {
            playerControllerScript = playerObject.GetComponent<PlayerController>();// - set the PlayerController-reference (called playerControllerScript) to the <script component> of the Player gameobject (via the gameObject-reference) to have access the instance of the PlayerController script
        }
        if (playerObject == null) //for exception handling - to have the console debug the absense of a player controller script in order for this entire code, the code in the GameController to work
        {
            Debug.Log("Cannot find PlayerController script for Health End State in GameController");
        }
        this._GeneratePickups(); // at start, generate pickups via tis method
        this._GenerateEnemy(); // at start, generate enemies via this method
	}
	
	// Update is called once per frame
    void Update()
    {   
        //check every frame - if player health == 0 - call endgamehealthstate method
        if (playerControllerScript != null)// needed in order not to reference the PlayerController script once it doesn't exist after the game object (player) associating with the PlayerController script itself is destroyed in the EndGameHealthState method
        {
            if (playerControllerScript.health <= 0)//if health, through the instance of the PlayerController script, is less than or equal to zero
            {
                this._backgroundAudioSource.Stop(); // stop the background music -
                this._gameOverAudioSource.Play(); // and start the game over music
                restart = true; // set restart to true

                playerControllerScript._EndGameHealthState(); // call EndGameHealthState method, through instance of PlayerController script
                Destroy(playerControllerScript); // - which in turn makes this necessary in order not to have Unity not have the MissingReferenceException error for the Player Controller once the Player object is destroyed in the EndGameHealthState method
            }
        }
            
            if (restart)//if restart is true within the game over if-statement (if health = 0) 
            {
                if (Input.GetKeyDown(KeyCode.R))//check for the key press if it is R, then -
                {
                    Application.LoadLevel(Application.loadedLevel); // - reload the current loaded level/scene file
                }
            }
    }
    //PRIVATE METHODS
    private void _GenerateEnemy() // method to instantiate enemies based on enemyCount set in the Inspector
    {
        if (enemyCount > 0) // if enemy count is greater than 0 (1 is inclusive)
        {
            for (int count = 0; count < this.enemyCount; count++) // instantiate the number of enemies (clones of the enemy prefab) into the scene - which is once since it is called in Start()
            {
                Instantiate(enemy);
            }

        }
    }
    //Same logic of _GenerateEnemy for this method - for each pickup - instantiate number of pickups - call method on Start() so there is only one set of prefab clones instantiated for the scene
    private void _GeneratePickups()
    {
        if (pickups.goldCount > 0)
        {
            for (int count = 0; count < this.pickups.goldCount; count++)
            {
                Instantiate(pickups.gold);
            }

        }
        if (pickups.silverCount > 0)
        {
            for (int count = 0; count < this.pickups.silverCount; count++)
            {
                Instantiate(pickups.silver);
            }

        }
        if (pickups.wrenchCount > 0)
        {
            for (int count = 0; count < this.pickups.wrenchCount; count++)
            {
                Instantiate(pickups.wrench);
            }
        }
    }
}
