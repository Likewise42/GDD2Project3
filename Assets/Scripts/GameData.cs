using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YetiGameData {

    private static uint coldCash = 0;

    public static int coldCashMultiplier = 1;

    public static uint ColdCash
    {
        get{
            return coldCash;
        }
        set
        {
            if(value < 0)
                coldCash = 0;
            else
                coldCash = value;
        }
    }
}
