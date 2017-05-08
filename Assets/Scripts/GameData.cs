using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YetiGameData {

    private static uint coldCash = 0;

    private static uint highScore = 0;

    public static int coldCashMultiplier = 1;

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
}
