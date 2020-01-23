using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Globalization;

public class canvasController : MonoBehaviour
{

    public GameObject scoreText, timeText, goalFront, goalBack;
    public GameObject mainText, livesText, bananasText, winUI, totalScoreText, pauseUI;
    public GameObject trophyAlert, recordAlert;
    public AudioSource loseLifeAudio, gameOverAudio;

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
    float warningDuration = 5f;
    float celebrateDuration = 2f;
    float deathDuration = 2f;

    //Array for notification alerts
    List<Coroutine> recordAlerts = new List<Coroutine>();
    List<Coroutine> trophyAlerts = new List<Coroutine>();

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
                    StartCoroutine(flashTimeColor());
                }
                break;

            case "takeoff":
                if (totalScore == 0)
                {
                    float timeOnClock = maxTime - (finishTime - startTime - startDuration);
                    int timeScore = Mathf.FloorToInt(timeOnClock * 100);

                    int subscore = score + timeScore;
                    int multiplier = lives + bananas;
                    totalScore = subscore * multiplier;
                    totalScoreText.GetComponent<Text>().text = totalScore.ToString();
                    winUI.SetActive(true);
                    LevelData L = Globals.levels[level];
                    if (L.newHighscore(totalScore))
                    {
                        recordAlerts.Add(StartCoroutine(spawnAlert(recordAlert, "HIGHSCORE", recordAlerts)));
                    }
                    checkScoreTrophies(totalScore);
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
                //Pause game if P is pressed
                if (Input.GetKeyDown(KeyCode.P))
                {
                    Time.timeScale = 0;
                    state = "pause";
                    pauseUI.SetActive(true);
                }
                break;

            case "gameOver":
                //If death duration is over and no more alerts, then return to levelSelect
                if(Time.time - finishTime > deathDuration && recordAlerts.Count == 0 && trophyAlerts.Count == 0)
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

            case "pause":
                //Unpause game if P is pressed
                if (Input.GetKeyDown(KeyCode.P))
                {
                    Time.timeScale = 1;
                    state = "play";
                    pauseUI.SetActive(false);
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
            timeText.GetComponent<Text>().color = new Color(255f, 0, 0);
            yield return new WaitForSeconds(0.2f);
            timeText.GetComponent<Text>().color = new Color(0, 0, 0);
            yield return new WaitForSeconds(0.2f);
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
        LevelData L = Globals.levels[level];
        if (bananas == L.maxBananas)
        {
            Trophy t = findTrophy("Level " + (level + 1).ToString() + " All Bananas");
            if (t != null && t.unlocked == false)
            {
                t.unlocked = true;
                trophyAlerts.Add(StartCoroutine(spawnAlert(trophyAlert, "ALL NANAS", trophyAlerts)));
            }
        }
    }

    //Function decreases lives and score
    public void DecreaseLives()
    {
        lives -= 1;
        score -= 100;
        scoreText.GetComponent<Text>().text = score.ToString();
        livesText.GetComponent<Text>().text = lives.ToString();

        if (lives <= 0)
        {
            state = "gameOver";
            mainText.GetComponent<Text>().text = "GAME OVER";
            finishTime = Time.time;
            gameOverAudio.Play();
        }
        else
        { 
            loseLifeAudio.Play();
        }
    }

    //Function to transition to celebrate and compute stats
    public void ShowGoal()
    {
        //Transition stuff
        state = "celebrate";
        finishTime = Time.time;
        float timeSpent = finishTime - startTime - startDuration;
        float timeLeft = maxTime - timeSpent;
        updateTime(timeLeft);
        mainText.GetComponent<Text>().text = "GOAL!";
        //Global data update and spawn alerts
        
        LevelData L = Globals.levels[level];
        if(L.completed == false)
        {
            L.completed = true;
            Trophy t = findTrophy("Level " + (level + 1).ToString() + " Completed");
            if(t != null && t.unlocked == false)
            {
                t.unlocked = true;
                trophyAlerts.Add(StartCoroutine(spawnAlert(trophyAlert, "ANY TIME", trophyAlerts)));
            }
        }
        if (L.newBestTime(timeSpent))
        {
            recordAlerts.Add(StartCoroutine(spawnAlert(recordAlert, "TIME", recordAlerts)));
        }
        if (L.newBanana(bananas))
        {
            recordAlerts.Add(StartCoroutine(spawnAlert(recordAlert, "BANANAS", recordAlerts)));

        }
        checkTimeTrophies(timeSpent);
    }

    void checkTimeTrophies(float time)
    {
        LevelData L = Globals.levels[level];
        float[] times = { L.easyTime, L.mediumTime, L.hardTime };
        string[] difficulties = { "Easy", "Medium", "Hard" };
        for (var i = 0; i < 3; i++)
        {
            if (time < times[0])
            {
                string tName = "Level " + (level + 1).ToString() + " " + difficulties[i] +" Time";
                Trophy t = findTrophy(tName);
                if (t != null && t.unlocked == false)
                {
                    t.unlocked = true;
                    string alertName = difficulties[i].ToUpper(new CultureInfo("en-US", false)) + " TIME";
                    trophyAlerts.Add(StartCoroutine(spawnAlert(trophyAlert, alertName, trophyAlerts)));
                }
            }

        }

    }

    void checkScoreTrophies(float score)
    {
        LevelData L = Globals.levels[level];
        float[] scores = { L.easyScore, L.mediumScore, L.hardScore };
        string[] difficulties = { "Easy", "Medium", "Hard" };
        for (var i = 0; i < 3; i++)
        {
            if (score > scores[0])
            {
                string tName = "Level " + (level + 1).ToString() + " " + difficulties[i] + " Score";
                Trophy t = findTrophy(tName);
                if (t != null && t.unlocked == false)
                {
                    t.unlocked = true;
                    string alertName = difficulties[i].ToUpper(new CultureInfo("en-US", false)) + " SCORE";
                    trophyAlerts.Add(StartCoroutine(spawnAlert(trophyAlert, alertName, trophyAlerts)));
                }
            }

        }

    }

    Trophy findTrophy(string description)
    {
        for(var i = 0; i < Globals.trophies.Length; i++)
        {
            if(Globals.trophies[i].description == description)
            {
                return Globals.trophies[i];
            }
        }

        return null;
    }

    //Coroutine spawn a notification
    IEnumerator spawnAlert(GameObject prefab, string description, List<Coroutine> list)
    {
        int index = list.Count;
        yield return new WaitForSeconds(2.0f * (float)index);
        GameObject alert = Instantiate(prefab, this.transform) as GameObject;
        alert.GetComponent<Notification>().setDescription(description);
        yield return new WaitForSeconds(2.0f * (float)index);
        list.RemoveAt(0);
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