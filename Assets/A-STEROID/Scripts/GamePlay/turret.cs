using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turret : MonoBehaviour
{
	public LaserBehaviour las;
	public float angleprecision = 10;
	public float nbshootfor10s = 10;
	public float nbofshotbyburst = 1;
	public float porte;
	public bool alwaysshoot;

	public float deregulatorvalue = 0;
	public float delay = 1;

	void Start ()
	{
		delay += Random.Range(-deregulatorvalue, deregulatorvalue);
	}
	
	void Update ()
	{
		if (delay > 0)
		{
			delay -= Time.deltaTime;
			return;
		}
		// if (alwaysshoot == false && cib)
		delay = 10 / nbshootfor10s + Random.Range(-deregulatorvalue, deregulatorvalue);
		for (int i = 0; i < nbofshotbyburst; i++)
		{
			LaserPool.instance.NewLaser(transform.position, transform.rotation * Quaternion.Euler(Vector3.forward * 90));
		}
	}
}
