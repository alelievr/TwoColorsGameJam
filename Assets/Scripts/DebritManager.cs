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

	public int	debritCount;

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
		debrit.Agglomerate(integrity);
		debrit.onLaserReceived += OnLaserReceived;
		debrit.transform.SetParent(transform);
	}

	void OnLaserReceived(DebritController controller)
	{
		ContactFilter2D filter = new ContactFilter2D();
		filter.layerMask = 1 << LayerMask.NameToLayer("Default");
		int count = circleCollider.OverlapCollider(filter, results);

		integrity++;

		for (int i = 0; i < count; i++)
			results[i].GetComponent<DebritController>().CheckIntegryty(integrity);
		
		// Iterate over each debrits
		foreach (var debrit in debrits)
			if (debrit.integrity != integrity)
				Destroy(debrit);

		Debug.Log("Check integrity TODO !");
	}

	public int GetNewDebritIndex()
	{
		return debritCount++;
	}

	private void Update()
	{
		
	}
}