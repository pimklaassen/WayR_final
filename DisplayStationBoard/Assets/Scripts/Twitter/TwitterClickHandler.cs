using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TwitterClickHandler : MonoBehaviour
{
    public GameObject tweetPanel;
    public GameObject tweetPrefab;
    public GameObject[] tweetbuttonlist = new GameObject[4];

    public void Start()
    {
        int i = 0;
        foreach (Transform child in tweetPanel.transform)
        {
            if (i < tweetbuttonlist.Length)
            {
                tweetbuttonlist[i] = child.gameObject;
                tweetbuttonlist[i].SetActive(false);
                i++;
            }

        }
    }

    public void OnClick()
    {
        List<string> switchList = new List<string>();
        switch (EventSystem.current.currentSelectedGameObject.tag)
        {
            case "trump":
                foreach(List<string> tweets in Main.alltweets)
                {
                    string[] parts = tweets[1].Split('*');
                    if(parts[0] == "gislab")
                        updateButtons(tweets);
                }
                break;
            case "tagesanzeiger":
                foreach (List<string> tweets in Main.alltweets)
                {
                    string[] parts = tweets[1].Split('*');
                    if (parts[0] == "tagesanzeiger")
                        updateButtons(tweets);
                }
                break;
            case "sbb":
                foreach (List<string> tweets in Main.alltweets)
                {
                    string[] parts = tweets[1].Split('*');
                    if (parts[0] == "railinfo_sbb")
                        updateButtons(tweets);
                }
                break;
            case "reuters":
                foreach (List<string> tweets in Main.alltweets)
                {
                    string[] parts = tweets[1].Split('*');
                    if (parts[0] == "reutersworld")
                        updateButtons(tweets);
                }
                break;
        }
    }

    void updateButtons (List<string> alltweets)
    {
        for (int i = 0; i < tweetbuttonlist.Length; i++)
        {
            tweetbuttonlist[i].SetActive(false);
            if (i < alltweets.Count)
            {
                tweetbuttonlist[i].SetActive(true);
                string[] helper = alltweets[i].Split('*');
                string[] parts = helper[1].Split('"');

                Text text = tweetbuttonlist[i].transform.GetChild(0).GetComponent<Text>();
                Text textclone = tweetbuttonlist[i].transform.GetChild(1).GetComponent<Text>(); ;

                tweetbuttonlist[i].transform.GetChild(0).GetComponent<Text>().text = parts[0] + parts[1];
                tweetbuttonlist[i].transform.GetChild(1).GetComponent<Text>().text = parts[2].Substring(1).Replace('\n', ' ');
            }
        }
    }

}
