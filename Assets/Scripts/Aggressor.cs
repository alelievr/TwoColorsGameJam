using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aggressor : MonoBehaviour {

	public	AggressiveProjectile	projectilePrefab;
	public	GameObject				target;
	[Space]
	public	float		distance = 10;
	public	float		frequancy = 5;

	private	AggressiveProjectile	projectile;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Random.Range(frequancy, 100) >= 99) {
			projectile = (AggressiveProjectile)Instantiate(projectilePrefab, (Vector2)(target.transform.position) + Random.insideUnitCircle.normalized * distance, Quaternion.identity);
			projectile.target = target;
		}
	}
}
