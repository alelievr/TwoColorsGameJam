using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenController : MonoBehaviour
{
	public float		scrollSpeed = 1;
	public float		amplitude = 1;

	Vector3 originalPosition;

	void Start ()
	{
		originalPosition = transform.position;
	}
	
	void Update ()
	{
		Vector3 randomPosition = new Vector3(Mathf.PerlinNoise(Time.time * scrollSpeed + 596.55f, Time.time * scrollSpeed - 965.56f), Mathf.PerlinNoise(Time.time * scrollSpeed - 124.65f, -Time.time * scrollSpeed + 634.54f), 0) * 2.0f - Vector3.one;
		transform.position = originalPosition + randomPosition * amplitude;
	}
}
