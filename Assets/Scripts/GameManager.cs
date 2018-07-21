﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public int		gameState;
	public bool 	isBossFight;

	public static GameManager instance;

	private void Awake()
	{
		instance = this;
	}

	public void DefeatBoss()
	{
		gameState++;
		isBossFight = false;
	}

	public void EnterBossFight()
	{
		isBossFight = true;
	}
}