﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyEntity : MonoBehaviour
{
	public bool instanfind = false;
	public GameObject cible = null;
	Transform cibleTransform = null;
	public float speed = 30;
	public float maxspeed = 20;
	public float rotationspeed = 30;
	public float maxrotationspeed = 20;
	public float idealdistancetocible = 10;
	public float agroDistance = Mathf.Infinity;
	public enum TypeOfMouvement {mouche};
	public TypeOfMouvement typeOfMouvement = TypeOfMouvement.mouche;
	// Use this for initialization

	protected Rigidbody2D rb;

	protected void BaseStart ()
	{
		rb = GetComponent<Rigidbody2D>();
		if (instanfind == true)
		{
			cible = GameManager.instance.player;
			cibleTransform = GameManager.instance.playerTransform;
		}
	}

	void MoucheMouvement()
	{
		if (cible == null || Vector2.Distance(transform.position, cibleTransform.position) > agroDistance)
			return ;
		Vector2 idealpostmp = transform.position + (cibleTransform.position - transform.position).normalized * idealdistancetocible;
		rb.AddForce(((Vector2)cibleTransform.position - idealpostmp).normalized * speed, ForceMode2D.Force);
		if (Mathf.Abs(rb.velocity.magnitude) > maxspeed)
		{
			Vector2 tmp = new Vector2(rb.velocity.x, rb.velocity.y);
			tmp = tmp.normalized * maxspeed;
			rb.velocity = tmp;
		}
		int sideToTurnTo = 0;
       
        Vector3 angleToSpotRelativeToMe = cibleTransform.position - transform.position;
       
        // get its cross product, which is the axis of rotation to
        // get from one vector to the other
        Vector3 cross = Vector3.Cross(transform.up, angleToSpotRelativeToMe);
 
        if (cross.z < 0) {
            sideToTurnTo = -1;
        }
        else {
            sideToTurnTo = 1;
        }
		rb.AddTorque(rotationspeed * sideToTurnTo, ForceMode2D.Impulse);
		rb.angularVelocity = Mathf.Clamp(rb.angularVelocity, -maxrotationspeed, maxrotationspeed);
	}

	protected void BaseFixedUpdate ()
	{
		if (typeOfMouvement == TypeOfMouvement.mouche)
			MoucheMouvement();
	}
}
