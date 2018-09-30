using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NoColDebrisController : MonoBehaviour
{
    public GameObject player;

    [Header("Target reach settings")]
    public float toVel = 2.5f;
    public float maxVel = 15.0f;
    public float maxForce = 40.0f;
    public float gain = 5f;

    public event Action<NoColDebrisController> onLaserReceived;
    public event Action<NoColDebrisController> onDestroyed;

    public GameObject debritExplosionPrefab;

    Rigidbody2D rb;
    CircleCollider2D circleCollider;
    Collider2D[] results = new Collider2D[16];
    NoColDebrisManager manager;
    public bool agglomerationEnabled;

    public int integrity = 0;

    bool dead = false;

    List<NoColDebrisController> touchingDebrits = new List<NoColDebrisController>();

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        manager = NoColDebrisManager.instance;

        StartCoroutine("Killme");
    }

    public void Agglomerate(int integrity)
    {
		// Debug.Log("AGGLO PRE RETURN");
        if (agglomerationEnabled)
            return;
		// Debug.Log("AGGLO PASS RETURN");
        // AudioController.instance.PlayAggregateOnPlayer();

        tag = "Player";

        this.integrity = integrity;

        Destroy(rb);

        ContactFilter2D filter = new ContactFilter2D();
        int count = circleCollider.OverlapCollider(filter, results);

        for (int i = 0; i < count; i++)
            touchingDebrits.Add(results[i].GetComponent<NoColDebrisController>());

        integrity = 0;
        StopCoroutine("Killme");
        agglomerationEnabled = true;
    }

    private void FixedUpdate()
    {
        
        if (tag == "debrit")
        {
            if ((transform.position - GameManager.instance.playerPos).sqrMagnitude < GameManager.instance.playerSizeSqr + 10f)
            {
//                	Debug.Log("UNDER THE INFLUENCE");
                Debug.DrawLine(transform.position, NoColDebrisManager.instance.transform.position, Color.green, Time.fixedDeltaTime);
                GameObject tmp;
                if ((tmp = manager.DebritCollisionCheck(transform.position)) != null)
                    ToDoOnCol(tmp);
            }
        }
    }

    void Update()
    {
        // if (agglomerationEnabled)
        // Debug.DrawLine(transform.position, NoColDebrisManager.instance.transform.position, Color.blue, Time.deltaTime);
    }

    public void CheckIntegryty(int newIntegrity)
    {
        if (newIntegrity == integrity)
            return;

        integrity = newIntegrity;

        foreach (var touchingDebrit in touchingDebrits)
        {
            // Safety check
            if (touchingDebrit != null)
                touchingDebrit.CheckIntegryty(newIntegrity);
        }
    }

    private void ToDoOnCol(GameObject Collided)
    {
        //	Debug.Log("ON A TOUCHER UN TRUC deb cont = " + Collided.tag);
        if (agglomerationEnabled)
            return;
        //Debug.Log("AGGLO PASSED");
        // if (Collided.tag == "debrit")
        // {
        // 	Debug.Log("debrit COLLIDED deb cont");
        // 	var otherDebrit = Collided.GetComponent<NoColDebrisController>();
        // 	manager.AgglomerateDebrit(otherDebrit);
        // 	touchingDebrits.Add(otherDebrit);
        // }
        if (Collided.tag == "Player")
        {
      //      Debug.Log("PLAYER COLLIDED  deb cont");
            var otherDebrit = Collided.GetComponent<NoColDebrisController>();
            manager.AgglomerateDebrit(this);
            if (otherDebrit != null)
				otherDebrit.touchingDebrits.Add(this);
        }
    }

    // private void OnCollisionEnter2D(Collision2D other)
    // {
    // 	if (!agglomerationEnabled)
    // 		return ;

    // 	if (other.gameObject.tag == "debrit")
    // 	{
    // 		var otherDebrit = other.gameObject.GetComponent<NoColDebrisController>();
    // 		manager.AgglomerateDebrit(otherDebrit);
    // 		touchingDebrits.Add(otherDebrit);
    // 	}
    // }

    public void ToDoWhenLaserHit(LaserBehaviour laser)
    {
        if (agglomerationEnabled && onLaserReceived != null)
            onLaserReceived(this);
        OnWillBeDestroyed();
        NoColDebrisPool.instance.FreeDebris(this);
        LaserPool.instance.FreeLaser(laser);
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    // 	if (other.tag == "Laser")
    // 	{
    // 		if (agglomerationEnabled && onLaserReceived != null)
    // 			onLaserReceived(this);
    // 		OnWillBeDestroyed();
    // 		NoColDebrisPool.instance.FreeDebris(this);
    // 		LaserPool.instance.FreeLaser(other.GetComponent<LaserBehaviour>());

    // 	}
    // }

    IEnumerator Killme()
    {
        yield return new WaitForSeconds(10f);
        NoColDebrisPool.instance.FreeDebris(this);
    }

    public void OnWillBeDestroyed()
    {
        if (dead)
            return;

        dead = true;
        Instantiate(debritExplosionPrefab, transform.position, Quaternion.identity);

        // AudioController.instance.PlayExplosionAtPosition(transform.position);
    }

    private void OnDestroy()
    {
        foreach (var debrit in touchingDebrits)
        {
            if (debrit != null)
                debrit.touchingDebrits.Remove(this);
        }

        if (onDestroyed != null)
            onDestroyed(this);
    }
}
