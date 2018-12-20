using System.Collections;
using UnityEngine;
using System;

public class UserTimelineTwitterData
{
	public string tweetText = "";
	public string tweetTime = "";
    public string tweetTopic = "";
	
	public override string ToString(){
		return tweetTopic + "*"+"\"" + tweetText + "\". \n" + tweetTime;
	}
}