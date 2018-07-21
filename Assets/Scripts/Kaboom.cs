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
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.Space))
			rbody.velocity = new Vector2((name == "blanc") ? 20 : -20, 0);
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
		StartCoroutine(Destroyation());
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		Debug.Log("fdsf");
		Kaboom impactant;

		if ((impactant = other.gameObject.GetComponent<Kaboom>()) != null)
		{
			Vector2 realvelocity = impactant.rbody.velocity - rbody.velocity;
			// if (this.tag == "Player" && vcam != null)
			// 	StartCoroutine(impactoEffect());
			life -= realvelocity.magnitude;
			if (life < 0)
				die();
			Debug.Log("fdsf2" + life);
			// rbody.velocity += realvelocity;
		}
	}
}