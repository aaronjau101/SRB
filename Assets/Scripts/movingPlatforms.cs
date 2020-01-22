using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingPlatforms : MonoBehaviour
{
    public Vector3 offset = Vector3.zero;
    Vector3 startPosition;
    public float speed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float x = startPosition.x;
        float y = startPosition.y;
        float z = startPosition.z;

        if (offset.x != 0)
        {
            if (offset.x > 0)
            {
                x += Mathf.PingPong(Time.time * speed, offset.x);
            }
            else
            {
                x -= Mathf.PingPong(Time.time * speed, Mathf.Abs(offset.x));
            }
        }
        if (offset.y != 0)
        {
            if (offset.y > 0)
            {
                y += Mathf.PingPong(Time.time * speed, offset.y);
            }
            else
            {
                y -= Mathf.PingPong(Time.time * speed, Mathf.Abs(offset.y));
            }
        }
        if (offset.z != 0)
        {
            if (offset.z > 0)
            {
                z += Mathf.PingPong(Time.time * speed, offset.z);
            }
            else
            {
                z -= Mathf.PingPong(Time.time * speed, Mathf.Abs(offset.z));
            }
        }
        this.transform.position = new Vector3(x, y, z);
    }
}
