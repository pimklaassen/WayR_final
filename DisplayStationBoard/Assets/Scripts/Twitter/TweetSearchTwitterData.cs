using System.Collections;
using UnityEngine;
using System;

public class TweetSearchTwitterData {
	public string tweetText = "";
    public string tweetTime = "";
    public string tweetUser = "";


    public override string ToString(){
        return "User " + tweetUser + " tweeted: " + "\"" + tweetText + "\". \n" + tweetTime;
    }
}