using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebritController : MonoBehaviour {

	public GameObject player;
	Rigidbody2D rb;
	Vector2 dir;
	float speed = 5000;	

	void Start () {
		rb = GetComponent<Rigidbody2D>();
		
	}

	Vector2 Directionator()
	{
		Vector2 tmp = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
		return tmp;
	}

	 private void Update()
	{
		dir = Directionator().normalized;
	}
	// Update is called once per frame
	void FixedUpdate () {
		rb.AddForce(dir * speed * Time.fixedDeltaTime, ForceMode2D.Force);	
		if (Mathf.Abs(rb.velocity.magnitude) > 200)
		{
			
			Vector2 tmp = new Vector2(rb.velocity.x, rb.velocity.y);

			// tmp.x = Mathf.Clamp(tmp.x, -100, 100);
			tmp = tmp.normalized * 200;
			// tmp.y = Mathf.Clamp(tmp.y, -100, 100);
			rb.velocity = tmp;
		}
	}
}
