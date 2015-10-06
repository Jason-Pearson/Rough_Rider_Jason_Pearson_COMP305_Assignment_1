/* Author: Jason Pearson
 * File: PlayerController.cs
 * Creation Date: Sept 29, 2015
 * Description: This script controls the movement, shooting the Blast (instantiating upon key press), explosions upon impact, enabling and disabling labels and updating scoring system upon collisions with pickups and destroying enemies, play the music assigned to Player contextually, update health and hold the health bar gameobject for visual representation, and to handle game over state via zero health for the Player gameobject.
 * Last Modified Date: Oct 5, 2015
 */
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//must serialize class in order to have properties of a new class (which can't be inherited now) be accessible to the Inspector
[System.Serializable]
public class Boundary // class to set a min and max for x and y axis - to create a boundary for logical purposes
{
    public float xMin, xMax, yMin, yMax;
}
public class PlayerController : MonoBehaviour {

    // PUBLIC INSTANCE VARIABLES
    public float speed; // adjust the speed of movement by adding the new position of the player object by speed(#) per unit
    public Boundary boundary; //create reference for the Boundary class
    
    public GameObject shot; // reference for shot gameobject to hold the 2D Blast sprite object
    public Transform shotSpawn; // holds transform position of the empty gameobject ShotSpawn - which is a child of the shot gameobject to instantiate in front of the player
    public float fireRate; // = 0.25 --> shoots 4 times a second --> 1/0.25
    private float nextFire; // = 0 
   
    public GameObject explosion;// reference the explosion 2D sprite gameobject for when player gets hit by laser or crash into enemy
    public Transform explosionSpawn;// holds the position of the explosion

    public Text scoreLabel; // reference for score label - to display score in the game scene
    public int score; // holds the score (starting at 0) to be added upon and updated to the score label
    public Text gameOverLabel;//shows game over label for end state after health =0
    public Text finalScoreLabel;// shows final score upon game over
    public int killCount; // to count the kills of enemies
    public Text enemyKilled; // to display the kills of enemies during gameplay via this label
    public Text finalKillCount;// displays the final count of kills upon the game over state
    public Text restartLabel;//shows the prompt to restart game upon game over state

    public int health; // holds the health for the Player - starting at 100
    public Text healthCounter; // visual counter for the Player health via this label

    public AudioSource[] _audioSources;//to hold an array of the current audio sources associated with the player -
    public AudioSource _playerExplosion; // - primarily to hold the player explosion sound in this to control playing upon collision

    // PRIVATE INSTANCE VARIABLES
    private Vector2 _newPosition = new Vector2(0.0f, 0.0f); // for the new position of the player via user input to equal the player object's position

    // Use this for initialization
    void Start()
    {
        this._audioSources = this.GetComponents<AudioSource>(); // at start, have array hold all current audio sources associated with the player object
        this._playerExplosion = this._audioSources[1];// assign this audio source reference with the certain index in the array
        Debug.Log("Player entered the scene"); // for debugging
        this._SetScore(); // upon first frame, set the score label via this method (score is set to 0, should show 0)
        this.healthCounter.text = health.ToString(); // set the health counter text in the label to the current health (100)

        //Disabled Labels to be Enabled and Displayed in the EndGameHealthState method
        this.restartLabel.enabled = false;
        this.gameOverLabel.enabled = false;
        this.finalScoreLabel.enabled = false;
        this.finalKillCount.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        this._CheckInput(); // check the input of the player every frame and move accordingly
        
        if (Input.GetButton("Fire1") && Time.time > nextFire) //Hold key and shoot every 0.25 sec by - game time > nextFire = 0
        {
            nextFire = Time.time + fireRate; // then update nextFire = gametime (now 0.2) + fireRate (0.25) --> then when game time is 0.27 > nextFire (0.26) and fire button is held = shoot Bolt prefab via the reference shot gameObject 
            Instantiate(shot, shotSpawn.position,shotSpawn.rotation);//instantiate the game object shot per frame at a held key press, set at a vector3 position, at a set quaternion euler (rotation)
            shot.GetComponent<AudioSource>().playOnAwake = true;//upon instantiating the (shot), if the audio isn't playing on awake (on the very first frame), play this audio clip
        }
    }

