using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PlayerConstrainstZone : MonoBehaviour
{
	public GameObject		player;
	public bool				active;
	public float			repulseRadius = 10;

	new CircleCollider2D	collider;
	Rigidbody2D				playerRigidBody;

	void Start ()
	{
		collider = GetComponent< CircleCollider2D >();
		playerRigidBody = player.GetComponent< Rigidbody2D >();
	}
	
	void Update ()
	{
		if (!active)
			return ;
		
		Vector2 diff = player.transform.position - transform.position;
		if (diff.magnitude > collider.radius)
			diff = Vector2.ClampMagnitude(diff, collider.radius - 1);

		// If the player is near the limit, we apply a force in direction of the center of the zone
		if (diff.magnitude > collider.radius - repulseRadius)
		{
			float inversePower = repulseRadius / (collider.radius - diff.magnitude);
			playerRigidBody.AddForce(-diff.normalized * inversePower * 10, ForceMode2D.Impulse);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject == player)
		{
			active = true;
		}
	}
}
