using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class trophyRoom : MonoBehaviour
{

    public GameObject trophyText, percentageText, bananaTrophy, scoreTrophy, timeTrophy, noTrophy;

    GameObject currentTrophy;
    int index = 0;

    Vector3 right = new Vector3(15f, 0, 0);
    Vector3 left = new Vector3(-15f, 0, 0);

    void Start()
    {
        currentTrophy = newTrophy(Vector3.zero);
        StartCoroutine(updateDescription(0));

        float trophyPercentage = Mathf.Floor((getTrophiesUnlocked() / (float)Globals.trophies.Length) * 100);
        percentageText.GetComponent<Text>().text = trophyPercentage.ToString() + "%";
    }

    float getTrophiesUnlocked ()
    {
        float count = 0;
        for (var i = 0; i < Globals.trophies.Length; i++)
        {
            if(Globals.trophies[i].unlocked)
            {
                count += 1;
            }
        }
        return count;
    }

    IEnumerator updateDescription(float delay)
    {
        trophyText.GetComponent<Text>().text = "";
        yield return new WaitForSeconds(delay);
        Trophy currentTrophy = Globals.trophies[index];
        trophyText.GetComponent<Text>().text = currentTrophy.description.ToUpper();
    }

    GameObject newTrophy(Vector3 startPos)
    {
        Trophy trophy = Globals.trophies[index];
        
        if (trophy.unlocked)
        {
            string s = trophy.description;
            if (s.Contains("Completed") || s.Contains("Time")) {
                return Instantiate(timeTrophy, startPos, Quaternion.Euler(Vector3.zero));
            }
            else if(s.Contains("Score"))
            {
                return Instantiate(scoreTrophy, startPos, Quaternion.Euler(Vector3.zero));
            }
            else
            {
                return Instantiate(bananaTrophy, startPos, Quaternion.Euler(Vector3.zero));
            }
        }
        else
        {
            return Instantiate(noTrophy, startPos, Quaternion.Euler(Vector3.zero));
        }
        
    }

    public void nextTrophy()
    {
        if(currentTrophy.GetComponent<trophyGlide>().gliding)
        {
            return;
        }
        index = index < Globals.trophies.Length - 1 ? index + 1 : 0;
        GameObject temp = currentTrophy;
        temp.GetComponent<trophyGlide>().glideLeft();
        float glideTime = temp.GetComponent<trophyGlide>().glideTime;
        Destroy(temp, glideTime);
        currentTrophy = newTrophy(right);
        currentTrophy.GetComponent<trophyGlide>().glideLeft();
        StartCoroutine(updateDescription(glideTime));
    }

    public void prevTrophy()
    {
        if (currentTrophy.GetComponent<trophyGlide>().gliding)
        {
            return;
        }
        index = index > 0 ? index - 1 : Globals.trophies.Length - 1;
        GameObject temp = currentTrophy;
        temp.GetComponent<trophyGlide>().glideRight();
        float glideTime = temp.GetComponent<trophyGlide>().glideTime;
        Destroy(temp, glideTime);
        currentTrophy = newTrophy(left);
        currentTrophy.GetComponent<trophyGlide>().glideRight();
        StartCoroutine(updateDescription(glideTime));
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("levelSelect");
    }
}