    //PRIVATE METHODS
    //Checks the input of the player via user input - update the player object's position to the new position via directional key press
    private void _CheckInput()
    {
        //if health = 0
        this._newPosition = gameObject.GetComponent<Transform>().position; // current position

        if (Input.GetAxis("Horizontal") > 0) // check if going right
        {
            this._newPosition.x += this.speed; // add move value to current position to have move in the desired units per frame
        }
        if (Input.GetAxis("Horizontal") < 0) // check if going left
        {
            this._newPosition.x -= this.speed; // subtract move value to current position
        }
        if (Input.GetAxis("Vertical") > 0) // check if going up
        {
            this._newPosition.y += this.speed; // add move value to current position
        }
        if (Input.GetAxis("Vertical") < 0) // check if going down
        {
            this._newPosition.y -= this.speed; // subtract move value to current position
        }

        this.BoundaryCheck(); //check if player is moving beyond boundary - make it a constant position

        gameObject.GetComponent<Transform>().position = this._newPosition; // new position (right or left or at rest)
    }
    private void BoundaryCheck() //check the new position to clamp within the boundaries of the x-axis
    {
        if (this._newPosition.x < this.boundary.xMin) // if the new position goes beyond the minimum -
        {
            this._newPosition.x = this.boundary.xMin; // the new position will be constant
        }
        // same thing for the maximum
        if (this._newPosition.x > this.boundary.xMax)
        {
            this._newPosition.x = this.boundary.xMax;
        }
        // same check for vertical movement
        if (this._newPosition.y < this.boundary.yMin)
        {
            this._newPosition.y = this.boundary.yMin;
        }
        if (this._newPosition.y > this.boundary.yMax)
        {
            this._newPosition.y = this.boundary.yMax;
        }
    }

    void OnTriggerEnter2D(Collider2D otherGameObject) // to control Player collision with - 
    {
        if (otherGameObject.tag == "Gold") // - Gold Pickup
        {
            this.score += 50; // add score by 50
            otherGameObject.GetComponent<AudioSource>().Play(); // and play the audio source associated with that pick object
        }
        if (otherGameObject.tag == "Silver") // - Silver Pickup
        {
            this.score += 25; // add score by 25
            otherGameObject.GetComponent<AudioSource>().Play(); // and play audio source
        }
        if (otherGameObject.tag == "Wrench") // Wrench Pickup
        {
            if (health == 100) // if health is already 100, and Player collides with Wrench pickup - don't do anything, particularly add upon health
            {
                Debug.Log("Health: FULL"); // for debugging
            }
            else// or else, if health does not equal 100 -
            {
                Debug.Log("Health (+5): " + health); 
                health += 5; // add health by 5 - this is a health pickup
                this.healthCounter.text = health.ToString(); // update healthCounter label to the current health as well
            }
            otherGameObject.GetComponent<AudioSource>().Play(); // play audio source every time player collides with this pickup
        }
        if (otherGameObject.tag == "Laser") // Player gets hit by Laser
        {
            this._playerExplosion.Play(); // play player explosion sound
            Instantiate(explosion, explosionSpawn.position, explosionSpawn.rotation); // instantiate explosion 2D sprite object at the set location of explosionSpawn variable
            Debug.Log("Health (-10): " + health); // for degugging
            health -= 10; // decrement health by 10
            this.healthCounter.text = health.ToString(); // update healthCounter label to current health
        }
        this._SetScore(); //after the if statements of colliding with pickup objects (gold and silver) set the score label with the new score via this method call
    }

    //PUBLIC METHODS 
    public void _SetEnemyKilled() // method to update the label to the current amount of the enemies killed
    {
        this.enemyKilled.text = "Enemy Killed: " + killCount; // label equals this string statement - killCount is concatenated to string for display
    }
    public void _SetScore() // method to update to the current score upon killing enemies or picking up Gold and Silver coins
    {
        this.scoreLabel.text = "Score: " + score; // label equals this string statement - score is concatenated to string for display
    }
    //End Game Method - called in Game Controller script - after player health = 0
    public void _EndGameHealthState()
    {
        Destroy(gameObject); // destroy player gameobject
        
        //Disable these Labels that are for Gameplay
        this.scoreLabel.enabled = false;
        this.enemyKilled.enabled = false;
        this.healthCounter.enabled = false;
        //Enable these labels for the Game Over screen
        this.finalScoreLabel.enabled = true;
        this.gameOverLabel.enabled = true;
        this.finalKillCount.enabled = true;
        this.restartLabel.enabled = true;
        this.finalScoreLabel.text = "Score: " + this.score; // set final score to this label
        this.finalKillCount.text = "Kills: " + this.killCount; // set final kill count to this label
        this.restartLabel.text = "Press 'R' to Restart"; // set restart label to this string statement, to notify play how to restart
        
    }
}
