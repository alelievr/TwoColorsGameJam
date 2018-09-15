using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class NoColKaboom : MonoBehaviour
{
	public float poid = 1;
	[Range(0, 1)] public float resitimpact = 0;
	public float life = 1000;
	[HideInInspector] public Rigidbody2D rbody;

	public float		flickerCount = 4;
	public float		flickerInterval = 0.1f;

	public GameObject	invoqueondead;

	new Renderer		renderer;

	public int			debritCount = 2;
	public GameObject	debrit;

	[Space]
	public	float		recupTime;
	public	bool		recup;

	bool				dead = false;

	void Start ()
	{
		rbody = GetComponent<Rigidbody2D>();
		renderer = GetComponentInChildren< Renderer >();
		if (renderer == null)
			renderer = GetComponent< Renderer >();
	}
	
    IEnumerator Destroyation()
    {
        yield return new WaitForSeconds(0.1f);
		Destroy(gameObject);
    }

	void Die()
	{
		if (dead)
			return ;
		dead = true;

		GameObject.Instantiate(invoqueondead, transform.position, Quaternion.identity);

		for (int i = 0; i < debritCount; i++)
			NoColDebrisPool.instance.NewDebris(transform.position);
		if (tag == "boss")
			GameManager.instance.DefeatBoss();
		StartCoroutine(Destroyation());
	}

	float invudegat = 0.2f;
	IEnumerator Flicker()
	{
		for (int i = 0; i < flickerCount; i++)
		{
			renderer.enabled = false;
			yield return new WaitForSeconds(flickerInterval);
			renderer.enabled = true;
			yield return new WaitForSeconds(flickerInterval);
		}
	}

	IEnumerator Recup()
	{
		recup = true;
		yield return new WaitForSeconds(recupTime);
		recup = false;
	}

	IEnumerator HitSound()
	{
		AudioController.instance.PlayDamageAtPosition(transform.position);
		yield return new WaitForSeconds(0.2f);
		AudioController.instance.PlayDamageAtPosition(transform.position);
		yield return new WaitForSeconds(0.2f);
		AudioController.instance.PlayDamageAtPosition(transform.position);
	}	

	void ToDoOnCol(GameObject collided)
	{
		// NoColKaboom impactant = collided.GetComponent<NoColKaboom>();

		if ((!recup)/* && (impactant != null)*/)
		{
			// Vector2 realvelocity = impactant.rbody.velocity - rbody.velocity;
			
			if (collided.tag == "Player")//lol
			{
			// Debug.Log("PLAYER COLLIDED KABOOM");

				StartCoroutine(Flicker());
				StartCoroutine(Recup());
				StartCoroutine(HitSound());
				Die();
			}
			if (collided.tag == "boss")
				rbody.velocity += (Vector2)(collided.transform.position - transform.position).normalized * 2000;
		}
	}

	// void OnCollisionEnter2D(Collision2D other)
	// {
	// 	Kaboom impactant = other.gameObject.GetComponent<Kaboom>();

	// 	if ((!recup) && (impactant != null))
	// 	{
	// 		Vector2 realvelocity = impactant.rbody.velocity - rbody.velocity;
			
	// 		if ((resitimpact < 0.5f || other.gameObject.tag == "Player") && invudegat < 0) //lol
	// 		{
	// 			StartCoroutine(Flicker());
	// 			StartCoroutine(Recup());
	// 			StartCoroutine(HitSound());
	// 			life -= Mathf.Clamp(realvelocity.magnitude, 0, 30);
	// 			invudegat = 0.2f;
	// 		}
	// 		if (life < 0 && !dead)
	// 			Die();
	// 		if (other.gameObject.tag == "boss")
	// 			rbody.velocity += (Vector2)(other.transform.position - transform.position).normalized * 2000;
	// 	}
	// }

	private void FixedUpdate()
	{
		GameObject tmp;
		if (tag == "Proj" || tag == "Xenon")
		{
			if ((transform.position - GameManager.instance.playerPos).sqrMagnitude < GameManager.instance.playerSizeSqr + 30f)
			{
			//	Debug.Log("KABOOM dist to asteroid = " + (transform.position - NoColDebritManager.instance.transform.position).magnitude);
			//	Debug.DrawLine(transform.position, NoColDebritManager.instance.transform.position, Color.red, 0.1f);
				if ((tmp = NoColDebritManager.instance.DebritCollisionCheck(gameObject)) != null)
					ToDoOnCol(tmp);
			//	else
			//		Debug.Log("DEBRIT NULL");

			}
		}
	}

	private void Update()
	{
		if (invudegat >= 0)
			invudegat -= Time.deltaTime;
	}
}

