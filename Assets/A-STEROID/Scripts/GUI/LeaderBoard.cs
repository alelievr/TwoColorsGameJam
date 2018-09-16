using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoard : MonoBehaviour
{
	public InputField	username;

	GameManager			gameManager;

	string privateCode = "tBcJoWI-0USUZw_CuQKgPQlfUyON_K4k-EMiRnG7S9uw";

	void Start()
	{
		gameManager = FindObjectOfType< GameManager >();
	}
	
	string dreamloWebserviceURL = "http://dreamlo.com/lb/";

	public void AddEntry()
	{
		AddEntryLong(username.text, gameManager.pts);
	}

	public void AddEntryLong(string pseudo, float points)
	{
		StartCoroutine(AddScoreWithPipe(pseudo, points));
	}
	
	IEnumerator AddScoreWithPipe(string playerName, float totalScore)
	{
		playerName = Clean(playerName);
		
		WWW www = new WWW(dreamloWebserviceURL + privateCode+ "/add-pipe/" + WWW.EscapeURL(playerName) + "/" + totalScore.ToString());
		yield return www;
		// highScores = www.text;
	}

	string Clean(string s)
	{
		s = s.Replace("/", "");
		s = s.Replace("|", "");
		return s;
		
	}
}
