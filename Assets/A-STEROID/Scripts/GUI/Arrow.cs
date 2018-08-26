using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

	public Transform nextBossZone;	
	public Transform playerTraansform;

	void Update ()
	{
		  transform.up = nextBossZone.position - playerTraansform.position;
	}
}
