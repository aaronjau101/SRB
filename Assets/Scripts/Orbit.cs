using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public float turnSpeed = 4.0f;
    public Transform player;
    public canvasController cc;

    private Vector3 offset;
    private Vector3 rotation;
    float yOffset = 4.5f;
    float zOffset = -8.0f;

    float t;
    Vector3 startPosition;
    Vector3 target;
    float timeToReachTarget;

    void Start()
    {
        t = 0;
        offset = new Vector3(0, yOffset, zOffset);
        rotation = new Vector3(29.358f, 0, 0);
        this.transform.eulerAngles = rotation;
        target = player.position + offset;
        timeToReachTarget = cc.startDelay;
        startPosition = target + (Vector3.up * 50f);
        this.transform.position = startPosition;
    }

    void Update()
    {
        if (Time.time - cc.startTime < cc.startDelay)
        {
            t += Time.deltaTime / timeToReachTarget;
            transform.position = Vector3.Lerp(startPosition, target, t);
        }
    }

    void LateUpdate()
    {
        if (Time.time - cc.startTime < cc.startDelay)
        {
            return;
        }
        else
        {
            offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * offset;
            transform.position = player.position + offset;
            transform.LookAt(player.position);
        }
    }
}
