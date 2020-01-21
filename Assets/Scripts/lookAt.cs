using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookAt : MonoBehaviour
{
    public GameObject obj;

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(obj.transform);
    }
}
