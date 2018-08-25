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

		foreach(GameObject item in pool)
			poolLaser.Enqueue(item.GetComponent<Laser>());
	}

	public void newLaser(Vector3 pos, Vector3 dir)
	{
		base.GetGameObject();
		Laser las = poolLaser.Dequeue();

		las.gameObject.SetActive(true);
		las.transform.position = pos;
		las.dir = dir;
		AudioController.instance.PlayOneShotOnPlayer(spawnSound);
	}

	public void FreeGameObject(Laser o)
	{
		o.gameObject.SetActive(false);
		FreeGameObject(o.gameObject);
		poolLaser.Enqueue(o);
	}
}
