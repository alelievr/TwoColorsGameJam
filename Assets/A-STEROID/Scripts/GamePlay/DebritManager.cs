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
	Queue<DebritController>	debritdistancelist = new Queue<DebritController>();

	int integrity = 0;

	float sizeInitOfCam;

	private void Awake()
	{
		instance = this;
		debritCount = 0;
	}

	private void Start()
	{
		circleCollider = GetComponent< CircleCollider2D >();
		sizeInitOfCam = Camera.main.orthographicSize;
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "debrit")
		{
			AgglomerateDebrit(other.gameObject.GetComponent< DebritController >());
		}
	}
	
	float oldsize = -1;

	public void resizeCamera()
	{
		if (GameManager.instance.playerSize != oldsize)
			GameManager.instance.originalvcam.m_Lens.OrthographicSize = sizeInitOfCam + GameManager.instance.playerSize; // not sure about that
		oldsize = GameManager.instance.playerSize;
	}
	
	public void AgglomerateDebrit(DebritController debrit)
	{
		debrits.Add(debrit);
		debrit.Agglomerate(integrity);
		debrit.onDestroyed += OnDebritDestroyed;
		debrit.onLaserReceived += (a) => { needsIntegrityCheck = true; controller = a; };
		debrit.transform.SetParent(transform, true);
		resizeCamera();

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

		GameManager.instance.playerSizeSqr = size;
	}

	void OnDebritDestroyed(DebritController controller)
	{
		resizeCamera();
		debrits.Remove(controller);
	}

	private void Update()
	{
		if (needsIntegrityCheck)
			IntegrityCheck(controller);
	}
}