using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class AsteroidController : MonoBehaviour
{
	public SimpleTouchController movementController;

	public float speed = 100;
	public float maxSpeed = 100;
	public float rotationSpeed = 60;

	Vector2 		dir;
	Rigidbody2D		rb;
	AsteroidDeath	deathController;
	Gyroscope		gyro;

	// Use this for initialization
	void Start ()
	{
		gyro = Input.gyro;
		gyro.enabled = true;
		Debug.Log("gyro enabled: " + SystemInfo.supportsGyroscope);
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

		float r = 0;
		if (gyro != null)
		{
			r = gyro.rotationRate.z * rotationSpeed;
			//Debug.Log("gyro rotation: " + gyro.rotationRate);
		}
		
		transform.Rotate(transform.forward, r * Time.deltaTime * rotationSpeed);
	}
	
	public float AugmentationInertia = 50;
	public float minForceSpeed = 50;

	// Update is called once per frame
	void FixedUpdate ()
	{
		if (deathController.dead)
			return ;
		
		rb.AddForce(dir * (speed - Mathf.Max(GameManager.instance.playerSize * AugmentationInertia, minForceSpeed)), ForceMode2D.Force);

		if (Mathf.Abs(rb.velocity.magnitude) > maxSpeed)
		{
			Vector2 tmp = new Vector2(rb.velocity.x, rb.velocity.y);
			tmp = tmp.normalized * maxSpeed;
			rb.velocity = tmp;
		}
	}
}
