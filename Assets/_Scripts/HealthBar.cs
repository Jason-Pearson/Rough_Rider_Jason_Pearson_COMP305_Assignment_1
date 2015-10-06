/* Author: Jason Pearson
 * File: HealthBar.cs
 * Creation Date: Sept 29, 2015
 * Description: This script animates the health bar for the player based on the health value cycling the index of the sprite array to the appropriate health bar sprite
 * Last Modified Date: Oct 5, 2015
 */
using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {
    public Sprite[] sprites; // sprite array for health bar sprite images
    public PlayerController playerControllerScript; // reference of type PlayerController to hold an instance of the PlayerController script

    private SpriteRenderer _spriteRenderer; // reference to hold the sprite renderer of the current sprite that is cycled to to show on the scene
    // Use this for initialization
    void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null) // if the gameControllerObject gameObject-reference is not null - after finding the GameController gameobject and holding it in this reference -
        {
            playerControllerScript = playerObject.GetComponent<PlayerController>();// - set the GameController-reference (called gameController) to the <script component> of the GameController gameobject (via the gameObject-reference) to have access the instance of the Game Controller script
        }
        if (playerObject == null) //for exception handling - to have the console debug the absense of a game controller script in order for this entire code, the code in the GameController - to work in order to display and increment the score to display as well in the UIText object
        {
            Debug.Log("Cannot find PlayerController script for Player Health dictating Health Bar sprites in HealthBar script");
        }

        this._spriteRenderer = gameObject.GetComponent<SpriteRenderer>() as SpriteRenderer; // at start, sprite renderer reference holds the intial image of the gameObject associated with this script (health bar)
    }

    // Update is called once per frame
    void Update()
    {
        //every frame - check the health of the player through the PlayerController reference, and cycle the 
        if(playerControllerScript.health == 100)
        {
            playerControllerScript.healthCounter.color = Color.yellow;
            this._spriteRenderer.sprite = this.sprites[0]; //changes sprite renderer to one sprite image to another
        }
        if (playerControllerScript.health < 100)
        {
            playerControllerScript.healthCounter.color = Color.yellow;
            this._spriteRenderer.sprite = this.sprites[1]; //changes sprite renderer to one sprite image to another
        }
        if (playerControllerScript.health <= 80)
        {
            playerControllerScript.healthCounter.color = Color.yellow;
            this._spriteRenderer.sprite = this.sprites[2]; //changes sprite renderer to one sprite image to another
        }
        if (playerControllerScript.health <= 60)
        {
            playerControllerScript.healthCounter.color = Color.white;
            this._spriteRenderer.sprite = this.sprites[3]; //changes sprite renderer to one sprite image to another
        }
        if (playerControllerScript.health <= 40)
        {
            playerControllerScript.healthCounter.color = Color.red;
            this._spriteRenderer.sprite = this.sprites[4]; //changes sprite renderer to one sprite image to another
        }
        if (playerControllerScript.health <= 20)
        {
            playerControllerScript.healthCounter.color = Color.red;
            this._spriteRenderer.sprite = this.sprites[5]; //changes sprite renderer to one sprite image to another
        }
        if (playerControllerScript.health <= 0)
        {
            Destroy(gameObject); //destroy Health Bar 2D sprite gameObject upon Player dying 
        }
    }

}
