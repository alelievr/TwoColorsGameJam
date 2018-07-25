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
	public AudioClip boostsound;

	public SimpleTouchController movementController;
	public SimpleTouchController rotationController;

	public float rotationSpeed = 60;

	float boostmult = 1;
	public float timeboostins = 0.3f;
	public float boostcd = 1;
	AsteroidDeath	deathController;

	public PlayableDirector	director;
	AudioSource audios;

	// Use this for initialization
	void Start ()
	{
		audios = GetComponent<AudioSource>();
		rb = GetComponent<Rigidbody2D>();
		deathController = GetComponentInChildren< AsteroidDeath >();
	}
	
	Vector2 Directionator()
	{
		// return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
		return movementController.GetTouchPosition();
	}

	IEnumerator boost()
    {
		director.Play();
		float tmptime = timeboostins;
		boostcdtmp = boostcd;
		if (boostsound)
			audios.PlayOneShot(boostsound);
		
		boostmult = 2;
		while (tmptime > 0)
		{
			tmptime -= Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		boostmult = 1;
    }

	float boostcdtmp = -1;

	private void Update()
	{
		if (deathController.dead)
			return ;
		if (boostcdtmp >= 0)
			boostcdtmp -= Time.deltaTime;
		dir = Directionator();
		// if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0)
		// || Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.LeftControl)) && boostcdtmp < 0)
		// 	StartCoroutine(boost());
		float r = rotationController.GetTouchPosition().x;
		transform.Rotate(transform.forward, r * Time.deltaTime * rotationSpeed);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (deathController.dead)
			return ;
		//Debug.Log("hor" + Input.GetAxisRaw("Horizontal")+ "ver" + Input.GetAxisRaw("Vertical"));
		rb.AddForce(dir * speed * boostmult, ForceMode2D.Force);
		if (Mathf.Abs(rb.velocity.magnitude) > maxSpeed * boostmult)
		{
			Vector2 tmp = new Vector2(rb.velocity.x, rb.velocity.y);
			tmp = tmp.normalized * maxSpeed * boostmult;
			rb.velocity = tmp;
		}

	}

}
