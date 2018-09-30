using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisPool : GameObjectPool
{
	public static DebrisPool instance;

	protected Queue<DebrisController> debrisPool = null;

	protected override void Awake()
	{
		base.Awake();
		debrisPool = new Queue<DebrisController>();
		instance = this;
		AllocPool();
	}

	protected override void OnNewObjectReserved(GameObject go)
	{
		debrisPool.Enqueue(go.GetComponent<DebrisController>());
	}

	public DebrisController NewDebris(Vector3 pos)
	{
		base.GetGameObject();

		DebrisController Debris = debrisPool.Dequeue();

		Debris.transform.position = pos;
		
		Benchmark.instance.maxSimultaneousDebriss = allocatedObjectCount;

		return Debris;
	}

	public void FreeDebris(DebrisController o)
	{
		// Prevent double free
		if (!o.gameObject.activeSelf)
			return ;
		
		FreeGameObject(o.gameObject);
		debrisPool.Enqueue(o);
	}
}
