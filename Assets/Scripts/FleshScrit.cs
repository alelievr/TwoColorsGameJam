using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleshScrit : MonoBehaviour {

	public Transform nextBossZone;	
	public Transform playerTraansform;
	// Update is called once per frame

	void Update () {
		  transform.up = nextBossZone.position - playerTraansform.position;
	}
}
