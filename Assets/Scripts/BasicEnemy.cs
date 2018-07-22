using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : EnemyEntity
{
	private void Start()
	{
		BaseStart();
		GameManager.instance.enemylimit--;
	}
	private void FixedUpdate()
	{
		BaseFixedUpdate();
	}

	private void OnDestroy()
	{
		GameManager.instance.enemylimit++;
	}
}
