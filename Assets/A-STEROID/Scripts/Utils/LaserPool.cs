using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPool : GameObjectPool
{
	public static LaserPool instance;

	protected Queue<Laser> laserPool = null;

	protected override void Awake()
	{
		base.Awake();
		laserPool = new Queue<Laser>();
		instance = this;
		AllocPool();
	}

	protected override void OnNewObjectReserved(GameObject go)
	{
		laserPool.Enqueue(go.GetComponent<Laser>());
	}

	public Laser NewLaser(Vector3 pos, Vector2 dir)
	{
		base.GetGameObject();

		Laser laser = laserPool.Dequeue();

		laser.transform.position = pos;
		laser.dir = dir;
		
		Benchmark.instance.maxSimultaneousLasers = allocatedObjectCount;
		
		laser.OnLaserSpawned();

		return laser;
	}

	public void FreeLaser(Laser o)
	{
		FreeGameObject(o.gameObject);
		laserPool.Enqueue(o);
	}
}
