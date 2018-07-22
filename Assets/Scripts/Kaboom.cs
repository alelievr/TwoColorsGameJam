﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(Rigidbody2D))]
public class Kaboom : MonoBehaviour {

	public float poid = 1;
	[Range(0, 1)] public float resitimpact = 0;
	public float life = 1000;
	[HideInInspector] public Rigidbody2D rbody;

	public float		flickerCount = 4;
	public float		flickerInterval = 0.2f;

	CinemachineVirtualCamera vcam;
    CinemachineBasicMultiChannelPerlin vcamperlin;
	public GameObject	invoqueondead;
	public int			numbertoinvoc = 15;

	public int debritCount = 2;
	public GameObject debrit;

	[Space]
	public	float		recupTime;
	public	bool		recup;
	// Use this for initialization
	void Start () {
		rbody = GetComponent<Rigidbody2D>();
		if (tag == "player")
		{
			vcam = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera as CinemachineVirtualCamera;
			if (vcam)
        	    vcamperlin = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
		}
	}
	
    IEnumerator Destroyation()
    {
        yield return new WaitForSeconds(0.1f);
		Destroy(gameObject);
    }

	void die()
	{
		for (int i = 0; i < numbertoinvoc; i++)
		{
			GameObject.Instantiate(invoqueondead, transform.position, Quaternion.identity);
		}
		for (int i = 0; i < debritCount; i++)
		{
			GameObject.Instantiate(debrit, transform.position, Quaternion.identity);
		}
		if (tag == "boss")
			GameManager.instance.DefeatBoss();
		StartCoroutine(Destroyation());
	}

	// private void Update()
	// {
	// 	if (Input.GetKeyDown(KeyCode.Q)) {
	// 		testhit();
	// 	}
		
	// }

	IEnumerator Flicker()
	{
		for (int i = 0; i < flickerCount; i++)
			yield return new WaitForSeconds(flickerInterval);
	}

	IEnumerator Recup()
	{
		recup = true;
		yield return new WaitForSeconds(recupTime);
		recup = false;
	}


	void OnCollisionEnter2D(Collision2D other)
	{
		Debug.Log("fdsf");
		Kaboom impactant;

		if ( !recup && (impactant = other.gameObject.GetComponent<Kaboom>()) != null )
		{
			Vector2 realvelocity = impactant.rbody.velocity - rbody.velocity;
			// if (this.tag == "Player" && vcam != null)
			// 	StartCoroutine(impactoEffect());
			if (resitimpact < 0.5f || other.gameObject.tag == "Player") //lol
			{
				StartCoroutine(Flicker());
				StartCoroutine(Recup());
				life -= realvelocity.magnitude;
			}
			if (life < 0)
				die();
			if (other.gameObject.tag == "boss")
				rbody.velocity += (Vector2)(other.transform.position - transform.position).normalized * 2000;
	//		Debug.Log("fdsf2" + life);
			// rbody.velocity += realvelocity;
		}
	}
}
