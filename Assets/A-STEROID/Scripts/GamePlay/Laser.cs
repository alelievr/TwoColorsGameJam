using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
	public float		speed;

	Rigidbody2D			rb;
	bool				visible;
	Vector2				forward;

	void Start ()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	public void OnLaserSpawned()
	{
		AudioController.instance.PlayLaserAtPosition(transform.position);
		visible = false;
		forward = transform.right;

		StartCoroutine(DestroyLaser());
	}

	IEnumerator DestroyLaser()
	{
		yield return new WaitForSeconds(1);

		// If a mystical bug appear with laser (disapear / tp) look here

		if (!visible)
			LaserPool.instance.FreeLaser(this);
	}
	
	void FixedUpdate ()
	{
		rb.velocity = forward * speed;
	}

	void FreeLaser()
	{
		LaserPool.instance.FreeLaser(this);
	}

	private void OnBecameVisible()
	{
		visible = true;
	}

	private void OnBecameInvisible()
	{
		FreeLaser();
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
