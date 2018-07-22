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
	public GameObject	explosionPrefab;
	public GameObject	agglomeratePrefab;

	Rigidbody2D			rb;
	CircleCollider2D		circleCollider;
	Vector2				dir;
	int					index;
	Collider2D[]		results = new Collider2D[16];
	DebritManager		manager;
	bool				agglomerationEnabled;

	public int			integrity = 0;

	List<DebritController>	touchingDebrits = new List<DebritController>();

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		circleCollider = GetComponent< CircleCollider2D >();
		manager = DebritManager.instance;

		if (manager != null)
			index = manager.GetNewDebritIndex();
		StartCoroutine("Killme");
	}

	Vector2 Directionator()
	{
		Vector2 target = manager.GetDebritPosition(index);
		return new Vector2(target.x - transform.position.x, target.y - transform.position.y);
	}

	private void Update()
	{
		dir = Directionator();
	}

	// Update is called once per frame
	// void FixedUpdate()
	// {
	// 	if (agglomerationEnabled)
	// 		return ;
		
	// 	// Control the force to avoid overshooting the target:
	// 	Vector2 tgtVel = Vector2.ClampMagnitude(toVel * dir, maxVel);
	// 	// calculate the velocity error
	// 	Vector2 error = tgtVel - rb.velocity;
	// 	// calc a force proportional to the error (clamped to maxForce)
	// 	Vector2 force = Vector2.ClampMagnitude(gain * error, maxForce);
	// 	//rb.AddForce(force);

	// 	if (Mathf.Abs(rb.velocity.magnitude) > 200)
	// 	{

	// 		Vector2 tmp = new Vector2(rb.velocity.x, rb.velocity.y);

	// 		// tmp.x = Mathf.Clamp(tmp.x, -100, 100);
	// 		tmp = tmp.normalized * 200;
	// 		// tmp.y = Mathf.Clamp(tmp.y, -100, 100);
	// 		rb.velocity = tmp;
	// 	}
	// }



	public void Agglomerate(int integrity)
	{
		if (agglomerationEnabled)
			return ;
			
		Instantiate(agglomeratePrefab, transform.position, Quaternion.identity);

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
		Instantiate(debritExplosionPrefab, transform.position, Quaternion.identity);
		Instantiate(explosionPrefab, transform.position, Quaternion.identity);
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
