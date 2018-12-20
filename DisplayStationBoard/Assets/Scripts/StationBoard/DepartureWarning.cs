using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepartureWarning : MonoBehaviour {

   
    public GameObject warningText;
    public static bool checkPinned;
    public static string platform;
    public static string destination;
    public static DateTime departureTime;
    private TimeSpan warningMinutes;
    private int oldMinute;
    private int checkSeconds;

	// Use this for initialization
	void Start () {

        warningText.SetActive(false);
        checkPinned = false;
        checkSeconds = 0;
        oldMinute = 5;

        // calling warningFunction every 30 seconds
        InvokeRepeating("warningFunction", 0, 1f);
    }
	
    // function to display a warning if departure of train is close
	public void warningFunction () {

        if (checkPinned)
        {
            warningMinutes = departureTime.Subtract(DateTime.Now);
            int countMinutes = warningMinutes.Days * 24 * 60 + warningMinutes.Hours * 60 + warningMinutes.Minutes +1;
            
            if (countMinutes < 0)
            {
                if (warningText.activeSelf == true)
                    warningText.SetActive(false);
                checkPinned = false;
            }
            else if (countMinutes <= 1)
                warningText.SetActive(true);
            else if (countMinutes <= 5)
            {
                if (checkSeconds <= 10)
                    warningText.SetActive(true);
                else
                    warningText.SetActive(false);

                if(oldMinute == countMinutes)
                    checkSeconds++;
                else
                {
                    checkSeconds = 0;
                    oldMinute = countMinutes;
                }
            }
            else
            {
                if (warningText.activeSelf == true)
                    warningText.SetActive(false);
            }

            warningText.transform.GetComponent<TextMesh>().text = "Warning! Train to " + destination + " leaving in " + countMinutes + " Minutes.\nGo to platform " + platform + ".";   
        }
        else
        {
            if (warningText.activeSelf == true)
                warningText.SetActive(false);
        }
	}
}
