using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisPool : GameObjectPool
{
	public static DebrisPool instance;

	protected Queue<DebritController> debrisPool = null;

	protected override void Awake()
	{
		base.Awake();
		debrisPool = new Queue<DebritController>();
		instance = this;
		AllocPool();
	}

	protected override void OnNewObjectReserved(GameObject go)
	{
		debrisPool.Enqueue(go.GetComponent<DebritController>());
	}

	public DebritController NewDebris(Vector3 pos)
	{
		base.GetGameObject();

		DebritController Debris = debrisPool.Dequeue();

		Debris.transform.position = pos;
		
		Benchmark.instance.maxSimultaneousDebriss = allocatedObjectCount;

		return Debris;
	}

	public void FreeDebris(DebritController o)
	{
		FreeGameObject(o.gameObject);
		debrisPool.Enqueue(o);
	}
}
