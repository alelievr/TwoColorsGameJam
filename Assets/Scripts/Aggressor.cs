using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Aggressor : MonoBehaviour {

[System.Serializable]
	public class AggressiveProjectileList{
		public List<AggressiveProjectile> aggressiveProjectileList;
	}

	[System.Serializable]
	public class BigList
	{
		public List<AggressiveProjectileList> list;
	}

	[SerializeField]
	public	BigList					bigList;
	public	AggressiveProjectile	projectilePrefab;
	public	GameObject				target;
	[Space]
	public	float		distance = 10;
	public	float		frequancy;
	public	int			gameState
	{
		get { return GameManager.instance.gameState; }
	}

	public	bool			isBossFight {
		get { return GameManager.instance.isBossFight; }
	}


	private int			pickedProjectile;

	private	AggressiveProjectile	projectile;

	void Start ()
	{
		
	}
	
	void Update () {
		if (!isBossFight && Random.value >= frequancy)
			return;
		
		bigList.list.Take(gameState).ToList().ForEach(l => {
			projectile = Instantiate(l.aggressiveProjectileList
			.Skip(Random.Range(0, l.aggressiveProjectileList.Count))
			.Take(1)
			.First(), (Vector2)(target.transform.position) + Random.insideUnitCircle.normalized * distance, Quaternion.identity);
			projectile.target = target;
		});


	}
}
