﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class yetiShop : MonoBehaviour {

    public Text coldCashText;

    public Text nameText;
    public Text costText;
    public Text descText;

    public Text purchaseButtonTextYeti;
    public Text purchaseButtonTextBoard;

    public Button purchaseYetiButton;
    public Button purchaseBoardButton;

    public GameObject yetiViewPanel;
    public GameObject viewsPanel;
    public GameObject exitPanel;

    public Text boardNameText;
    public Text boardCostText;
    public Text boardDescText;

    public GameObject boardViewPanel;

    public void Update()
    {
        coldCashText.text = "Cold Cash: " + YetiGameData.ColdCash;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (exitPanel.activeInHierarchy)
            {
                exitPanel.SetActive(false);
                viewsPanel.SetActive(true);
            }
            else
            {
                exitPanel.SetActive(true);
                viewsPanel.SetActive(false);
            }
        }
    }

    public void setYetiView(string name, int cost, string description,bool purchased = false)
    {
        nameText.text = name;
        costText.text = "$" + cost;
        purchaseButtonTextYeti.text = "Buy";
        purchaseYetiButton.enabled = true;

        if (purchased)
        {
            costText.text = " Owned";
            purchaseButtonTextYeti.text = "Equip";
        }
        else if(cost > YetiGameData.ColdCash)
        {
            purchaseYetiButton.enabled = false;
            costText.text = "<color=red>$" + cost + "</color>";
        }
        descText.text = description;
        yetiViewPanel.SetActive(true);
    }

    public void setBoardView(string name, int cost, string description, bool purchased = false)
    {
        boardNameText.text = name;
        boardCostText.text = "$" + cost;
        purchaseButtonTextBoard.text = "Buy";
        purchaseBoardButton.enabled = true;

        if (purchased)
        {
            boardCostText.text = " Owned";
            purchaseButtonTextBoard.text = "Equip";
        }
        else if (cost > YetiGameData.ColdCash)
        {
            purchaseBoardButton.enabled = false;
            boardCostText.text = "<color=red>$" + cost + "</color>";
        }

        boardDescText.text = description;
        boardViewPanel.SetActive(true);
    }

    public void setAllViewsInactive()
    {
        yetiViewPanel.SetActive(false);
        boardViewPanel.SetActive(false);
    }


    public void toggleExitScreen(bool pause)
    {
        viewsPanel.SetActive(!pause);
        exitPanel.SetActive(pause);
    }



}
