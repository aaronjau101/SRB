using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class levelSelect : MonoBehaviour
{
    public GameObject hsText, btText, bsText, recordsText, playButton;
    public GameObject[] levelButtons;
    public GameObject loadingSlider, loadingName, loadingUI;

    void Start()
    {
        Globals.currentLevel = -1;

        //Hide stats
        hsText.SetActive(false);
        btText.SetActive(false);
        bsText.SetActive(false);
        recordsText.SetActive(false);
        playButton.GetComponent<Button>().interactable = false;

        //Iterate each level button
        for (var i = 1; i < Globals.levels.Length || i < levelButtons.Length; i++)
        {
            LevelData L = Globals.levels[i - 1];
            if(L.completed)
            {
                //Make level buttons active if previous was completed
                L.blocked = false;
                GameObject panel = levelButtons[i].gameObject.transform.GetChild(2).gameObject;
                panel.SetActive(false);
                Button button = levelButtons[i].gameObject.GetComponent<Button>();
                button.interactable = true;
            }
            else
            {
                //Make level buttons inactive if previous was not completed
                L.blocked = true;
                GameObject panel = levelButtons[i].gameObject.transform.GetChild(2).gameObject;
                panel.SetActive(true);
                Button button = levelButtons[i].gameObject.GetComponent<Button>();
                button.interactable = false;
            }
        }
        

    }

    //Function will set current level and display info
    void loadLevelData(int level)
    {
        //Sets the current level
        Globals.currentLevel = level;

        //Loads and displays a level's highscores, best time, and best bananas
        LevelData L = Globals.levels[level];
        hsText.GetComponent<Text>().text = "HIGHSCORE\n\n" + L.score.ToString();
        btText.GetComponent<Text>().text = "BEST TIME\n\n" + scoreboard(L.time);
        bsText.GetComponent<Text>().text = "BANANAS\n\n" + L.bananas.ToString();
        recordsText.GetComponent<Text>().text = "LEVEL " + (level + 1).ToString() + " RECORDS";
        hsText.SetActive(true);
        btText.SetActive(true);
        bsText.SetActive(true);
        recordsText.SetActive(true);
        playButton.GetComponent<Button>().interactable = true;
    }

    //Functions for the level button's OnClick event
    public void Level1()
    {
        loadLevelData(0);
    }

    public void Level2()
    {
        loadLevelData(1);
    }

    public void Level3()
    {
        loadLevelData(2);
    }

    //Function to convert time float into readable string
    string scoreboard(float amount)
    {
        if (amount >= 100 || amount <= 0)
        {
            return "00:00";
        }

        string result = Mathf.Floor((amount * 100)).ToString();
        while (result.Length < 4)
        {
            result = result.Insert(0, "0");
        }
        result = result.Insert(2, ":");
        return result;
    }

    //Functions to Load Levels and Menu
    public void LoadLevel()
    {
        int level = Globals.currentLevel + 1;
        string levelName = "level" + level.ToString();
        loadingName.GetComponent<Text>().text = "LEVEL " + level.ToString();
        loadingUI.SetActive(true);
        if (level > 0 && level <= Globals.levels.Length)
        {
            StartCoroutine(LoadAsynchronously(levelName));
        }
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("menu");
    }

    IEnumerator LoadAsynchronously(string levelName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelName);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            loadingSlider.GetComponent<Slider>().value = progress;

            yield return null;
        }
    }
}
