using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LeaderBoardDisplay : MonoBehaviour
{
	string dreamloWebserviceURL = "http://dreamlo.com/lb/";

	string leaderboardCode = "5b9e930ecb934a0e10a89a20";

	public List< LeaderBoardCell >	leaderboardCells = new List< LeaderBoardCell >();

	string highScores;

	void OnEnable()
	{
		LoadLeaderboard();
	}

	public void LoadLeaderboard()
	{
		StartCoroutine(GetScores());
	}
	
	public IEnumerator GetScores()
	{
		highScores = "";
		WWW www = new WWW(dreamloWebserviceURL +  leaderboardCode  + "/pipe");
		yield return www;
		highScores = www.text;

		Debug.Log("highScores: " + highScores);

		string[] rows = highScores.Split(new char[] {'\n'}, System.StringSplitOptions.RemoveEmptyEntries);

		for (int i = 0; i < rows.Length && i < leaderboardCells.Count; i++)
		{
			string[] values = rows[i].Split(new char[] {'|'}, System.StringSplitOptions.None);

			Debug.Log("name: " + values[0]);

			LeaderBoardCell cell = leaderboardCells[i];
			
			cell.UpdateProperties(long.Parse(values[1]), 10000000000, values[0]);

		}
		
		// loadingBar.SetActive(false);

	}
	
}