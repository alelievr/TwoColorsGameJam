using UnityEngine;
using System.Collections;
 
public class ExplosionAutoManager : MonoBehaviour 
{
	private ParticleSystem ps;
 
 
	public void Start() 
	{
		ps = GetComponent<ParticleSystem>();
	}

	private void OnEnable()
	{
		ps.Play();
	}
	
	public void Update() 
	{
		if(ps)
		{
			if(!ps.IsAlive())
			{
				ExplosionPool.instance.FreeExplosion(ps);
			}
		}
	}
 }