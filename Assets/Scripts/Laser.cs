using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
	public Vector2		dir;
	public float		speed;
	public float		lifetime = 5;
	public AudioClip	spawnSound;

	Rigidbody2D			rb;

	void Start ()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void OnEnable()
	{
		// TODO: do not use PlayOneShotOnPlayer it disable audio spatialization !
		AudioController.instance.PlayOneShotOnPlayer(spawnSound);
	}
	
	void FixedUpdate ()
	{
		lifetime -= Time.fixedDeltaTime;

		if (lifetime < 0)
			GameObject.Destroy(gameObject);
		rb.velocity = dir * speed;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
			LaserPool.instance.FreeGameObject(this);
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Player")
			LaserPool.instance.FreeGameObject(this);
	}
}
