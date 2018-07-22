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

	public int			debritCount;
	bool				needsIntegrityCheck;
	DebritController	controller;

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

	public Vector3 GetDebritPosition(int index)
	{
		index++;

		const float Phi = 1.6180339887498948482045868343656f;
		const float  dA = Phi / Mathf.PI;
		float size = Mathf.Sqrt(debritCount) * dA;
		float Angle = dA + ((Phi - 1) * 2 * Mathf.PI) * index;
		float r = ((Mathf.Sqrt(index) * 1 * dA) / size); //radius
		Vector3 pos = new Vector3(r * Mathf.Cos(Angle), r * Mathf.Sin(Angle), 0);

		pos = pos * outerRadius + pos.normalized * innerRadius;

		return pos + transform.position;
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
		debrits.Add(debrit);
		debrit.Agglomerate(integrity);
		debrit.onDestroyed += OnDebritDestroyed;
		debrit.onLaserReceived += (a) => { needsIntegrityCheck = true; controller = a; };
		debrit.transform.SetParent(transform);

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
		debrits.Remove(controller);
	}

	public int GetNewDebritIndex()
	{
		return debritCount++;
	}

	private void Update()
	{
		if (needsIntegrityCheck)
			IntegrityCheck(controller);
	}
}