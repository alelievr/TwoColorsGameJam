using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class EnemySpawnerInfo
{
	public GameObject	prefab;
	public float		spawnAfterDistance = 0;
	public float		spawnRatio = 1;
	public float		spawnRatioOverDistance = 0;
}

public class EnemySpawner : MonoBehaviour
{
	// TODO: pollify this
	public List< EnemySpawnerInfo >	enemies = new List< EnemySpawnerInfo >();

	[Space, Header("Spawn settings")]
	public float	spawnDelay = 5;
	public float	spawnDelayVariation = 2f;
	public float	radius = 5;

	public float traveledDistance
	{
		get { return 0; } // TODO: implement this
	}

	void Start ()
	{
		StartCoroutine(SpawnEnemies());
	}
	
	IEnumerator SpawnEnemies()
	{
		while (true)
		{
			yield return new WaitForSeconds(spawnDelay + Random.Range(-spawnDelayVariation / 2f, spawnDelayVariation / 2f));
			
			float totalSpawnRatio = enemies.Sum((e) => {
				if (traveledDistance < e.spawnAfterDistance)
					return 0;
				float dist = traveledDistance - e.spawnAfterDistance;
				return e.spawnRatio + e.spawnRatioOverDistance * dist;
			});

			float r = Random.Range(0, totalSpawnRatio);
			float d = 0;
			foreach (var e in enemies)
			{
				if (traveledDistance < e.spawnAfterDistance)
					continue ;
				
				float dist = traveledDistance - e.spawnAfterDistance;
				d += e.spawnRatio + e.spawnRatioOverDistance * dist;
				if (r >= d)
				{
					SpawnEnemy(e.prefab);
					break ;
				}
			}
			
			yield return new WaitForEndOfFrame();
		}
	}

	void SpawnEnemy(GameObject prefab)
	{
		Vector2 randomDirection = Random.insideUnitCircle.normalized;
		Vector3 pos = (Vector2)GameManager.instance.playerPos + randomDirection * Mathf.Sqrt(GameManager.instance.playerSizeSqr) + randomDirection * radius;
		GameObject.Instantiate(prefab, pos, Quaternion.identity);
	}
}
