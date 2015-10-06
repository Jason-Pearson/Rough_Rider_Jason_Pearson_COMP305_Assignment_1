/* Author: Jason Pearson
 * File: RoadController.cs
 * Creation Date: Sept 29, 2015
 * Description: This script controls the road gameObjects's scrolling movement and reset position
 * Last Modified Date: Oct 5, 2015
 */

using UnityEngine;
using System.Collections;

public class RoadController : MonoBehaviour {
    //PUBLIC INSTANCE VARIABLES
    public float speed; // speed = 5pixels/frame * 60frames/sec = moving 300pixels/sec

	// Use this for initialization
	void Start () {

        this._Reset();//start Road object in correct position (x = +800)
	}
	
	// Update is called once per frame
	void Update () {
        //Every frame - check and store the current position of the Road gameObject
        Vector2 currentPosition = new Vector2(0.0f, 0.0f); // vector2 variable to store the current position of the Road gameObject -
        currentPosition = gameObject.GetComponent<Transform>().position; // - via the Transform component of the gameObject
        currentPosition.x -= speed; // subtract speed value (=5) from current position - moving the starting x = +800 decrementing to x= -800 (which then it should reset)
        
        //assign Road gameObject to the new currentPosition that is decremented by speed
        gameObject.GetComponent<Transform>().position = currentPosition;

        //checks when Road gameObject meets the far right of camera's viewport
        if (currentPosition.x <= -800) // if the current position is less-than-equal to x = -800
        {
            this._Reset(); // - then reset Road gameObject's position to the reset position (x = +800) - via the _Reset method 
        }
	} //and then the cycle repeats...

    //PRIVATE METHODS
    private void _Reset() //resets position of Road gameobject when it hits -800 on the x-axis - back to +800
    {
        Vector2 resetPosition = new Vector2(800f, 0.0f); //to start Road at x = +800
        gameObject.GetComponent<Transform>().position = resetPosition; //since a vector2 is needed for modifying the Road gameObject's position in its Transform component
    }
}
