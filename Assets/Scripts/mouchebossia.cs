using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouchebossia : EnemyEntity
{

	void Update () {

	}

	void FixedUpdate () {
		BaseFixedUpdate();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Proj" || other.tag == "debrit")
			Destroy(other.gameObject);
	}
}
