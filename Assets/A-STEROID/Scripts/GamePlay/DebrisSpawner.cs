using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisSpawner : MonoBehaviour
{
	[Header("Chunk spawn settings")]
	public int			chunkSize = 256;
	public int			chunkLoadCount = 1;
	public int			chunkUnloadDistance = 3;
	public int			debrisPerChunk = 3;
	public float		noiseIntensity = 60;
	public float		turbulance = 1f;

	[Header("Random spawn settings")]
	public float		radius = 50;
	public float		jitterDirection = 5;
	public float		spawnPerSeconds = 2;
	public Vector2		speedMultiplierRange = new Vector2(0.5f, 3f);

	float				debrisDensity;
	Vector2Int			oldPlayerChunk;

	Dictionary<Vector2Int, List< DebrisController > >	loadedChunks = new Dictionary<Vector2Int, List< DebrisController > >();

	private void Start()
	{
		debrisDensity = (1f / debrisPerChunk) * chunkSize;
		UpdateChunks(Vector2Int.zero);

		StartCoroutine(SpawnRandomDebris());
	}

	void Update ()
	{
		Vector2 p = GameManager.instance.playerPos / chunkSize;
		Vector2Int playerChunk = new Vector2Int((int)p.x, (int)p.y);

		if (playerChunk != oldPlayerChunk)
		{
			UpdateChunks(playerChunk);
		}

		oldPlayerChunk = playerChunk;
	}

	IEnumerator SpawnRandomDebris()
	{
		while (true)
		{
			yield return new WaitForSeconds(1f / spawnPerSeconds);

			Vector3 pos = GameManager.instance.playerPos + (Vector3)Random.insideUnitCircle.normalized * radius;
			Debug.Log("playerPos: " + GameManager.instance.playerPos);
			var debris = DebrisPool.instance.NewDebris(pos);
			Vector3 force = (GameManager.instance.playerPos + (Vector3)Random.insideUnitCircle * jitterDirection) - pos;
			debris.ApplyForce(force * Random.Range(speedMultiplierRange.x, speedMultiplierRange.y));
			debris.destroyIfInvisible = true;
		}
	}

	void UpdateChunks(Vector2Int playerChunkPos)
	{
		if (debrisPerChunk <= 0)
			return ;
		
		for (int x = -chunkLoadCount; x <= chunkLoadCount; x++)
		{
			for (int y = -chunkLoadCount; y <= chunkLoadCount; y++)
			{
				Vector2Int chunkPos = playerChunkPos + new Vector2Int(x, y);
				if (!loadedChunks.ContainsKey(chunkPos))
					LoadChunk(chunkPos);
			}
		}

		foreach (var loadedChunkPosition in loadedChunks)
		{
			if (Vector2Int.Distance(loadedChunkPosition.Key, playerChunkPos) > chunkUnloadDistance)
				UnloadChunk(loadedChunkPosition.Key);
		}
	}

	void LoadChunk(Vector2Int chunkPos)
	{
		Vector2 cellSize = new Vector2(chunkSize / debrisPerChunk, chunkSize / debrisPerChunk) / 2f;
		List<DebrisController>	debris = new List<DebrisController>();

		for (float x = 0; x < chunkSize; x += debrisDensity)
		{
			for (float y = 0; y < chunkSize; y += debrisDensity)
			{
				Vector3 pos = chunkPos * chunkSize + new Vector2(x, y) - new Vector2(chunkSize / 2f, chunkSize / 2f) + cellSize;

				pos.x += (Mathf.PerlinNoise(pos.x * turbulance, pos.y * turbulance) - 0.5f) * debrisPerChunk * noiseIntensity;
				pos.y += (Mathf.PerlinNoise(pos.x * turbulance + 1000, pos.y * turbulance + 1000) - 0.5f) * debrisPerChunk * noiseIntensity;

				var d = DebrisPool.instance.NewDebris(pos);
				d.destroyIfInvisible = false;
				debris.Add(d);
			}
		}
		loadedChunks.Add(chunkPos, debris);
	}

	void UnloadChunk(Vector2Int chunkPos)
	{
		foreach (var debris in loadedChunks[chunkPos])
			DebrisPool.instance.FreeDebris(debris);
		loadedChunks.Remove(chunkPos);
	}

	private void OnDrawGizmos()
	{
		foreach (var v in loadedChunks)
			Gizmos.DrawWireCube((Vector2)v.Key * chunkSize, Vector3.one * chunkSize);
	}
}
