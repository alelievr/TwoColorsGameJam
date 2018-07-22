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

	CinemachineVirtualCamera vcam;
    CinemachineBasicMultiChannelPerlin vcamperlin;
	public GameObject	invoqueondead;
	public int			numbertoinvoc = 15;

	public int debritCount = 2;
	public GameObject debrit;
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

	float invudegat = 0.2f;
	void OnCollisionEnter2D(Collision2D other)
	{
	//	Debug.Log("fdsf");
		Kaboom impactant;

		if ((impactant = other.gameObject.GetComponent<Kaboom>()) != null)
		{
			Debug.Log(name);
			Vector2 realvelocity = impactant.rbody.velocity - rbody.velocity;
			// if (this.tag == "Player" && vcam != null)
			// 	StartCoroutine(impactoEffect());
			if (resitimpact < 0.5f && invudegat < 0) //lol
			{
				life -= realvelocity.magnitude;
				invudegat = 0.2f;
			}
			if (life < 0)
				die();
			if (other.gameObject.tag == "boss")
				rbody.velocity += (Vector2)(other.transform.position - transform.position).normalized * 2000;
	//		Debug.Log("fdsf2" + life);
			// rbody.velocity += realvelocity;
		}
	}

	private void Update()
	{
		if (invudegat >= 0)
			invudegat -= Time.deltaTime;
	}
}

