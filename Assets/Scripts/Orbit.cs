using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public float turnSpeed = 4.0f;
    public Transform player;

    private Vector3 offset;

    public float yOffset = 5.0f;
    public float zOffset = -2.0f;

    void Start()
    {
        offset = new Vector3(player.position.x, player.position.y + yOffset, player.position.z + zOffset);
    }

    void LateUpdate()
    {
        offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * offset;
        transform.position = player.position + offset;
        transform.LookAt(player.position);
    }
}
