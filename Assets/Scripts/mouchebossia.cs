using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouchebossia : MonoBehaviour
{
	public bool instanfind = false;
	public GameObject cible = null;
	public float speed = 30;
	public float maxspeed = 20;
	public float rotationspeed = 30;
	public float maxrotationspeed = 20;
	public float idealdistancetocible = 10;
	public	Animator anim;
	// Use this for initialization

	Rigidbody2D rb;

	void Start () {
		rb = GetComponent<Rigidbody2D>();
		if (instanfind == true)
			cible = GameManager.instance.player;
	}

	void Update () {
		if (Input.GetKey(KeyCode.Space)) {
			anim.SetTrigger("hit"); 
		}
	}
	
	void FixedUpdate () {
		if (cible == null)
			return ;
		Vector2 idealpostmp = transform.position + (cible.transform.position - transform.position).normalized * idealdistancetocible;
		rb.AddForce(((Vector2)cible.transform.position - idealpostmp).normalized * speed, ForceMode2D.Force);
		if (Mathf.Abs(rb.velocity.magnitude) > maxspeed)
		{
			Vector2 tmp = new Vector2(rb.velocity.x, rb.velocity.y);
			tmp = tmp.normalized * maxspeed;
			rb.velocity = tmp;
		}
		        int sideToTurnTo = 0;
       
        Vector3 angleToSpotRelativeToMe = cible.transform.position - transform.position;
       
        //get the angle between transform's "forward" and target delta
        float angleDiff = Vector3.Angle(transform.up, angleToSpotRelativeToMe);
 
        // get its cross product, which is the axis of rotation to
        // get from one vector to the other
        Vector3 cross = Vector3.Cross(transform.up, angleToSpotRelativeToMe);
 
        if (cross.z < 0) {
            sideToTurnTo = -1;
        }
        else {
            sideToTurnTo = 1;
        }
		rb.AddTorque(rotationspeed * sideToTurnTo);
		rb.angularVelocity = Mathf.Clamp(rb.angularVelocity, -maxrotationspeed, maxrotationspeed);
	}
}
