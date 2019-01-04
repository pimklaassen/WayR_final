using System.Collections;
using UnityEngine;
using System;

// Data class for the tweets of the Twitter User Timelines.
public class UserTimelineTwitterData
{
	public string tweetText = "";
	public string tweetTime = "";
    public string tweetTopic = "";
	
    // Function to form a string containing all the data.
	public override string ToString(){
		return tweetTopic + "*"+"\"" + tweetText + "\". \n" + tweetTime;
	}
}