using UnityEngine;
using System.Collections.Generic;

public class DebritManager : MonoBehaviour
{
	public static DebritManager instance;
	public float innerRadius = 3;
	public float outerRadius = 5;

	public int	debritCount;

	private void Awake()
	{
		instance = this;
		debritCount = 0;
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

	public int GetNewDebritIndex()
	{
		return debritCount++;
	}

	private void Update()
	{
		
	}
}