/* Author: Jason Pearson
 * File: Pickup.cs
 * Creation Date: Sept 29, 2015
 * Description: This script controls all pickup gameObjects's (gold and silver coin, wrench) scrolling movement and reset position
 * Last Modified Date: Oct 5, 2015
 */
using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {
    //PUBLIC INSTANCE VARIABLES
    public float speed; // speed = 5pixels/frame * 60frames/sec = moving 300pixels/sec
    
    // Use this for initialization
    void Start()
    {
        this._Reset();//start Pickup object in original position (x = +425 to x = +800)
    }

    // Update is called once per frame
    void Update()
    {
        //Every frame - check and store the current position of the pickup gameObject
        Vector2 currentPosition = new Vector2(0.0f, 0.0f); // vector2 variable to store the current position of the pickup gameObject -
        currentPosition = gameObject.GetComponent<Transform>().position; // - via the Transform component of the gameObject
        currentPosition.x -= speed; // subtract speed value (=5) from current position - moving the starting x = (+425 to +800) decrementing to x= -425 (which then it should reset)
        //assign pickup gameObject to the new currentPosition that is decremented by speed
        gameObject.GetComponent<Transform>().position = currentPosition;

        //checks when pickup gameObject meets the far left of camera's viewport
        if (currentPosition.x <= -425) // if the current position is less-than-equal to x = -425 -
        {
            this._Reset(); // - then reset pickup gameObject's position to the reset position x = (+425 to+800) - via the _Reset method 
        }
    }

    //PRIVATE METHODS
    private void _Reset() //resets position of pickup gameobject when it hits -425 on the x-axis - back to +425 to +800
    {
        Vector2 resetPosition = new Vector2(Random.Range(425f, 800f), Random.Range(-245f, 260f)); //to start pickup randome between x = +425 and x = +080 and y = -245 to y = 260
        gameObject.GetComponent<Transform>().position = resetPosition; //assignment pickup object's position to resetPosition
    }
    void OnTriggerEnter2D(Collider2D otherGameObject) // when a pickup collides with the Player
    {
        if (otherGameObject.tag == "Player")
        {
            this._Reset(); // reset the position of the pickup to continuous flow of the game objects without destrorying and re-instantiation
        }
    }
}
