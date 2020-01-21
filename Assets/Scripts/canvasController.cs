using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class canvasController : MonoBehaviour
{

    public GameObject scoreText, timeText, goalFront, goalBack, mainText, livesText, bananasText;
    int score, lives, bananas;
    public float startTime;
    public float maxTime = 30f;
    public float startDelay = 5.0f;
    public float warningStart = 3.0f;
    public bool gameOver = false;
    bool counting = true;
    float timeLeft = 0;

    void Start()
    {
        score = Globals.score;
        lives = Globals.lives;
        bananas = Globals.bananas;
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
            transferPoints();
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
            //Make time red
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
        if(timeLeft == 0)
        {
            return;
        }
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
        Globals.reset();
        Invoke("loadMenu", 2f);
    }

    void loadMenu()
    {
        SceneManager.LoadScene("menu");
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
        timeLeft = maxTime + startDelay - Time.time;
        mainText.GetComponent<Text>().text = "GOAL!";
        Destroy(mainText, 3f);
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
