using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// a mettre sur le player

public class spawnquejecomprend : MonoBehaviour {

	// Use this for initialization
	public List<GameObject> list;
	public	float		frequancy = 0.05f;
	public	float		distance = 10;
	Rigidbody2D rbplayer = null;

	public	float			size {
		get { return GameManager.instance.playerSize; }
	}
	void Start () {
		rbplayer = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		list.ForEach(l =>{
			if (Random.value < frequancy)
				GameObject.Instantiate(l, (Vector2)(transform.position) + Random.insideUnitCircle.normalized * (distance + rbplayer.velocity.magnitude + size), Quaternion.identity);
		});
	}
}
