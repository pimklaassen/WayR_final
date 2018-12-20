using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;



public class Main : MonoBehaviour {
    public static List<string> tweetList = new List<string>();
    public static List<List<string>> alltweets = new List<List<string>>();

    public GameObject[] tweetButtons = new GameObject[5];

    private int nextUpdate = 60;

    // Use this for initialization
    void Start ()
    {
        TwitterAPI.instance.SearchTwitter("gislab", SearchTweetsResultsCallBack);
        TwitterAPI.instance.UserTimelineTwitter("tagesanzeiger", "extended", UserTimelineResultsCallBack);
        TwitterAPI.instance.UserTimelineTwitter("railinfo_sbb", "extended", UserTimelineResultsCallBack);
        TwitterAPI.instance.UserTimelineTwitter("reutersworld", "extended", UserTimelineResultsCallBack);
    }
	
	// Update is called once per frame
	void Update () {
        if (Time.time >= nextUpdate)
        {
            nextUpdate = Mathf.FloorToInt(Time.time) + 600;

            alltweets = new List<List<string>>();
            
            TwitterAPI.instance.SearchTwitter("gislab", SearchTweetsResultsCallBack);
            TwitterAPI.instance.UserTimelineTwitter("tagesanzeiger", "extended", UserTimelineResultsCallBack);
            TwitterAPI.instance.UserTimelineTwitter("railinfo_sbb", "extended", UserTimelineResultsCallBack);
            TwitterAPI.instance.UserTimelineTwitter("reutersworld", "extended", UserTimelineResultsCallBack);

        }
    }

    void UserTimelineResultsCallBack(List<UserTimelineTwitterData> timelineList)
    {
        List<string> tweetsaslist = new List<string>();
        string tweets = "";
        foreach (UserTimelineTwitterData twitterData in timelineList)
        {
            string toadd = twitterData.ToString();
            tweets = tweets + "\n\n" + toadd;
            tweetsaslist.Add(toadd);
        }

        tweetList.Add(tweets);
        alltweets.Add(tweetsaslist);
    }

    void SearchTweetsResultsCallBack(List<TweetSearchTwitterData> searchTweetList)
    {
        List<string> tweetsaslist = new List<string>();
        string tweets = "";
        foreach (TweetSearchTwitterData twitterData in searchTweetList)
        {
            string toadd = twitterData.ToString();
            tweets = tweets + "\n\n" + toadd;
            tweetsaslist.Add(toadd);
        }

        tweetList.Add(tweets);
        alltweets.Add(tweetsaslist);
    }


}


