using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class AsteroidController : MonoBehaviour
{

	public float speed = 100;
	public float maxSpeed = 100;
	Vector2 dir;
	Rigidbody2D rb;

	public SimpleTouchController movementController;
	public SimpleTouchController rotationController;

	public float rotationSpeed = 60;

	AsteroidDeath	deathController;

	public PlayableDirector	director;

	// Use this for initialization
	void Start ()
	{
		rb = GetComponent<Rigidbody2D>();
		deathController = GetComponentInChildren< AsteroidDeath >();
	}
	
	Vector2 Directionator()
	{
		return movementController.GetTouchPosition();
	}

	private void Update()
	{
		if (deathController.dead)
			return ;
		
		dir = Directionator();
		// TODO: change the direction handle here
		float r = rotationController.GetTouchPosition().x;
		transform.Rotate(transform.forward, r * Time.deltaTime * rotationSpeed);
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (deathController.dead)
			return ;
		
		rb.AddForce(dir * speed, ForceMode2D.Force);

		if (Mathf.Abs(rb.velocity.magnitude) > maxSpeed)
		{
			Vector2 tmp = new Vector2(rb.velocity.x, rb.velocity.y);
			tmp = tmp.normalized * maxSpeed;
			rb.velocity = tmp;
		}
	}
}
