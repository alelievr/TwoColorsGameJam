using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
	public Vector2		dir;
	public float		speed;
	public float		lifetime = 5;

	Rigidbody2D			rb;

	void Start ()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	public void OnLaserSpawned()
	{
		AudioController.instance.PlayLaserAtPosition(transform.position);
	}
	
	void FixedUpdate ()
	{
		lifetime -= Time.fixedDeltaTime;

		if (lifetime < 0)
			GameObject.Destroy(gameObject);
		rb.velocity = dir * speed;
	}

	private void OnBecameInvisible()
	{
		LaserPool.instance.FreeLaser(this);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
			LaserPool.instance.FreeLaser(this);
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Player")
			LaserPool.instance.FreeLaser(this);
	}
}
