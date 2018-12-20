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
            switch (EventSystem.current.currentSelectedGameObject.tag)
            {
                case "trump":
                    updateButtons(Main.alltweets[0]);
                    break;
                case "tagesanzeiger":
                    updateButtons(Main.alltweets[1]);
                    break;
                case "sbb":
                    updateButtons(Main.alltweets[2]);
                    break;
                case "reuters":
                    updateButtons(Main.alltweets[3]);
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
                string[] parts = alltweets[i].Split('"');

                Text text = tweetbuttonlist[i].transform.GetChild(0).GetComponent<Text>();
                Text textclone = tweetbuttonlist[i].transform.GetChild(1).GetComponent<Text>(); ;

                tweetbuttonlist[i].transform.GetChild(0).GetComponent<Text>().text = parts[0] + parts[1];
                tweetbuttonlist[i].transform.GetChild(1).GetComponent<Text>().text = parts[2].Substring(1).Replace('\n', ' ');
            }
        }
    }

}
