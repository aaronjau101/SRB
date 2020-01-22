using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class canvasController : MonoBehaviour
{

    public GameObject scoreText, timeText, goalFront, goalBack, mainText, livesText, bananasText;
    public GameObject trophyAlert, recordAlert;
    int score, lives, bananas;
    public int level;
    public float startTime;
    public float maxTime = 30f;
    public float startDelay = 5.0f;
    public float warningStart = 3.0f;
    public bool gameOver = false;
    bool counting = true;
    float timeLeft = 0;
    bool exitStarted = false;
    public bool celebrationDone = false;
    List<Coroutine> alerts = new List<Coroutine>();

    void Start()
    {
        score = 0;
        lives = 3;
        bananas = 0;
        scoreText.GetComponent<Text>().text = score.ToString();
        livesText.GetComponent<Text>().text = lives.ToString();
        bananasText.GetComponent<Text>().text = bananas.ToString();
        mainText.GetComponent<Text>().text = "";
        updateTime(maxTime);
        startTime = Time.time;
    }

    private void Update()
    {
        if(counting == false)
        {
            if(timeLeft == 0)
            {
                if(celebrationDone && exitStarted == false && alerts.Count == 0)
                {
                    LevelData L = Globals.levels[level];
                    if (L.newHighscore(score))
                    {
                        alerts.Add(StartCoroutine(spawnAlert(recordAlert, "HIGHSCORE", alerts.Count)));
                    }
                    Invoke("loadLevels", 2f);
                    exitStarted = true;  
                }
            }
            else
            {
                transferPoints();
            }
            
            return;
        }
        float time = Time.time - startTime;
        if (time < startDelay - 1.0f)
        {
            mainText.GetComponent<Text>().text = "READY?";
        } else if (time < startDelay)
        {
            mainText.GetComponent<Text>().text = "GO!";
        }
        else if (time - startDelay < maxTime - warningStart)
        {
            mainText.GetComponent<Text>().text = "";
            updateTime(maxTime + startDelay - time);
        }
        else if (time - startDelay < maxTime)
        {
            if (Mathf.Floor(time*10) % 2f == 0) { 
                timeText.GetComponent<Text>().color = new Color(255f, 0f, 0f);
            }
            else
            {
                timeText.GetComponent<Text>().color = new Color(255f, 255f, 255f);
            }
            updateTime(maxTime + startDelay - time);

        }
        else
        {
            updateTime(maxTime + startDelay - time);
            endGame();
        }
    }

    void transferPoints()
    {
        if (timeLeft > 0.1f)
        {
            timeLeft -= 0.1f;
        }
        else
        {
            timeLeft = 0;
        }
        timeText.GetComponent<Text>().text = scoreboard(timeLeft);
        score += 10;
        scoreText.GetComponent<Text>().text = score.ToString();
        
    }

    void updateTime(float time)
    {
        timeText.GetComponent<Text>().text = scoreboard(time);
        goalFront.GetComponent<Text>().text = scoreboard(time);
        goalBack.GetComponent<Text>().text = scoreboard(time);
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
        bananas += 1;
        scoreText.GetComponent<Text>().text = score.ToString();
        bananasText.GetComponent<Text>().text = bananas.ToString();
    }

    void endGame()
    {
        gameOver = true;
        mainText.GetComponent<Text>().text = "GAME OVER";
        Invoke("loadLevels", 2f);
    }

    void loadMenu()
    {
        SceneManager.LoadScene("menu");
    }

    void loadLevels()
    {
        SceneManager.LoadScene("levelSelect");
    }

    public void DecreaseLives()
    {
        lives -= 1;
        
        if(lives < 0)
        {  
            counting = false;
            endGame();
        }
        else
        {
            livesText.GetComponent<Text>().text = lives.ToString();
        }
    }

    public void ShowGoal()
    {
        counting = false;
        float timeSpent = (Time.time - startTime) - startDelay;
        timeLeft = maxTime - timeSpent;     
        mainText.GetComponent<Text>().text = "GOAL!";

        LevelData L = Globals.levels[level];
        if (L.newBestTime(timeSpent))
        {
            alerts.Add(StartCoroutine(spawnAlert(recordAlert, "TIME", alerts.Count)));
        }
        if (L.newBanana(bananas))
        {
            alerts.Add(StartCoroutine(spawnAlert(recordAlert, "BANANAS", alerts.Count)));
        }
        Destroy(mainText, 3f);
    }

    IEnumerator spawnAlert(GameObject prefab, string description, int index)
    {
        yield return new WaitForSeconds(2.0f * (float)index);
        GameObject alert = Instantiate(prefab, this.transform) as GameObject;
        alert.GetComponent<Notification>().setDescription(description);
        yield return new WaitForSeconds(2.0f * (float)index);
        alerts.RemoveAt(0);
    }

    string scoreboard(float amount)
    {
        if(amount >= 100 || amount <= 0)
        {
            return "00:00";
        }

        string result = Mathf.Floor((amount*100)).ToString();
        while(result.Length < 4)
        {
            result = result.Insert(0, "0");
        }
        result = result.Insert(2, ":");
        return result;
    }
}
