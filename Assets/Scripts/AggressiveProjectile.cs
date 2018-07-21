using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AggressiveProjectile : MonoBehaviour {


	public	GameObject	target;
	[Space]
	public	float		prec = 10;
	public	float		speed = 10;

	private Rigidbody2D	rb;
	float basedistance = 0;
	// Use this for initialization
	void Start () {
		rb = gameObject.GetComponent<Rigidbody2D>();
		// transform.rotation = Quaternion.LookRotation(target.transform.position);
		rb.AddForce((((target.transform.position - transform.position).normalized + new Vector3(Random.Range(-prec, prec), Random.Range(-prec, prec), 0).normalized * 0.1f)).normalized * speed);
		basedistance = Vector3.Distance(transform.position, target.transform.position) * 2;
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector3.Distance(transform.position, target.transform.position) > basedistance)
			GameObject.Destroy(gameObject);
	}
}
