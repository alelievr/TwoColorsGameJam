using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleshScrit : MonoBehaviour {

	public Transform nextBossZone;	
	// Update is called once per frame
	void Update () {
		 transform.up = nextBossZone.position - transform.position;
	}
}
