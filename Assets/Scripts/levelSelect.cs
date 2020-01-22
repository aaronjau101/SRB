using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class levelSelect : MonoBehaviour
{
    public GameObject hsText, btText, bsText, level1, level2, level3;

    // Start is called before the first frame update
    void Start()
    {
        hsText.SetActive(false);
        btText.SetActive(false);
        bsText.SetActive(false);
        GameObject[] levelButtons = { level1, level2, level3 };
        for(var i = 1; i < Globals.levels.Length; i++)
        {
            LevelData L = Globals.levels[i - 1];
            if(L.score > 0)
            {
                L.blocked = false;
                GameObject panel = levelButtons[i].gameObject.transform.GetChild(2).gameObject;
                panel.SetActive(false);
                Button button = levelButtons[i].gameObject.GetComponent<Button>();
                button.interactable = true;
                if (i == 2)
                {
                    button.onClick.AddListener(() => { loadLevelData(2); });
                }
                else if(i == 1)
                {
                    button.onClick.AddListener(() => { loadLevelData(1); });
                }
                
            }
            else
            {
                L.blocked = true;
                levelButtons[i].gameObject.transform.GetChild(2).gameObject.SetActive(true);
                Button button = levelButtons[i].gameObject.GetComponent<Button>();
                button.interactable = false;
            }
        }
        

    }

    void loadLevelData(int level)
    {
        LevelData L = Globals.levels[level];
        hsText.GetComponent<Text>().text = "HIGHSCORE\n\n" + L.score.ToString();
        btText.GetComponent<Text>().text = "BEST TIME\n\n" + scoreboard(L.time);
        bsText.GetComponent<Text>().text = "BANANAS\n\n" + L.bananas.ToString();
        hsText.SetActive(true);
        btText.SetActive(true);
        bsText.SetActive(true);
        this.GetComponent<mainMenu>().currentLevel = level + 1;
    }

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
}
