using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

	// Use this for initialization
	public Vector2 dir;
	public float speed;
	public float lifetime = 5;
	Rigidbody2D rb;
	void Start () {
		rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		lifetime -= Time.fixedDeltaTime;
		if (lifetime < 0)
			GameObject.Destroy(gameObject);
			rb.velocity = dir * speed;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
			Destroy(gameObject);
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.tag == "Player")
			Destroy(gameObject);
	}
}
