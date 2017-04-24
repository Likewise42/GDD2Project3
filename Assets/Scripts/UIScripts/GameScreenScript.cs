﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScreenScript : MonoBehaviour {

    public GameObject MainPanel;
    public GameObject PausePanel;

    public Slider DistanceSlider;
    public Text Coldcash;
    public Text Score;


    #region testVars

    private int coldCash = 0;
    private int score = 0;
    private float distance = 0.0f;


    #endregion

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (MainPanel.activeInHierarchy)
            {
                MainPanel.SetActive(false);
                PausePanel.SetActive(true);
            }
            else
            {
                MainPanel.SetActive(true);
                PausePanel.SetActive(false);
            }
        }

        #region TestCode

        if (Input.GetKey(KeyCode.C))
        {
            coldCash += 1;
            SetColdCash(coldCash);
        }
        if (Input.GetKey(KeyCode.V))
        {
            score += 10;
            SetScore(score);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            distance -= .01f;
            if(distance < 0)
            {
                distance = 0;
            }
            SetDistancePercent(distance);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            distance += .01f;
            if (distance > 1)
            {
                distance = 1;
            }
            SetDistancePercent(distance);
        }



        #endregion
    }


    public void SetScore(int score)
    {
        Score.text = "Score: " + score;
    }

    public void SetColdCash(int coldCash)
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
