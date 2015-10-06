using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    // PUBLIC INSTANCE VARIABLES

    public float speed; // adjust the speed of movement
    public Boundary boundary; //create reference for the Boundary class in the other script, in order to use its properties be availiable in the Game Controller Inspector
    public SpriteRenderer enemy;

    public GameObject shot;
    public Transform shotSpawn; //this variable is a refernece of the game object Shot Spawn but the variable type only references its transform component
    public float fireRate; // = 0.25 --> shoots 4 times a second --> 1/0.25
    private float nextFire; // = 0 
    public int hitCount = 0;

    public GameObject explosion;
    public Transform explosionSpawn;

    public PlayerController playerControllerScript;

    private AudioSource[] _audioSources;
    private AudioSource _enemyExplosion;
    // PRIVATE INSTANCE VARIABLES
    private Vector2 _newPosition = new Vector2(0.0f, 0.0f); // for the new position of the player via user input to equal the player object's position

    // Use this for initialization
    void Start()
    {
        Debug.Log("Enemy entered the scene");
        this._audioSources = this.GetComponents<AudioSource>();
        this._enemyExplosion = this._audioSources[0];

        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null) // if the gameControllerObject gameObject-reference is not null - after finding the GameController gameobject and holding it in this reference -
        {
            playerControllerScript = playerObject.GetComponent<PlayerController>();// - set the GameController-reference (called gameController) to the <script component> of the GameController gameobject (via the gameObject-reference) to have access the instance of the Game Controller script
        }
        if (playerObject == null) //for exception handling - to have the console debug the absense of a game controller script in order for this entire code, the code in the GameController - to work in order to display and increment the score to display as well in the UIText object
        {
            Debug.Log("Cannot find PlayerController script for Enemy Scoring and Health Damage in EnemyController");
        }
        this._Reset(); // current position

        //this._newPosition.x -= this.speed; //automatically move left upon instantiation of enemy (which is out of the camera)
        //gameObject.GetComponent<Transform>().position = this._newPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextFire) //shoot every 0.5 sec by - game time > nextFire = 0
        {
            if(playerControllerScript != null)
            {
            if (playerControllerScript.health > 0)
            {
                if (this._newPosition.x < this.boundary.xMax)
                {
                    nextFire = Time.time + fireRate; // then update nextFire = gametime (now 0.2) + fireRate (0.25) --> then when game time is 0.27 > nextFire (0.26) and fire button is held = shoot Bolt prefab via the reference shot gameObject 
                    Instantiate(shot, shotSpawn.position, shotSpawn.rotation);//instantiate the game object shot per frame at a held key press, set at a vector3 position, at a set quaternion euler (rotation)
                    shot.GetComponent<AudioSource>().playOnAwake = true;//upon instantiating the (shot), if the audio isn't playing on awake (on the very first frame), play this audio clip
                }
            }
            else { Destroy(playerControllerScript); }
            }
        }
        this._newPosition = gameObject.GetComponent<Transform>().position; // current position
        this._newPosition.x -= this.speed; //automatically move left upon instantiation of enemy (which is out of the camera)
        gameObject.GetComponent<Transform>().position = this._newPosition;

        _CheckVisibleInput();
        //checks when Road gameObject meets the far right of camera's viewport
        if (_newPosition.x <= -425) // if the current position is less-than-equal to x = -800
        {
            this.speed = 1;
            this.hitCount = 0;
            this._Reset(); // - then reset Road gameObject's position to the reset position (x = +800) - via the _Reset method 
        }
    }

    private void _CheckVisibleInput()
    {
        this._newPosition = gameObject.GetComponent<Transform>().position; // current position

        if (enemy.isVisible)//when enemy becomes visible to the camera - have its movement be constantly checked by boundary and move in the opposite directoin upon bordering
        {
            //Horizontal Movement
            if (this._newPosition.x < this.boundary.xMin) // if the new position goes beyond the minimum -
            {
                this.speed = 5; // - speed is turned up higher and the enemy rushes the screen
            }
            this._newPosition.x -= this.speed;
        }
        gameObject.GetComponent<Transform>().position = this._newPosition;
    }
    private void _Reset() //resets position of Road gameobject when it hits -425 on the x-axis - back to +800
    {
        Vector2 resetPosition = new Vector2(Random.Range(445f, 530f), Random.Range(-180f, 180f)); //to start Road at x = +425
        gameObject.GetComponent<Transform>().position = resetPosition; //since a vector2 is needed for modifying the Road gameObject's position in its Transform component
    }
    void OnTriggerEnter2D(Collider2D otherGameObject) //other is a reference for Asteroid's trigger collider
    {
        if (otherGameObject.tag == "Player")
        {
            playerControllerScript._playerExplosion.Play();
            Instantiate(playerControllerScript.explosion, playerControllerScript.explosionSpawn.position, playerControllerScript.explosionSpawn.rotation);
            
            Debug.Log("Health (-20): " + playerControllerScript.health); 
            playerControllerScript.health -= 20;
            playerControllerScript.healthCounter.text = playerControllerScript.health.ToString();
            
            this.speed = 1;
            this.hitCount = 0;
            this._Reset();
        }
        if (otherGameObject.tag == "Blast")
        {
            this.hitCount++;
            if(this.hitCount == 2)
            {
            this._enemyExplosion.Play();
            Instantiate(explosion, explosionSpawn.position, explosionSpawn.rotation);
            playerControllerScript.killCount++;
            playerControllerScript._SetEnemyKilled();
            Debug.Log("Added 100 Points");
            playerControllerScript.score += 100;
            playerControllerScript._SetScore();
            this.hitCount = 0;
            this.speed = 1;
            this._Reset();
            }
        }
    }
    
}

