using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(Collider2D))]
public abstract class Projectile : MonoBehaviour
{
	protected Rigidbody2D	rb;

	bool	visible;

	protected virtual void OnEnable()
	{
		visible = false;

		StartCoroutine(DestroyProjectileIfNotVisible());
	}

	IEnumerator DestroyProjectileIfNotVisible()
	{
		yield return new WaitForSeconds(1);

		// If a mystical bug appear with laser (disapear / tp) look here

		if (!visible)
			DestroyProjectile();
	}

	protected abstract void DestroyProjectile();

	protected virtual void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}
	
	private void OnBecameVisible()
	{
		visible = true;
	}

	private void OnBecameInvisible()
	{
		DestroyProjectile();
	}
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player" || other.tag == "Proj")
			DestroyProjectile();
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Player" || other.gameObject.tag == "Proj")
			DestroyProjectile();
	}
}