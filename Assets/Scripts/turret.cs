using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turret : MonoBehaviour {

	public Laser las;
	public float angleprecision = 10;
	public float nbshootfor10secorburst = 10;
	public float porte;
	public bool alwaysshoot;

	public float deregulatorvalue = 0;
	public float delay = 1;

	// Use this for initialization
	void Start () {
		delay += Random.Range(-deregulatorvalue, deregulatorvalue);
	}
	
	
	// Update is called once per frame
	void Update () {
		if (delay > 0)
		{
			delay -= Time.deltaTime;
			return;
		}
		delay = nbshootfor10secorburst / 10 + Random.Range(-deregulatorvalue, deregulatorvalue);
		Laser lastmp = GameObject.Instantiate(las, transform.position, Quaternion.identity);
		var rotation = transform.rotation;
		transform.Rotate(transform.forward, Random.Range(-angleprecision / 2, angleprecision / 2));
		lastmp.dir = transform.up;
		transform.rotation = rotation;
	}
}
