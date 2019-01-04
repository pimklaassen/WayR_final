using System.Collections;
using UnityEngine;
using System;

// data class for the tweet search. 
public class TweetSearchTwitterData {
	public string tweetText = "";
    public string tweetTime = "";
    public string tweetUser = "";
    public string tweetTopic = "";

    // Function to form one string with all the data included.
    public override string ToString(){
        return tweetTopic + "*" + "User " + tweetUser + " tweeted: " + "\"" + tweetText + "\". \n" + tweetTime;
    }
}