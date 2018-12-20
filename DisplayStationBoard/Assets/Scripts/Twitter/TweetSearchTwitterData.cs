using System.Collections;
using UnityEngine;
using System;

public class TweetSearchTwitterData {
	public string tweetText = "";
    public string tweetTime = "";
    public string tweetUser = "";
    public string tweetTopic = "";


    public override string ToString(){
        return tweetTopic + "*" + "User " + tweetUser + " tweeted: " + "\"" + tweetText + "\". \n" + tweetTime;
    }
}