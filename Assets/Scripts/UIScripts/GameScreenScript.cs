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
    public Text AirScore;

    public float highScoreFlash;

    private bool end = false;
    private bool endedWithNewHighScore = false;
    private float endTimer = 0.0f;

    public Color flashColor = new Color(0.0f, 0.0f, 1.0f);


    public Color airStartColor;
    public Color airEndColor;

    public float airScoreMinSize;
    public float airScoreMaxSize;

    

    private Color ogColor;
    private float colorFlashCounter;

    private Vector3 lColorS;
    private Vector3 lColorE;

    private uint prevColdCash;
    

    public int colorChangeFrames = 100;

    public void Start()
    {
        prevColdCash = YetiGameData.ColdCash;
        colorFlashCounter = colorChangeFrames;

        ogColor = Coldcash.color;

        lColorS.x = flashColor.r;
        lColorS.y = flashColor.g;
        lColorS.z = flashColor.b;

        lColorE.x = ogColor.r;
        lColorE.y = ogColor.g;
        lColorE.z = ogColor.b;
    }

    public void setAirScore(uint airScore, float percentToMax)
    {


        float size = Mathf.Lerp(airScoreMinSize, airScoreMaxSize, percentToMax);
        Color color = Color.Lerp(airStartColor, airEndColor, percentToMax);

        AirScore.text = "" + airScore;
        AirScore.color = color;
        AirScore.fontSize = (int)size;
    }

    public void hideAirScore()
    {
        AirScore.text = "";
    }

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

            if(prevColdCash < YetiGameData.ColdCash)
            {
                colorFlashCounter = 0;
                Coldcash.color = flashColor;
                prevColdCash = YetiGameData.ColdCash;
            }

            if(colorFlashCounter < colorChangeFrames)
            {
                Coldcash.color = Color.Lerp(flashColor, ogColor,colorFlashCounter/colorChangeFrames);
                colorFlashCounter++;
            }
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

    public void EndGameUI(int timeInseconds, uint totalScore, uint coldCashCollected, bool newHighScore)
    {
        end = true;
        MainPanel.SetActive(false);
        PausePanel.SetActive(false);
        MakeGoAway(false);

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
            MakeGoAway(false);
        }
        else
        {
            MainPanel.SetActive(true);
            PausePanel.SetActive(false);
            MakeGoAway(true);
        }
    }

    private void MakeGoAway(bool active)
    {
        foreach (GameObject go in ThingsToMakeGoAway)
        {
            go.SetActive(active);
        }
    }


    public void SetScore(uint score, float multiplier = 1.0f)
    {
        Score.text = "Score: " + score;

        if(multiplier > 1.0f)
        {
            float size = Score.fontSize + 2 * multiplier;
            Score.text += "\n<size=" + size + ">X" + multiplier + "</size>";
        }
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
