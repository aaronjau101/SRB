using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public float turnSpeed = 4.0f;
    public Transform player;
    public canvasController cc;

    private Vector3 offset = new Vector3(0, 4.5f, -8.0f);
    private Vector3 startRotation = new Vector3(29.358f, 0, 0);

    private Vector3 startPosition;
    private Vector3 target;

    float timeToReachTarget;
    float t = 0;

    void Start()
    {
        //define initial values
        timeToReachTarget = cc.startDuration;
        target = player.position + offset;
        startPosition = target + (Vector3.up * 50f);
        //set the camera's transform
        this.transform.position = startPosition;
        this.transform.eulerAngles = startRotation;
    }

    void Update()
    {
        //move camera down during start
        if (cc.state == "start")
        {
            t += Time.deltaTime / timeToReachTarget;
            transform.position = Vector3.Lerp(startPosition, target, t);
        }
    }

    void LateUpdate()
    {
        //Only allow camera to orbit when playing
        if (cc.state == "play")
        { 
            offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * offset;
            transform.position = player.position + offset;
            transform.LookAt(player.position);
        }
        if(cc.state == "gameOver")
        {
            transform.position = player.position + offset;
            transform.LookAt(player.position);
        }
    }
}
