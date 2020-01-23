using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class canvasController : MonoBehaviour
{

    public GameObject scoreText, timeText, goalFront, goalBack;
    public GameObject mainText, livesText, bananasText, winUI, totalScoreText;
    public GameObject trophyAlert, recordAlert;

    //Current attempt's information
    int totalScore = 0;
    int score = 0;
    int lives = 3;
    int bananas = 0;
    float startTime;
    float finishTime;
    public string state = "start"; //Used in playerMovement.cs and orbit.cs
    bool flashing = false;

    //Global information
    int level;
    float maxTime;

    //General Information
    public float startDuration = 5f; //Orbit.cs needs to know it for camera animation
    float warningDuration = 3f;
    float celebrateDuration = 2f;
    float deathDuration = 2f;

    //Array for notification alerts
    List<Coroutine> alerts = new List<Coroutine>();

    void Start()
    {
        //Obtain global information
        level = Globals.currentLevel;
        LevelData L = Globals.levels[level];
        maxTime = L.maxTime;

        //Set all texts to initial values
        scoreText.GetComponent<Text>().text = score.ToString();
        livesText.GetComponent<Text>().text = lives.ToString();
        bananasText.GetComponent<Text>().text = bananas.ToString();
        mainText.GetComponent<Text>().text = "READY?";
        updateTime(maxTime);

        //Begin the timer
        startTime = Time.time;
    }

    void Update()
    {
        states();
        transitions();
    }

    void states()
    {
        switch (state)
        {
            case "start":
                break;

            case "play":

                //Track and update time
                float timeLeft = maxTime - (Time.time - startTime - startDuration);
                updateTime(timeLeft);

                //Start flashing time if within warning time
                if(timeLeft < warningDuration && flashing == false)
                {
                    flashing = true;
                    StartCoroutine("flashTimeColor");
                }
                break;

            case "takeoff":
                if (totalScore == 0)
                {
                    int subscore = score + Mathf.FloorToInt((finishTime - startTime) - startDuration);
                    int multiplier = lives + bananas;
                    totalScore = subscore * multiplier;
                    totalScoreText.GetComponent<Text>().text = totalScore.ToString();
                    winUI.SetActive(true);
                    LevelData L = Globals.levels[level];
                    if (L.newHighscore(totalScore))
                    {
                        alerts.Add(StartCoroutine(spawnAlert(recordAlert, "HIGHSCORE", alerts.Count)));
                    }
                }
                
                break;

            default:
                break;
        }
    }

    void transitions()
    {
        switch (state)
        {
            case "start":
                //Show Go once play begins
                float time = Time.time - startTime;
                if (time >= startDuration)
                {
                    StartCoroutine("showGo");
                    state = "play";
                }
                break;

            case "play":
                //Game Over if time is out
                float timeLeft = maxTime - (Time.time - startTime - startDuration);
                if (timeLeft <= 0)
                {
                    state = "gameOver";
                    mainText.GetComponent<Text>().text = "GAME OVER";
                    finishTime = Time.time;
                }
                break;

            case "gameOver":
                //If death duration is over and no more alerts, then return to levelSelect
                if(Time.time - finishTime < deathDuration && alerts.Count == 0)
                {
                    SceneManager.LoadScene("levelSelect");
                }
                break;

            case "celebrate":
                if(Time.time - finishTime > celebrateDuration)
                {
                    state = "takeoff";
                    mainText.GetComponent<Text>().text = "";
                }
                break;

            default:
                break;
        }
    }
    
    //Coroutine shows Go! for one second
    IEnumerator showGo()
    {
        mainText.GetComponent<Text>().text = "GO!";
        yield return new WaitForSeconds(1.0f);
        mainText.GetComponent<Text>().text = "";
    }

    //Coroutine flashes time text as warning
    IEnumerator flashTimeColor()
    {
        while (state == "play")
        {
            timeText.GetComponent<Text>().color = new Color(255f, 0f, 0f);
            yield return new WaitForSeconds(0.2f);
            timeText.GetComponent<Text>().color = new Color(255f, 255f, 255f);
        }
    }

    //Function updates all time texts
    void updateTime(float time)
    {
        timeText.GetComponent<Text>().text = scoreboard(time);
        goalFront.GetComponent<Text>().text = scoreboard(time);
        goalBack.GetComponent<Text>().text = scoreboard(time);
    }

    //Function increases score and bananas
    public void eatBanana(int scoreAmount, int bananaAmount)
    {
        score += scoreAmount;
        bananas += bananaAmount;
        scoreText.GetComponent<Text>().text = score.ToString();
        bananasText.GetComponent<Text>().text = bananas.ToString();
    }

    //Function decreases lives and score
    public void DecreaseLives()
    {
        lives -= 1;
        score -= 100;
        scoreText.GetComponent<Text>().text = score.ToString();

        if (lives < 0)
        {
            state = "gameOver";
            finishTime = Time.time;
        }
        else
        {
            livesText.GetComponent<Text>().text = lives.ToString();
        }
    }

    //Function to transition to celebrate and compute stats
    public void ShowGoal()
    {
        //Transition stuff
        state = "celebrate";
        finishTime = Time.time;
        mainText.GetComponent<Text>().text = "GOAL!";
        //Global data update and spawn alerts
        float timeSpent = (Time.time - startTime) - startDuration;
        LevelData L = Globals.levels[level];
        L.completed = true;
        if (L.newBestTime(timeSpent))
        {
            alerts.Add(StartCoroutine(spawnAlert(recordAlert, "TIME", alerts.Count)));
        }
        if (L.newBanana(bananas))
        {
            alerts.Add(StartCoroutine(spawnAlert(recordAlert, "BANANAS", alerts.Count)));
        }
        
    }

    //Coroutine spawn a notification
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

    public void loadLevels()
    {
        SceneManager.LoadScene("levelSelect");
    }

    public void reloadScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}