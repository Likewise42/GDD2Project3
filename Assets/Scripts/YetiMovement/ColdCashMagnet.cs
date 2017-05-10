using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColdCashMagnet : MonoBehaviour
{

    private GameObject coldCash;

    private List<GameObject> CCList = new List<GameObject>();

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        // every frame, check if there are null values in the CCList

        // if there is cold cash in our CCList
        if (CCList.Count > 0)
        {
            // for all cold cash objects in our CCList
            foreach (GameObject coldCash in CCList)
            {
                if(coldCash != null)
                {

                    // if there is cold cash, we want to adjust its 'x' position to be the same as the player's 'x' postion
                    if (coldCash.transform.position.x > transform.position.x)
                    {
                        Vector3 tempColdCashVec = coldCash.transform.position;
                        tempColdCashVec.x--;
                        coldCash.transform.position = tempColdCashVec;
                    }
                    else if (coldCash.transform.position.x < transform.position.x)
                    {
                        Vector3 tempColdCashVec = coldCash.transform.position;
                        tempColdCashVec.x++;
                        coldCash.transform.position = tempColdCashVec;
                    }
                }
                else // remove the cold cash
                {
                    CCList.Remove(coldCash);
                }

            }
        }
        
        

        // Here, we should check if the player has touched the cold cash. If so, the cold cash object here will be removed
        // Otherwise, all Cold Cash received by the player will effectively populate the CCList for no reason.
    }
		

    void OnTriggerEnter(Collider other)
    {
        GameObject otherObj = other.gameObject;
        if (otherObj.tag == "ColdCash")
        {
            CCList.Add(otherObj);
        }
    }

    void OnTriggerExit(Collider other)
    {
        GameObject otherObj = other.gameObject;
        if(otherObj.tag == "ColdCash")
        {
            CCList.Remove(otherObj);
        }
    }

}
