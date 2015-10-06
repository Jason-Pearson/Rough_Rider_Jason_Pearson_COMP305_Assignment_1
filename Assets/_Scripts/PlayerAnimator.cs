/* Author: Jason Pearson
 * File: PlayerAnimator.cs
 * Creation Date: Sept 29, 2015
 * Description: This script controls the animation of the Player gameObject by having an array of sprite objects - cycle through them at a set framespersecond via The Inspector. This script is attached to the Player gameObject.
 * Last Modified Date: Oct 5, 2015
 */
using UnityEngine;
using System.Collections;

public class PlayerAnimator : MonoBehaviour {
    //PUBLIC INSTANCE VARIABLES
    public Sprite[] sprites; // sprite array for sprite images 
    public float framesPerSecond; // set framespersecond in Inspector
    //PRIVATE INSTANCE VARIABLE
    private SpriteRenderer _spriteRenderer; // reference to hold the spriterenderers of the images that are cycled per framespersecond

	// Use this for initialization
	void Start () {
        this._spriteRenderer = gameObject.GetComponent<SpriteRenderer>() as SpriteRenderer; // at start, spriteRenderer is the sprite renderer from the initial sprite image of the Player gameObject
	}
	
	// Update is called once per frame
	void Update () {
        int index = (int)(Time.timeSinceLevelLoad * this.framesPerSecond); //recast every frame per frame in game
        index = index % (this.sprites.Length - 1); //the index for the sprites array is divided by length of sprites, then modulus gets the remainder - and when remainder = 0, switch frames on tick count
        this._spriteRenderer.sprite = this.sprites[index]; //changes sprite renderer to one sprite image to another
    }
}
