﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : MonoBehaviour {

	// Use this for initialization

	public GameObject prefab;
	public int PooloriginalSize = 1000;
	protected Queue<GameObject> pool;

	protected virtual void Awake()
	{
		pool = new Queue<GameObject>();
	}
	public virtual void AllocPool()
	{
		if (prefab == null)
		{
			Debug.Log("Error rien a allocker 5655415");
			return ;
		}
		for (int i = 0; i < PooloriginalSize; i++)
		{
			GameObject go;
			pool.Enqueue(go = GameObject.Instantiate(prefab));
			go.SetActive(false);
		}
	}

	public virtual GameObject GetGameObject()
	{
		return (pool.Dequeue());
	}

	public virtual void FreeGameObject(GameObject o)
	{
		pool.Enqueue(o);
	}
}