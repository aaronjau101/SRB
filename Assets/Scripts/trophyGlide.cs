using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trophyGlide : MonoBehaviour
{
    public bool gliding = false;
    public float glideTime = 1f;
    float t = 0;
    Vector3 offset = new Vector3(15f, 0, 0);
    Vector3 startPos, endPos;

    void Update()
    {
        if (gliding)
        {
            t += Time.deltaTime / glideTime;
            transform.position = Vector3.Lerp(startPos, endPos, t);
        }
    }

    IEnumerator toggleGliding()
    {
        gliding = true;
        yield return new WaitForSeconds(glideTime);
        t = 0;
        gliding = false;
    }

    public void glideLeft()
    {
        startPos = this.transform.position;
        endPos = startPos - offset;
        StartCoroutine(toggleGliding());
    }

    public void glideRight()
    {
        startPos = this.transform.position;
        endPos = startPos + offset;
        StartCoroutine(toggleGliding());
    }
}
