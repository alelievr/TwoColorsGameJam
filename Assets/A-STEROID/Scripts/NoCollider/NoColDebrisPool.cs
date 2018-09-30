﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoColDebrisPool : GameObjectPool
{
	public static NoColDebrisPool instance;

	protected Queue<NoColDebrisController> debrisPool = null;

	protected override void Awake()
	{
		base.Awake();
		debrisPool = new Queue<NoColDebrisController>();
		instance = this;
		AllocPool();
	}

	protected override void OnNewObjectReserved(GameObject go)
	{
		debrisPool.Enqueue(go.GetComponent<NoColDebrisController>());
	}

	public NoColDebrisController NewDebris(Vector3 pos)
	{
		base.GetGameObject();

		NoColDebrisController Debris = debrisPool.Dequeue();

		Debris.transform.position = pos;
		
		Benchmark.instance.maxSimultaneousDebriss = allocatedObjectCount;

		return Debris;
	}

	public void FreeDebris(NoColDebrisController o)
	{
		FreeGameObject(o.gameObject);
		debrisPool.Enqueue(o);
	}
}
