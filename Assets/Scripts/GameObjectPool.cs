using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : MonoBehaviour {

	// Use this for initialization

	public GameObject prefab;
	public int PooloriginalSize = 100;
	protected List<GameObject> pool = null;

	public virtual List<GameObject>AllocPool(GameObject prefab = null)
	{
		if (this.prefab == null && prefab == null)
		{
			Debug.Log("Error rien a allocker 5655415");
			return null;
		}
		else if (prefab != null)
		{
			if (this.prefab != null && this.prefab != prefab)
			{
				Debug.Log("Warning: pool changing prefab pour l'instant c est pas senser arriver");
				pool.Clear();
			}
			this.prefab = prefab;
		}
		for (int i = 0; i < PooloriginalSize; i++)
			pool.Add(GameObject.Instantiate(this.prefab));
		return pool;
	}

}
