using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionPool : GameObjectPool
{
	public static ExplosionPool instance;

	protected Queue<ParticleSystem> explosionPool = null;

	protected override void Awake()
	{
		base.Awake();
		explosionPool = new Queue<ParticleSystem>();
		instance = this;
		AllocPool();
	}

	protected override void OnNewObjectReserved(GameObject go)
	{
		explosionPool.Enqueue(go.GetComponent<ParticleSystem>());
	}

	public ParticleSystem NewExplosion(Vector3 pos, Vector2 dir)
	{
		base.GetGameObject();

		ParticleSystem Explosion = explosionPool.Dequeue();

		Explosion.transform.position = pos;
		
		Benchmark.instance.maxSimultaneousExplosions = allocatedObjectCount;

		return Explosion;
	}

	public void FreeExplosion(ParticleSystem o)
	{
		FreeGameObject(o.gameObject);
		explosionPool.Enqueue(o);
	}
}
