using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour {

	public float speed = 100;
	Vector2 dir;
	Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
	}
	
	Vector2 Directionator()
	{
		Vector2 tmp = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		return tmp;
	}

	private void Update()
	{
		dir = Directionator();
	}
	// Update is called once per frame
	void FixedUpdate () {
		//Debug.Log("hor" + Input.GetAxisRaw("Horizontal")+ "ver" + Input.GetAxisRaw("Vertical"));
		// Debug.Log(rb.velocity);
		rb.AddForce(dir * speed * Time.fixedDeltaTime, ForceMode2D.Force);
		if (rb.velocity.x > 100 || rb.velocity.y > 100)
		{
			Vector2 tmp = new Vector2(rb.velocity.x, rb.velocity.y);
			tmp.x = Mathf.Clamp(tmp.x, -100, 100);
			tmp.y = Mathf.Clamp(tmp.y, -100, 100);
			rb.velocity = tmp;
		}

	}
}
