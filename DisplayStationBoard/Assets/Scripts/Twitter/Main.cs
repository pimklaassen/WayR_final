/* This class is an adapted class of a source that is not findable anymore. 
 * It was part of a TwitterAPI demo for HoloLens.
 * (See also: Report_GISLab_dindc_pimk.pdf  p.12 and reference [9])
 */
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;


/* Main class of the Twitter part.
 * Instanciates the Twitter API and saves the tweets in Lists  */
public class Main : MonoBehaviour {

    // static lists that contain all the tweets to be accessed by the TwitterClickHandler script
    public static List<string> tweetList = new List<string>();
    public static List<List<string>> alltweets = new List<List<string>>();

    public GameObject[] tweetButtons = new GameObject[5];

    // seconds until next update
    private int nextUpdate = 600;

    // Starting TwitterAPI instances for the different topics
    void Start ()
    {
        TwitterAPI.instance.SearchTwitter("gislab", SearchTweetsResultsCallBack);
        TwitterAPI.instance.UserTimelineTwitter("tagesanzeiger", "extended", UserTimelineResultsCallBack);
        TwitterAPI.instance.UserTimelineTwitter("railinfo_sbb", "extended", UserTimelineResultsCallBack);
        TwitterAPI.instance.UserTimelineTwitter("reutersworld", "extended", UserTimelineResultsCallBack);
    }
	
	// Updating TwitterAPI instances every 600 seconds
	void Update () {
        if (Time.time >= nextUpdate)
        {
            nextUpdate = Mathf.FloorToInt(Time.time) + 600;

            //clearing the alltweets list
            alltweets = new List<List<string>>();
            
            TwitterAPI.instance.SearchTwitter("gislab", SearchTweetsResultsCallBack);
            TwitterAPI.instance.UserTimelineTwitter("tagesanzeiger", "extended", UserTimelineResultsCallBack);
            TwitterAPI.instance.UserTimelineTwitter("railinfo_sbb", "extended", UserTimelineResultsCallBack);
            TwitterAPI.instance.UserTimelineTwitter("reutersworld", "extended", UserTimelineResultsCallBack);

        }
    }

    // callback for the User Timeline search
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

    // callback for the Tweet Search
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


