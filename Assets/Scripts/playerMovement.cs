using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    Rigidbody rb;
    float moveSpeed;
    public canvasController cc;
    Vector3 startingPosition;
    Quaternion startingRotation;
    int goalCollisions;
    bool finished;
    public GameObject mainCamera, goalCamera, stars;
    float finishTime, celebrationDelay = 3f;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        startingPosition = this.transform.position;
        startingRotation = this.transform.rotation;
        goalCollisions = 0;
        moveSpeed = 1000f;
        finished = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (cc.gameOver || Time.time - cc.startTime < cc.startDelay)
        {
            rb.Sleep();
        }
        else if (finished)
        {
            if (Time.time - finishTime < celebrationDelay)
            {
                rb.velocity = rb.velocity * 0.99f * Time.deltaTime;
            }
            else
            {
                rb.AddForce(Vector3.up * 5000f * Time.deltaTime);
                stars.SetActive(true);
                cc.celebrationDone = true;
            }
        }
        else
        {
            float hdir = Input.GetAxisRaw("Horizontal");
            float vdir = Input.GetAxisRaw("Vertical");
            Vector3 inputDirection = new Vector3(hdir, 0, vdir);
            Vector3 trueDirection = Camera.main.transform.TransformDirection(inputDirection);
            trueDirection.y = 0.0f;

            Vector3 norm = trueDirection.normalized;

            Vector3 force = norm * moveSpeed * Time.deltaTime;
            rb.AddForce(force);

            if (this.transform.position.y < -15f)
            {
                ReturnToStart();
            }
        }
    }

    void ReturnToStart()
    {
        cc.DecreaseLives();
        cc.IncreaseScore(-100);
        rb.Sleep();
        this.transform.position = startingPosition;
        this.transform.rotation = startingRotation;
    }

    void switchCamera()
    {
        mainCamera.SetActive(false);
        goalCamera.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Collectible")
        {
            if (other.GetComponent<collectibleController>().isCollected == false)
            {
                other.GetComponent<collectibleController>().isCollected = true;
                cc.IncreaseScore(200);
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
                finishTime = Time.time;
                Invoke("setFinished", 0.5f);
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

    void setFinished()
    {
        finished = true;
    }
}
