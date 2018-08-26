using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameObjectPool : MonoBehaviour
{
	public GameObject				prefab;
	public int						poolOriginalSize = 100;

	protected Queue<GameObject>		pool;
	protected int					allocatedObjectCount;

	protected virtual void Awake()
	{
		pool = new Queue<GameObject>();
	}

	protected void AllocPool()
	{
		if (prefab == null)
		{
			// wtf 🤔 ???
			Debug.Log("Error rien a allocker 5655415");
			return ;
		}
		prefab.SetActive(false);
		for (int i = 0; i < poolOriginalSize; i++)
		{
			GameObject go = GameObject.Instantiate(prefab);
			pool.Enqueue(go);
			OnNewObjectReserved(go);
		}
	}

	protected abstract void OnNewObjectReserved(GameObject go);

	protected GameObject GetGameObject()
	{
		allocatedObjectCount++;
		
		if (pool.Count == 0)
			AllocPool();

		GameObject go = pool.Dequeue();
		go.SetActive(true);
		return go;
	}

	protected void FreeGameObject(GameObject o)
	{
		allocatedObjectCount--;

		o.SetActive(false);
		pool.Enqueue(o);
	}
}
