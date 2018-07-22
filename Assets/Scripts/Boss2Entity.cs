using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Entity : EnemyEntity
{

	float timetmp2 = 0;
	
	void FixedUpdate () {
		if (cible == null)
			return ;
			
		BaseFixedUpdate();

		timetmp2 += Time.deltaTime;
		if (timetmp2 < 6)
		{
			idealdistancetocible = 100;
			speed = 30;
			maxspeed = 20;
		}
		else if (timetmp2 < 8)
		{
			idealdistancetocible = 0;
			speed = 60;
			maxspeed = 40;
		}
		else if (timetmp2 < 10)
		{
			idealdistancetocible = 100;
			speed = 10;
			maxspeed = 5;
		}
		else
			timetmp2 = 0;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Proj"/* || other.tag == "debrit"*/)
			Destroy(other.gameObject);
	}
}
