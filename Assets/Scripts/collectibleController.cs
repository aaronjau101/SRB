using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collectibleController : MonoBehaviour
{
    public bool isCollected;
    AudioSource collectSound;
    bool hasPlayed;
    float hoverHeight = 1f;
    float rotationLength = 180f;
    Vector3 startPosition;
    Vector3 startRotation;

    void Start()
    {
        isCollected = false;
        collectSound = this.GetComponent<AudioSource>();
        hasPlayed = false;
        startPosition = this.transform.position;
        startRotation = this.transform.eulerAngles;
    }

    void Update()
    {
        if(isCollected)
        {
            Destroy(this.gameObject, 1f);
            Disappear();
            if(hasPlayed == false)
            {
                hasPlayed = true;
                collectSound.Play();
            }
        }
        else
        {
            transform.position = new Vector3(transform.position.x, startPosition.y + Mathf.PingPong(Time.time, hoverHeight), transform.position.z);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, startRotation.y + Mathf.PingPong(Time.time * 100f, rotationLength), transform.eulerAngles.z);
        }
    }

    void Disappear()
    {
        this.transform.Rotate(Vector3.up, 720 * Time.deltaTime);
        this.transform.Translate(Vector3.up * 2f * Time.deltaTime);
        this.transform.localScale += new Vector3(0.25f, 0.25f, 0.25f) * Time.deltaTime;
    }
}
