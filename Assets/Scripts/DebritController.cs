using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebritController : MonoBehaviour
{
	public GameObject player;
	Rigidbody2D rb;
	Vector2 dir;
	int index;

	[Header("Target reach settings")]
	public float toVel = 2.5f;
	public float maxVel = 15.0f;
	public float maxForce = 40.0f;
	public float gain = 5f;

	DebritManager	manager;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		manager = DebritManager.instance;

		if (manager != null)
			index = manager.GetNewDebritIndex();
	}

	Vector2 Directionator()
	{
		Vector2 target = manager.GetDebritPosition(index);
		return new Vector2(target.x - transform.position.x, target.y - transform.position.y);
	}

	private void Update()
	{
		dir = Directionator();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		// Control the force to avoid overshooting the target:
		Vector2 tgtVel = Vector2.ClampMagnitude(toVel * dir, maxVel);
		// calculate the velocity error
		Vector2 error = tgtVel - rb.velocity;
		// calc a force proportional to the error (clamped to maxForce)
		Vector2 force = Vector2.ClampMagnitude(gain * error, maxForce);
		rb.AddForce(force);

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
