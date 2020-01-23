using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonController : MonoBehaviour
{
    public int level;
    Color originalColor;
    Color selectedColor;
    // Start is called before the first frame update
    void Start()
    {
        originalColor = this.GetComponent<Button>().colors.normalColor;
        selectedColor = this.GetComponent<Button>().colors.pressedColor;
    }

    // Update is called once per frame
    void Update()
    {
        if(Globals.currentLevel == level)
        {
            ColorBlock cb = GetComponent<Button>().colors;
            cb.normalColor = selectedColor;
            cb.highlightedColor = selectedColor;
            GetComponent<Button>().colors = cb;
        }
        else
        {
            ColorBlock cb = GetComponent<Button>().colors;
            cb.normalColor = originalColor;
            cb.highlightedColor = originalColor;
            GetComponent<Button>().colors = cb;
        }
    }
}
