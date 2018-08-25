using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DebritController : MonoBehaviour
{
	public GameObject	player;

	[Header("Target reach settings")]
	public float toVel = 2.5f;
	public float maxVel = 15.0f;
	public float maxForce = 40.0f;
	public float gain = 5f;

	public event Action< DebritController >	onLaserReceived;
	public event Action< DebritController >	onDestroyed;

	public GameObject	debritExplosionPrefab;

	Rigidbody2D			rb;
	CircleCollider2D	circleCollider;
	Collider2D[]		results = new Collider2D[16];
	DebritManager		manager;
	bool				agglomerationEnabled;

	public int			integrity = 0;

	bool dead = false;

	List<DebritController>	touchingDebrits = new List<DebritController>();

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		circleCollider = GetComponent< CircleCollider2D >();
		manager = DebritManager.instance;

		StartCoroutine("Killme");
	}

	public void Agglomerate(int integrity)
	{
		if (agglomerationEnabled)
			return ;
		
		AudioController.instance.PlayAggregateOnPlayer();

		tag = "Player";

		this.integrity = integrity;
		
		Destroy(rb);

		ContactFilter2D filter = new ContactFilter2D();
		int count = circleCollider.OverlapCollider(filter, results);

		for (int i = 0; i < count; i++)
			touchingDebrits.Add(results[i].GetComponent<DebritController>());

		integrity = 0;
		StopCoroutine("Killme");
		agglomerationEnabled = true;
	}

	public void CheckIntegryty(int newIntegrity)
	{
		if (newIntegrity == integrity)
			return ;
		
		integrity = newIntegrity;

		foreach (var touchingDebrit in touchingDebrits)
		{
			// Safety check
			if (touchingDebrit != null)
				touchingDebrit.CheckIntegryty(newIntegrity);
		}
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (!agglomerationEnabled)
			return ;
		
		if (other.gameObject.tag == "debrit")
		{
			var otherDebrit = other.gameObject.GetComponent< DebritController >();
			manager.AgglomerateDebrit(otherDebrit);
			touchingDebrits.Add(otherDebrit);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Laser")
		{
			if (agglomerationEnabled && onLaserReceived != null)
				onLaserReceived(this);
			OnWillBeDestroyed();
			Destroy(gameObject);
			Destroy(other.gameObject);
		}
	}

	IEnumerator Killme()
	{
		yield return new WaitForSeconds(10f);
		Destroy(gameObject);
	}
	
	public void OnWillBeDestroyed()
	{
		if (dead)
			return ;
		
		dead = true;
		Instantiate(debritExplosionPrefab, transform.position, Quaternion.identity);

		// TODO: check if spatialization is important here ?
		Debug.Log("explosion !");
		AudioController.instance.PlayExplosionOnPlayer();
	}

	private void OnDestroy()
	{
		foreach (var debrit in touchingDebrits)
		{
			if (debrit != null)
				debrit.touchingDebrits.Remove(this);
		}

		if (onDestroyed != null)
			onDestroyed(this);
	}
}
