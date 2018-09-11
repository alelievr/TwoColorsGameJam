using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoColDebrisPool : GameObjectPool
{
	public static NoColDebrisPool instance;

	protected Queue<NoColDebritController> debrisPool = null;

	protected override void Awake()
	{
		base.Awake();
		debrisPool = new Queue<NoColDebritController>();
		instance = this;
		AllocPool();
	}

	protected override void OnNewObjectReserved(GameObject go)
	{
		debrisPool.Enqueue(go.GetComponent<NoColDebritController>());
	}

	public NoColDebritController NewDebris(Vector3 pos)
	{
		base.GetGameObject();

		NoColDebritController Debris = debrisPool.Dequeue();

		Debris.transform.position = pos;
		
		Benchmark.instance.maxSimultaneousDebriss = allocatedObjectCount;

		return Debris;
	}

	public void FreeDebris(NoColDebritController o)
	{
		FreeGameObject(o.gameObject);
		debrisPool.Enqueue(o);
	}
}
