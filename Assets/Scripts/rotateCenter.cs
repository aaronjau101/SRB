using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateCenter : MonoBehaviour
{

    public float turnSpeed;
    public Transform center;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.rotation = Quaternion.AngleAxis(Time.deltaTime * turnSpeed, Vector3.up) * this.transform.rotation;
    }
}
