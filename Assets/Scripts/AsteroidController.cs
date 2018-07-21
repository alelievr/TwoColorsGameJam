using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class AsteroidController : MonoBehaviour {

	public float speed = 100;
	public float maxSpeed = 100;
	Vector2 dir;
	Rigidbody2D rb;
	public AudioClip boostsound;

	float boostmult = 1;
	public float timeboostins = 0.3f;
	public float boostcd = 1;
	CinemachineImpulseSource cis;
	public CinemachineVirtualCamera vcam;
	CinemachineFramingTransposer vcamframing;

	public PlayableDirector	director;
	AudioSource audios;

	// Use this for initialization
	void Start ()
	{
		audios = GetComponent<AudioSource>();
		rb = GetComponent<Rigidbody2D>();
		cis = GetComponent<CinemachineImpulseSource>();
		// vcam = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera as CinemachineVirtualCamera;
		vcamframing = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
	}
	
	Vector2 Directionator()
	{
		Vector2 tmp = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		return tmp;
	}

	IEnumerator boost()
    {
		director.Play();
		float tmptime = timeboostins;
		boostcdtmp = boostcd;
		if (boostsound)
			audios.PlayOneShot(boostsound);
		cis.GenerateImpulse(rb.velocity);
		// vcamframing.m_XDamping = 1.5f;
        // vcamframing.m_YDamping = 1.5f;
        // vcamframing.m_ZDamping = 1.5f;
		
		boostmult = 2;
		while (tmptime > 0)
		{
			tmptime -= Time.deltaTime;
			// vcamframing.m_LookaheadTime = tmptime / timeboostins * 0.05f;
			// vcamframing.m_XDamping = tmptime / timeboostins * 1.5f;
			// vcamframing.m_YDamping = tmptime / timeboostins * 1.5f;
			// vcamframing.m_ZDamping = tmptime / timeboostins * 1.5f;
			yield return new WaitForEndOfFrame();
		}
		// vcamframing.m_LookaheadTime = 0.05f;
		// vcamframing.m_XDamping = 0;
        // vcamframing.m_YDamping = 0;
        // vcamframing.m_ZDamping = 0;
		boostmult = 1;
    }

	float boostcdtmp = -1;

	private void Update()
	{
		if (boostcdtmp >= 0)
			boostcdtmp -= Time.deltaTime;
		dir = Directionator().normalized;
		if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0)
		|| Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.LeftControl)) && boostcdtmp < 0)
			StartCoroutine(boost());
	}
	// Update is called once per frame
	void FixedUpdate () {
		//Debug.Log("hor" + Input.GetAxisRaw("Horizontal")+ "ver" + Input.GetAxisRaw("Vertical"));
		Debug.Log(rb.velocity.magnitude);
		rb.AddForce(dir * speed * boostmult, ForceMode2D.Force);
		if (Mathf.Abs(rb.velocity.magnitude) > maxSpeed * boostmult)
		{
			Vector2 tmp = new Vector2(rb.velocity.x, rb.velocity.y);
			tmp = tmp.normalized * maxSpeed * boostmult;
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
