using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour {

	public float speed = 100;
	public float maxSpeed = 100;
	Vector2 dir;
	Rigidbody2D rb;

	// Use this for initialization
	void Start ()
	{
		rb = GetComponent<Rigidbody2D>();
	}
	
	Vector2 Directionator()
	{
		Vector2 tmp = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		return tmp;
	}

	private void Update()
	{
		dir = Directionator().normalized;
	}
	// Update is called once per frame
	void FixedUpdate () {
		//Debug.Log("hor" + Input.GetAxisRaw("Horizontal")+ "ver" + Input.GetAxisRaw("Vertical"));
		Debug.Log(rb.velocity.magnitude);
		rb.AddForce(dir * speed, ForceMode2D.Force);
		if (Mathf.Abs(rb.velocity.magnitude) > maxSpeed)
		{
			
			Vector2 tmp = new Vector2(rb.velocity.x, rb.velocity.y);

			// tmp.x = Mathf.Clamp(tmp.x, -100, 100);
			tmp = tmp.normalized * maxSpeed;
			// tmp.y = Mathf.Clamp(tmp.y, -100, 100);
			rb.velocity = tmp;
		}

	}

	void Grossissement()
	{
		Vector3 tmp = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
		tmp = tmp * 1.1f;
		tmp.z = 1f;
		transform.localScale = tmp;
		rb.mass *= 1.1f;
	}

	/// <summary>
	/// Sent when an incoming collider makes contact with this object's
	/// collider (2D physics only).
	/// </summary>
	/// <param name="other">The Collision2D data associated with this collision.</param>
	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "debrit")
		{
			Grossissement();
			Destroy(other.gameObject);
		}
	}
}
