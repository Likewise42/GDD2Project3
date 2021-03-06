﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YetiGameData {

    private static uint coldCash = 0;

    private static uint highScore = 0;

    public static int coldCashMultiplier = 1;

    public enum YetiType { LankyYeti, NormalYeti, FemaleYeti };

    public enum BoardType { NormalBoard, ATATBoard, WampBoard, CashBoard, YetiBoard };

    public static bool[] boardBoughtArray = {true, false, false, false, false};
    public static bool[] yetiBoughtArray = {true, false, false};

    public static YetiType SelectedYeti = YetiType.NormalYeti;
    public static BoardType SelectedBoard = BoardType.NormalBoard;

    public static uint currentBoard = 0;
    public static uint currentYeti = 0;


    public static uint ColdCash
    {
        get{
            return coldCash;
        }
        set
        {
                coldCash = value;
        }
    }


    public static uint HighScore
    {
        get
        {
            return highScore;
        }
        set
        {
            highScore = value;
        }
    }

    public static uint CurrentBoard
    {
        get
        {
            return currentBoard;
        }
        set
        {
            currentBoard = value;
        }
    }
}
