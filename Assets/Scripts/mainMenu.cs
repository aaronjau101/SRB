using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Placed in first run script
public static class Globals
{
    public static LevelData[] levels = { new LevelData(), new LevelData(), new LevelData()};
}

public class LevelData
{
    public int score = 0;
    public float time = Mathf.Infinity;
    public int bananas = 0;
    public bool blocked = false;

    public LevelData()
    {

    }

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

public class mainMenu : MonoBehaviour
{
    public int currentLevel = 0;

    public void LoadLevel()
    {
        if (currentLevel > 0 && currentLevel < 4)
        {
            SceneManager.LoadScene("level" + currentLevel.ToString());
        }
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("menu");
    }
    public void Help()
    {
        SceneManager.LoadScene("help");
    }
    public void LevelSelect()
    {
        SceneManager.LoadScene("levelSelect");
    }
}
