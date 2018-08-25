using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPool : GameObjectPool {

	public static LaserPool instance;
	protected Queue<Laser> poolLaser = null;

	// Use this for initialization
	protected override void Awake()
	{
		base.Awake();
		poolLaser = new Queue<Laser>();
		instance = this;
		AllocPool();
	}

	public override void AllocPool()
	{
		base.AllocPool();

		poolLaser.Clear();// pas opti;
		foreach(GameObject item in pool)
			poolLaser.Enqueue(item.GetComponent<Laser>());
	}

	public void newLaser(Vector3 pos, Vector2 dir)
	{
		if (poolLaser.Count == 0)
			AllocPool();
		base.GetGameObject();
		Laser las = poolLaser.Dequeue();

		las.gameObject.SetActive(true);
		las.transform.position = pos;
		las.dir = dir;
	}

	public void FreeGameObject(Laser o)
	{
		o.gameObject.SetActive(false);
		FreeGameObject(o.gameObject);
		poolLaser.Enqueue(o);
	}
}
