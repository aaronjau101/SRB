using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Placed in first run script
public static class Globals
{
    public static int score = 0;
    public static int lives = 3;
    public static int bananas = 0;

    public static void reset()
    {
        score = 0;
        lives = 0;
        bananas = 0;
    }
}

    public class mainMenu : MonoBehaviour
{
    public void NewGame()
    {
        SceneManager.LoadScene("level1");
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("menu");
    }
    public void Help()
    {
        SceneManager.LoadScene("help");
    }
}
