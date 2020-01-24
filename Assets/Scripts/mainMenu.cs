using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Global Variables
public static class Globals
{
    public static int currentLevel = -1;

    public static LevelData[] levels = 
    {
        new LevelData(16, 30, 10, 5, 3, 30000, 60000, 80000),
        new LevelData(16, 60, 30, 15, 10, 30000, 60000, 80000),
        new LevelData(16, 90, 15, 30, 60, 30000, 70000, 100000)
    };

    public static Trophy[] trophies =
    {
        new Trophy("Level 1 Completed"),
        new Trophy("Level 2 Completed"),
        new Trophy("Level 3 Completed"),
        new Trophy("Level 1 Easy Time"),
        new Trophy("Level 1 Medium Time"),
        new Trophy("Level 1 Hard Time"),
        new Trophy("Level 2 Easy Time"),
        new Trophy("Level 2 Medium Time"),
        new Trophy("Level 2 Hard Time"),
        new Trophy("Level 3 Easy Time"),
        new Trophy("Level 3 Medium Time"),
        new Trophy("Level 3 Hard Time"),
        new Trophy("Level 1 Easy Score"),
        new Trophy("Level 1 Medium Score"),
        new Trophy("Level 1 Hard Score"),
        new Trophy("Level 2 Easy Score"),
        new Trophy("Level 2 Medium Score"),
        new Trophy("Level 2 Hard Score"),
        new Trophy("Level 3 Easy Score"),
        new Trophy("Level 3 Medium Score"),
        new Trophy("Level 3 Hard Score"),
        new Trophy("Level 1 All Bananas"),
        new Trophy("Level 2 All Bananas"),
        new Trophy("Level 3 All Bananas")
    };
}

//Class to track trophy description and whether it was unlocked
public class Trophy
{
    public string description;
    public bool unlocked = false;

    public Trophy(string d)
    {
        description = d;
    }
}

//Class to hold level information
public class LevelData
{
    //Variables to track user's best stats
    public int score = 0;
    public float time = Mathf.Infinity;
    public int bananas = 0;

    //Check if the level is blocked in level select
    public bool blocked = false;
    public bool completed = false;

    //Stats for setting up level
    public int maxBananas;
    public float maxTime;

    //Stats for trophy unlock
    public float easyTime;
    public float mediumTime;
    public float hardTime;

    public float easyScore;
    public float mediumScore;
    public float hardScore;

    public LevelData(int _maxBananas, float _maxTime, float _easyTime, float _mediumTime, float _hardTime,  float _easyScore, float _mediumScore, float _hardScore)
    {
        maxBananas = _maxBananas;
        maxTime = _maxTime;
        easyTime = _easyTime;
        mediumTime = _mediumTime;
        hardTime = _hardTime;
        easyScore = _easyScore;
        mediumScore = _mediumScore;
        hardScore = _hardScore;
    }

    //Functions to check and update new best score, time, bananas
    public bool newHighscore(int newScore)
    {
        if(newScore > score)
        {
            score = newScore;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool newBestTime(float newTime)
    {
        if (newTime < time)
        {
            time = newTime;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool newBanana(int newBananas)
    {
        if (newBananas > bananas)
        {
            bananas = newBananas;
            return true;
        }
        else
        {
            return false;
        }
    }
}

//Functions to load scenes
public class mainMenu : MonoBehaviour
{
    public void Help()
    {
        SceneManager.LoadScene("help");
    }
    public void LevelSelect()
    {
        SceneManager.LoadScene("levelSelect");
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("menu");
    }
    public void Credits()
    {
        SceneManager.LoadScene("credits");
    }

}
