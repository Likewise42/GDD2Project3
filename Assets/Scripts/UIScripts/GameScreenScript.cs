using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScreenScript : MonoBehaviour {

    public GameObject MainPanel;
    public GameObject PausePanel;
    public GameObject EndPanel;

    public GameObject[] ThingsToMakeGoAway;

    public Slider DistanceSlider;
    public Text Coldcash;
    public Text Score;

    public Text TimeText;
    public Text ScoreText;
    public Text ColdCashCollectedText;
    public Text HighScore;

    public float highScoreFlash;

    private bool end = false;
    private bool endedWithNewHighScore = false;
    private float endTimer = 0.0f;
    
    

    public void Update()
    {
        if (!end)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (MainPanel.activeInHierarchy)
                {
                    SetPaused(true);
                }
                else
                {
                    SetPaused(false);
                }
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                EndGameUI(400, 10000, 29, true);
            }

            SetColdCash(YetiGameData.ColdCash);
        }
        else
        {
            if (endedWithNewHighScore)
            {
                endTimer += Time.deltaTime;
                if(endTimer > highScoreFlash)
                {
                    endTimer -= highScoreFlash;
                    HighScore.gameObject.SetActive(!HighScore.IsActive());
                }
            }

        }
    }

    public void EndGameUI(int timeInseconds, int totalScore, int coldCashCollected, bool newHighScore)
    {
        end = true;
        MainPanel.SetActive(false);
        PausePanel.SetActive(false);
        foreach (GameObject go in ThingsToMakeGoAway)
        {
            go.SetActive(false);
        }

        EndPanel.SetActive(true);

        int minutes = (timeInseconds / 60);
        int seconds = timeInseconds % 60;

        TimeText.text = "Total Time: " + minutes + ":" + seconds;
        ScoreText.text = "Total Score: " + totalScore;
        ColdCashCollectedText.text = "Cold Cash Collected: " + coldCashCollected;

        endedWithNewHighScore = newHighScore;
    }

    public void SetPaused(bool isPaused)
    {

        if (isPaused)
        {
            MainPanel.SetActive(false);
            PausePanel.SetActive(true);
            foreach (GameObject go in ThingsToMakeGoAway)
            {
                go.SetActive(false);
            }
        }
        else
        {
            MainPanel.SetActive(true);
            PausePanel.SetActive(false);
            foreach (GameObject go in ThingsToMakeGoAway)
            {
                go.SetActive(true);
            }
        }
    }


    public void SetScore(uint score)
    {
        Score.text = "Score: " + score;
    }

    public void SetColdCash(uint coldCash)
    {
        Coldcash.text = "Cold Cash: " + coldCash;
    }

    public void SetDistancePercent(float distance)
    {

        if(distance > 1)
        {
            distance = 1;
        }
        else if(distance < 0)
        {
            distance = 0;
        }

        DistanceSlider.value = distance;
    }

}
