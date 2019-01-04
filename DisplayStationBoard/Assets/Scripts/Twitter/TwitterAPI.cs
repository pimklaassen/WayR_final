/* This class is an adapted class of a source that is not findable anymore. 
 * It was part of a TwitterAPI demo for HoloLens.
 * (See also: Report_GISLab_dindc_pimk.pdf  p.12 and reference [9])
 */

using UnityEngine;

using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Collections;

using MiniJSON;

#if NETFX_CORE
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
#endif

public class TwitterAPI : MonoBehaviour {	
	public string oauthConsumerKey = "";
	public string oauthConsumerSecret = "";
	public string oauthToken = "";
	public string oauthTokenSecret = "";
	
	private string oauthNonce = "";
	private string oauthTimeStamp = "";
	
	public static TwitterAPI instance = null;
	
	// Use this for initialization
	void Awake () {
		if (instance == null) {
			instance = this;
		}
		else {
			Debug.LogError("More then one instance of TwitterAPI: " + this.transform.name);
		}
	}

	
    //===============================================================================================================
    // Start of part that is adapted by CedricDind
    //===============================================================================================================
    public void UserTimelineTwitter(string twitterUser, string tweet_mode, Action<List<UserTimelineTwitterData>> callback)
    {
        PrepareOAuthData();
        StartCoroutine(UserTimeline_Coroutine(twitterUser, tweet_mode, callback));
    }

    private IEnumerator UserTimeline_Coroutine(string twitterUser, string tweet_mode, Action<List<UserTimelineTwitterData>> callback)
    {
        // Fix up hashes to be webfriendly
        twitterUser = Uri.EscapeDataString(twitterUser);

        string twitterUrl = "https://api.twitter.com/1.1/statuses/user_timeline.json";

        SortedDictionary<string, string> twitterParamsDictionary = new SortedDictionary<string, string>
        {
            {"screen_name", twitterUser},
            {"tweet_mode", tweet_mode},
            {"count", "5"},
            {"result_type", "recent"},
        };

        WWW query = CreateTwitterAPIQuery(twitterUrl, twitterParamsDictionary);
        yield return query;

        callback(ParseResultsFromUserTimelineTwitter(query.text, twitterUser));
    }

    // Use of MINI JSON http://forum.unity3d.com/threads/35484-MiniJSON-script-for-parsing-JSON-data
    private List<UserTimelineTwitterData> ParseResultsFromUserTimelineTwitter(string jsonResults, string topic)
    {
        Debug.Log(jsonResults);

        List<UserTimelineTwitterData> twitterDataList = new List<UserTimelineTwitterData>();
        object jsonObject = Json.Deserialize(jsonResults);
        IList tweets = jsonObject as IList;
        foreach (IDictionary tweet in tweets)
        {
            UserTimelineTwitterData twitterData = new UserTimelineTwitterData();
            twitterData.tweetText = tweet["full_text"] as string;
            string tweetTime = tweet["created_at"] as string;
            twitterData.tweetTime = tweetTime.Substring(0, 20);
            twitterData.tweetTopic = topic;

            twitterDataList.Add(twitterData);
        }

        return twitterDataList;
    }

    //-----------------------------------------------------------------------
    // Tweets with hashtag 
    //-----------------------------------------------------------------------
    

    public void SearchTwitter(string keywords, Action<List<TweetSearchTwitterData>> callback)
    {
        PrepareOAuthData();
        StartCoroutine(SearchTwitter_Coroutine(keywords, callback));
    }

    private IEnumerator SearchTwitter_Coroutine(string keywords, Action<List<TweetSearchTwitterData>> callback)
    {
        // Fix up hashes to be webfriendly
        keywords = Uri.EscapeDataString(keywords);

        string twitterUrl = "https://api.twitter.com/1.1/search/tweets.json";

        SortedDictionary<string, string> twitterParamsDictionary = new SortedDictionary<string, string>
        {
            {"q", keywords},
            {"count", "5"},
            {"tweet_mode", "extended"},
            {"result_type", "recent"},
        };

        WWW query = CreateTwitterAPIQuery(twitterUrl, twitterParamsDictionary);
        yield return query;

        callback(ParseResultsFromSearchTwitter(query.text, keywords));
    }

    // Use of MINI JSON http://forum.unity3d.com/threads/35484-MiniJSON-script-for-parsing-JSON-data
    private List<TweetSearchTwitterData> ParseResultsFromSearchTwitter(string jsonResults, string topic)
    {
        Debug.Log(jsonResults);

        List<TweetSearchTwitterData> twitterDataList = new List<TweetSearchTwitterData>();
        object jsonObject = Json.Deserialize(jsonResults);
        IDictionary search = (IDictionary)jsonObject;
        IList tweets = (IList)search["statuses"];
        foreach (IDictionary tweet in tweets)
        {
            IDictionary userInfo = tweet["user"] as IDictionary;

            TweetSearchTwitterData twitterData = new TweetSearchTwitterData();
            twitterData.tweetUser = userInfo["screen_name"] as string;
            twitterData.tweetText = tweet["full_text"] as string;
            string tweetTime = tweet["created_at"] as string;
            twitterData.tweetTime = tweetTime.Substring(0, 20);
            twitterData.tweetTopic = topic;


            twitterDataList.Add(twitterData);
        }

        return twitterDataList;
    }
    
    //===============================================================================================================
    // End of part that is adapted by CedricDind
    //===============================================================================================================

