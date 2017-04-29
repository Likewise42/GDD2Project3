using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class yetiShop : MonoBehaviour {

    public Text coldCashText;

    public Text nameText;
    public Text costText;
    public Text descText;

    public GameObject yetiViewPanel;

    public void Update()
    {
        coldCashText.text = "Cold Cash: " + YetiGameData.ColdCash;
    }

    public void setYetiView(string name, int cost, string description)
    {
        nameText.text = name;
        costText.text = "$" + cost;
        descText.text = description;
        yetiViewPanel.SetActive(true);
    }

    public void setAllViewsInactive()
    {
        yetiViewPanel.SetActive(false);
    }




}
