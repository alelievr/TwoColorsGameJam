using UnityEngine;
using System.Collections.Generic;

public class DebritManager : MonoBehaviour
{
	public static DebritManager instance;
	public float innerRadius = 3;
	public float outerRadius = 5;

	CircleCollider2D	circleCollider;
	
	Collider2D[]		results = new Collider2D[16];

	List< DebritController > debrits = new List< DebritController >();

	public int				debritCount;
	bool					needsIntegrityCheck;
	DebritController		controller;
	Queue<DebritController>	debritdistancelist;
	Queue<float>			DistanceMaxOfAglo = new Queue<float>();

	int integrity = 0;

	private void Awake()
	{
		instance = this;
		debritCount = 0;
	}

	private void Start()
	{
		circleCollider = GetComponent< CircleCollider2D >();
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "debrit")
		{
			AgglomerateDebrit(other.gameObject.GetComponent< DebritController >());
		}
	}
	
	public void AgglomerateDebrit(DebritController debrit)
	{
		float tmp;
		if (tmp = Vector2.Distance(transform.position, other.position) > 0)
		{
			debritdistancelist.Enqueue(debrit);
			DistanceMaxOfAglo = tmp;
		}
		debrits.Add(debrit);
		debrit.Agglomerate(integrity);
		debrit.onDestroyed += OnDebritDestroyed;
		debrit.onLaserReceived += (a) => { needsIntegrityCheck = true; controller = a; };
		debrit.transform.SetParent(transform, true);

		UpdatePlayerSize();
	}

	void IntegrityCheck(DebritController controller)
	{
		int count = Physics2D.OverlapCircleNonAlloc(transform.position, circleCollider.radius + 0.1f, results);

		integrity++;

		for (int i = 0; i < count; i++)
		{
			var debrit = results[i].GetComponent<DebritController>();

			if (debrit == null)
				continue ;

			debrit.CheckIntegryty(integrity);
		}
		
		// Iterate over each debrits
		foreach (var debrit in debrits)
			if (debrit.integrity != integrity)
			{
				debrit.OnWillBeDestroyed();
				Destroy(debrit.gameObject);
			}
			
		needsIntegrityCheck = false;
		UpdatePlayerSize();
	}

	void UpdatePlayerSize()
	{
		float size = 0;
		foreach (var debrit in debrits)
		{
			size = Mathf.Max(size, Vector3.Distance(debrit.transform.position, transform.position));
		}

		GameManager.instance.playerSize = size;
	}

	void OnDebritDestroyed(DebritController controller)
	{
		if (debritdistancelist.Peek() == controller)
		{
			debritdistancelist.Dequeue();
			while (debritdistancelist.Peek() == null && debritdistancelist.count > 0)
				debritdistancelist.Dequeue();
			Vector2.Distance(transform.position, debritdistancelist.Peek().position);
		}
		debrits.Remove(controller);
	}

	private void Update()
	{
		if (needsIntegrityCheck)
			IntegrityCheck(controller);
	}
}