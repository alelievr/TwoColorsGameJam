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

	public	float			size {
		get { return GameManager.instance.playerSize; }
	}


	private int			pickedProjectile;

	private	AggressiveProjectile	projectile;
	private Rigidbody2D				rbTarget;
	int countenemy = 10;

	void Start ()
	{
		rbTarget = target.GetComponent<Rigidbody2D>();
	}
	
	void Update () {
		// Debug.Log(rbTarget.velocity.magnitude);
		if (/*!isBossFight &&*/ Random.value >= frequancy)
			return;

		bigList.list.Take(gameState).ToList().ForEach(l => {
			projectile = Instantiate(l.aggressiveProjectileList
			.Where(m => {
				if (m.GetComponent<BasicEnemy>() != null && GameManager.instance.enemylimit < 0)						
					return false;
				return true;
			})
			.OrderBy((k) => Random.value)
			.Take(1)
			.First(), (Vector2)(target.transform.position) + Random.insideUnitCircle.normalized * (distance + rbTarget.velocity.magnitude + size), Quaternion.identity);
			projectile.target = target;
		});
	}
}
