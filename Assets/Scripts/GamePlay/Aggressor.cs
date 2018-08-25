using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Aggressor : MonoBehaviour {

	[System.Serializable]
	public class AggressiveProjectileInfo
	{
		public AggressiveProjectile	projectile;
		public float				spawnRatio = 1;
	}

	[System.Serializable]
	public class AggressiveProjectileInfoList
	{
		public List<AggressiveProjectileInfo> aggressiveProjectileInfoList;
	}

	[System.Serializable]
	public class BigList
	{
		public List<AggressiveProjectileInfoList> list;
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

	void Start ()
	{
		rbTarget = target.GetComponent<Rigidbody2D>();

		StartCoroutine(SpawnAggressors());
	}

	IEnumerator SpawnAggressors()
	{
		while (true)
		{
			yield return new WaitForFixedUpdate();

			if (Random.value >= frequancy)
				continue ;

			bigList.list.Take(gameState).ToList().ForEach(l => {
				var randomObject = l.aggressiveProjectileInfoList
				.Where(m => {
					if (m.projectile.GetComponent<BasicEnemy>() != null)
					{
						if (GameManager.instance.enemylimit < 0)
							return false;
						if (GameManager.instance.isBossFight)
							return false;
					}
					return true;
				})
				.OrderByDescending((p) => Random.value * p.spawnRatio)
				.FirstOrDefault();
				if (randomObject != null)
				{
					projectile = Instantiate(randomObject.projectile, (Vector2)(target.transform.position) + Random.insideUnitCircle.normalized * (distance + rbTarget.velocity.magnitude + size), Quaternion.identity);
					projectile.target = target;
				}
			});
		}
	}
}
