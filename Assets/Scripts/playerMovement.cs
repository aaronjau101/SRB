using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    //GameObjects needed to be placed in inspector
    public GameObject mainCamera, goalCamera, stars;
    public canvasController cc;
    
    //General variables
    Quaternion startingRotation;
    Vector3 startingPosition;
    Rigidbody rb;
    float moveSpeed = 1000f;

    //Integer to track Goal collision
    int goalCollisions = 0;
    
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        startingPosition = this.transform.position;
        startingRotation = this.transform.rotation;
    }

    void Update()
    {
        //Dont move if starting or game is over
        if (cc.state == "start" || cc.state =="gameOver")
        {
            rb.Sleep();
        }
        //Slow down speed during celebration
        else if (cc.state == "celebrate")
        {
            rb.velocity = rb.velocity * 0.9f;
        }
        //Turn on stars and fly upwards
        else if (cc.state == "takeoff")
        {
            rb.AddForce(Vector3.up * 5000f * Time.deltaTime);
            if (stars.activeInHierarchy == false)
            {
                stars.SetActive(true);
            }
        }
        //Logic for playing the game
        else if(cc.state == "play")
        {
            //Move using input and camera direction
            float hdir = Input.GetAxisRaw("Horizontal");
            float vdir = Input.GetAxisRaw("Vertical");
            Vector3 inputDirection = new Vector3(hdir, 0, vdir);
            Vector3 trueDirection = Camera.main.transform.TransformDirection(inputDirection);
            trueDirection.y = 0.0f;
            Vector3 norm = trueDirection.normalized;
            Vector3 force = norm * moveSpeed * Time.deltaTime;
            rb.AddForce(force);
            //Reset player if falls off the map
            if (this.transform.position.y < -15f)
            {
                ReturnToStart();
            }
        }
    }

    //Function for resting when falling off the map
    void ReturnToStart()
    {
        cc.DecreaseLives();
        rb.Sleep();
        this.transform.position = startingPosition;
        this.transform.rotation = startingRotation;
    }

    //Function to switch to goal camera when celebrating
    void switchCamera()
    {
        mainCamera.SetActive(false);
        goalCamera.SetActive(true);
    }

    //Functions for trigger collision
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Collectible" && cc.state != "takeoff")
        {
            if (other.GetComponent<collectibleController>().isCollected == false)
            {
                other.GetComponent<collectibleController>().isCollected = true;
                cc.eatBanana(200, 1);
            }
        }
        if (other.gameObject.tag == "Goal")
        {
            int number = 0;
            int.TryParse(other.gameObject.name, out number);
            goalCollisions += number;
            if (goalCollisions == 6)
            {
                cc.ShowGoal();
                switchCamera();
            }
        }
        if (other.gameObject.tag == "Launch")
        {
            rb.AddForce(other.GetComponent<launch>().launchForce * Vector3.up);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Goal")
        {
            int number = 0;
            int.TryParse(other.gameObject.name, out number);
            goalCollisions -= number;
        }
    }
}