    private WWW CreateTwitterAPIQuery(string twitterUrl, SortedDictionary<string, string> twitterParamsDictionary)
    {
        string signature = CreateSignature(twitterUrl, twitterParamsDictionary);
        Debug.Log("OAuth Signature: " + signature);

        string authHeaderParam = CreateAuthorizationHeaderParameter(signature, this.oauthTimeStamp);
        Debug.Log("Auth Header: " + authHeaderParam);

        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers["Authorization"] = authHeaderParam;

        string twitterParams = ParamDictionaryToString(twitterParamsDictionary);

        WWW query = new WWW(twitterUrl + "?" + twitterParams, null, headers);
        return query;
    }


    private void PrepareOAuthData()
    {
        oauthNonce = Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture)));
        TimeSpan _timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
        oauthTimeStamp = Convert.ToInt64(_timeSpan.TotalSeconds).ToString(CultureInfo.InvariantCulture);

        // Override the nounce and timestamp here if troubleshooting with Twitter's OAuth Tool
        //oauthNonce = "69db07d069ac50cd673f52ee08678596";
        //oauthTimeStamp = "1442419142";
    }

    // Taken from http://www.i-avington.com/Posts/Post/making-a-twitter-oauth-api-call-using-c
    private string CreateSignature(string url, SortedDictionary<string, string> searchParamsDictionary)
    {
        //string builder will be used to append all the key value pairs
        StringBuilder signatureBaseStringBuilder = new StringBuilder();
        signatureBaseStringBuilder.Append("GET&");
        signatureBaseStringBuilder.Append(Uri.EscapeDataString(url));
        signatureBaseStringBuilder.Append("&");

        //the key value pairs have to be sorted by encoded key
        SortedDictionary<string, string> urlParamsDictionary = new SortedDictionary<string, string>
                             {
                                 {"oauth_version", "1.0"},
                                 {"oauth_consumer_key", this.oauthConsumerKey},
                                 {"oauth_nonce", this.oauthNonce},
                                 {"oauth_signature_method", "HMAC-SHA1"},
                                 {"oauth_timestamp", this.oauthTimeStamp},
                                 {"oauth_token", this.oauthToken}
                             };

        foreach (KeyValuePair<string, string> keyValuePair in searchParamsDictionary)
        {
            urlParamsDictionary.Add(keyValuePair.Key, keyValuePair.Value);
        }

        signatureBaseStringBuilder.Append(Uri.EscapeDataString(ParamDictionaryToString(urlParamsDictionary)));
        string signatureBaseString = signatureBaseStringBuilder.ToString();

        Debug.Log("Signature Base String: " + signatureBaseString);

        //generation the signature key the hash will use
        string signatureKey =
            Uri.EscapeDataString(this.oauthConsumerSecret) + "&" +
            Uri.EscapeDataString(this.oauthTokenSecret);

#if NETFX_CORE
        var objMacProv = MacAlgorithmProvider.OpenAlgorithm(MacAlgorithmNames.HmacSha1);
        byte[] secretKey = Encoding.UTF8.GetBytes(signatureKey);
        var hash = objMacProv.CreateHash(secretKey.AsBuffer());
        hash.Append(CryptographicBuffer.ConvertStringToBinary(signatureBaseString, BinaryStringEncoding.Utf8));
        string signatureString = CryptographicBuffer.EncodeToBase64String(hash.GetValueAndReset());
       
#else
        HMACSHA1 hmacsha1 = new HMACSHA1(
            new ASCIIEncoding().GetBytes(signatureKey));

        //hash the values
        string signatureString = Convert.ToBase64String(
            hmacsha1.ComputeHash(
                new ASCIIEncoding().GetBytes(signatureBaseString)));
#endif      
        return signatureString;
    }

    private string CreateAuthorizationHeaderParameter(string signature, string timeStamp)
    {
        string authorizationHeaderParams = String.Empty;
        authorizationHeaderParams += "OAuth ";

        authorizationHeaderParams += "oauth_consumer_key="
                                     + "\"" + Uri.EscapeDataString(this.oauthConsumerKey) + "\", ";

        authorizationHeaderParams += "oauth_nonce=" + "\"" +
                                     Uri.EscapeDataString(this.oauthNonce) + "\", ";

        authorizationHeaderParams += "oauth_signature=" + "\""
                                     + Uri.EscapeDataString(signature) + "\", ";

        authorizationHeaderParams += "oauth_signature_method=" + "\"" +
            Uri.EscapeDataString("HMAC-SHA1") +
            "\", ";

        authorizationHeaderParams += "oauth_timestamp=" + "\"" +
                                     Uri.EscapeDataString(timeStamp) + "\", ";

        authorizationHeaderParams += "oauth_token=" + "\"" +
                                     Uri.EscapeDataString(this.oauthToken) + "\", ";

        authorizationHeaderParams += "oauth_version=" + "\"" +
                                     Uri.EscapeDataString("1.0") + "\"";
        return authorizationHeaderParams;
    }

    private string ParamDictionaryToString(IDictionary<string, string> paramsDictionary)
    {
        StringBuilder dictionaryStringBuilder = new StringBuilder();
        foreach (KeyValuePair<string, string> keyValuePair in paramsDictionary)
        {
            //append a = between the key and the value and a & after the value
            dictionaryStringBuilder.Append(string.Format("{0}={1}&", keyValuePair.Key, keyValuePair.Value));
        }

        // Get rid of the extra & at the end of the string
        string paramString = dictionaryStringBuilder.ToString().Substring(0, dictionaryStringBuilder.Length - 1);
        return paramString;
    }
    
}
