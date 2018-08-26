using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPool : GameObjectPool
{
	public static LaserPool instance;

	protected Queue<LaserBehaviour> laserPool = null;

	protected override void Awake()
	{
		base.Awake();
		laserPool = new Queue<LaserBehaviour>();
		instance = this;
		AllocPool();
	}

	protected override void OnNewObjectReserved(GameObject go)
	{
		laserPool.Enqueue(go.GetComponent<LaserBehaviour>());
	}

	public LaserBehaviour NewLaser(Vector3 pos, Quaternion rotation)
	{
		base.GetGameObject();

		LaserBehaviour laser = laserPool.Dequeue();

		laser.transform.position = pos;
		laser.transform.rotation = rotation;
		
		Benchmark.instance.maxSimultaneousLasers = allocatedObjectCount;
		
		laser.OnLaserSpawned();

		return laser;
	}

	public void FreeLaser(LaserBehaviour o)
	{
		FreeGameObject(o.gameObject);
		laserPool.Enqueue(o);
	}
}
