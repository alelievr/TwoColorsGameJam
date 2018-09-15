using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebritSpawner : MonoBehaviour
{
	public int			chunkSize;
	public int			chunkLoadCount = 1;
	public int			debrisPerChunk = 1;
	public float		noiseIntensity = 2;
	public float		turbulance = 0.1f;

	float				debrisDensity;

	HashSet<Vector2Int>	loadedChunks = new HashSet<Vector2Int>();
	Vector2Int			oldPlayerChunk;

	private void Start()
	{
		debrisDensity = (1f / debrisPerChunk) * chunkSize;
		UpdateChunks(Vector2Int.zero);
	}

	void Update ()
	{
		Vector2 p = GameManager.instance.playerTransform.position / chunkSize;
		Vector2Int playerChunk = new Vector2Int((int)p.x, (int)p.y);

		if (playerChunk != oldPlayerChunk)
		{
			UpdateChunks(playerChunk);
		}

		oldPlayerChunk = playerChunk;
	}

	void UpdateChunks(Vector2Int playerChunkPos)
	{
		for (int x = -chunkLoadCount; x <= chunkLoadCount; x++)
		{
			for (int y = -chunkLoadCount; y <= chunkLoadCount; y++)
			{
				Vector2Int chunkPos = playerChunkPos + new Vector2Int(x, y);
				if (!loadedChunks.Contains(chunkPos))
					LoadChunk(chunkPos);
			}
		}
	}

	void LoadChunk(Vector2Int chunkPos)
	{
		Vector2 cellSize = new Vector2(chunkSize / debrisPerChunk, chunkSize / debrisPerChunk) / 2f;

		for (float x = 0; x < chunkSize; x += debrisDensity)
		{
			for (float y = 0; y < chunkSize; y += debrisDensity)
			{
				Vector3 pos = chunkPos * chunkSize + new Vector2(x, y) - new Vector2(chunkSize / 2f, chunkSize / 2f) + cellSize;

				pos.x += (Mathf.PerlinNoise(pos.x * turbulance, pos.y * turbulance) - 0.5f) * debrisPerChunk * noiseIntensity;
				pos.y += (Mathf.PerlinNoise(pos.x * turbulance + 1000, pos.y * turbulance + 1000) - 0.5f) * debrisPerChunk * noiseIntensity;

				DebrisPool.instance.NewDebris(pos);
			}
		}
		loadedChunks.Add(chunkPos);
	}

	private void OnDrawGizmos()
	{
		foreach (var v in loadedChunks)
			Gizmos.DrawWireCube((Vector2)v * chunkSize, Vector3.one * chunkSize);
	}
}
