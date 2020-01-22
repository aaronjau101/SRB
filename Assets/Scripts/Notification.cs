using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    Vector3 startPosition, targetPosition;
    float timeToReachTarget;
    float t;
    bool fading = false;
    float fadeSpeed = 0.1f;
    public float offsetY;

    void Start()
    {
        t = 0;
        timeToReachTarget = 1f;
        float cw = this.transform.parent.GetComponent<RectTransform>().rect.width;
        float ch = this.transform.parent.GetComponent<RectTransform>().rect.height;
        float w = this.gameObject.GetComponent<RectTransform>().rect.width;
        float h = this.gameObject.GetComponent<RectTransform>().rect.height;
        startPosition = new Vector3((cw / 2) + (w / 2), offsetY, 0);
        targetPosition = new Vector3((cw / 2) - (w / 2), offsetY, 0);
        this.gameObject.GetComponent<RectTransform>().localPosition = startPosition;
        Invoke("fadeOut", timeToReachTarget * 2f);
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime / timeToReachTarget;
        this.gameObject.GetComponent<RectTransform>().localPosition = Vector3.Lerp(startPosition, targetPosition, t);
        if(fading)
        {
            float alpha = this.GetComponent<CanvasGroup>().alpha;
            if(alpha < fadeSpeed)
            {
                this.GetComponent<CanvasGroup>().alpha = 0;
                Destroy(this.gameObject);
            }
            else
            {
                this.GetComponent<CanvasGroup>().alpha -= fadeSpeed;
            }
        }
    }

    public void fadeOut()
    {
        fading = true;
    }

    public void setDescription(string description)
    {
        GameObject textObj = this.gameObject.transform.GetChild(2).gameObject;
        textObj.GetComponent<Text>().text = description;
    }
}
