using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggressiveProjectile : MonoBehaviour {


	public	GameObject	target;
	[Space]
	public	float		prec = 10;
	public	float		speed = 10;

	private Rigidbody2D	rb;
	// Use this for initialization
	void Start () {
		rb = gameObject.GetComponent<Rigidbody2D>();
		// transform.rotation = Quaternion.LookRotation(target.transform.position);
		rb.AddForce(((target.transform.position - transform.position) + new Vector3(Random.Range(-prec, prec), Random.Range(-prec, prec), 0)) * speed);
	}
	
	// Update is called once per frame
	void Update () {
	}
}
