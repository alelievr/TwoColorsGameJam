using UnityEngine;
using System.Collections.Generic;

public class NoColDebritManager : MonoBehaviour
{
	public static NoColDebritManager instance;
	public float innerRadius = 3;
	public float outerRadius = 5;

	CircleCollider2D	circleCollider;
	
	Collider2D[]		results = new Collider2D[16];

	List< NoColDebritController > debrits = new List< NoColDebritController >();

	public int				debritCount;
	bool					needsIntegrityCheck;
	NoColDebritController		controller;
	Queue<NoColDebritController>	debritdistancelist = new Queue<NoColDebritController>();

	[HideInInspector]
	public float			DistanceMaxOfAglo = 0;

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
			AgglomerateDebrit(other.gameObject.GetComponent< NoColDebritController >());
		}
	}
	
	public void resizeCamera()
	{

	}
	
	public void AgglomerateDebrit(NoColDebritController debrit)
	{
		float tmp;
		if ((tmp = Vector2.Distance(transform.position, debrit.transform.position)) > DistanceMaxOfAglo)
		{
			debritdistancelist.Enqueue(debrit);
			DistanceMaxOfAglo = tmp;
			resizeCamera();
		}
		debrits.Add(debrit);
		debrit.Agglomerate(integrity);
		debrit.onDestroyed += OnDebritDestroyed;
		debrit.onLaserReceived += (a) => { needsIntegrityCheck = true; controller = a; };
		debrit.transform.SetParent(transform, true);

		UpdatePlayerSize();
	}

	void IntegrityCheck(NoColDebritController controller)
	{
		int count = Physics2D.OverlapCircleNonAlloc(transform.position, circleCollider.radius + 0.1f, results);

		integrity++;

		for (int i = 0; i < count; i++)
		{
			var debrit = results[i].GetComponent<NoColDebritController>();

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

	void OnDebritDestroyed(NoColDebritController controller)
	{
		if (debritdistancelist.Count > 0 && debritdistancelist.Peek() == controller)
		{
			debritdistancelist.Dequeue();
			while (debritdistancelist.Count > 0 && debritdistancelist.Peek() == null)
				debritdistancelist.Dequeue();
			DistanceMaxOfAglo = (debritdistancelist.Count > 0) ?
				Vector2.Distance(transform.position, debritdistancelist.Peek().transform.position) : 0;
			resizeCamera();

		}
		debrits.Remove(controller);
	}

	private void Update()
	{
		if (needsIntegrityCheck)
			IntegrityCheck(controller);
	}
}