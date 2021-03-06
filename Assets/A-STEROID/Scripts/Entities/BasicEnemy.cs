﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : EnemyEntity
{
	public float destroyRange = 200;

	private void Start()
	{
		BaseStart();
		GameManager.instance.enemylimit--;
	}
	private void FixedUpdate()
	{
		BaseFixedUpdate();
	}

	private void Update()
	{
		if (GameManager.instance.player != null)
			if (Vector3.Distance(transform.position, GameManager.instance.playerTransform.position) > destroyRange)
				Destroy(gameObject);
	}

	private void OnDestroy()
	{
		GameManager.instance.enemylimit++;
	}
}
