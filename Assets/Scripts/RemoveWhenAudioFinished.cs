using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RemoveWhenAudioFinished : MonoBehaviour
{
	public float startTimeout = .5f;
	bool checkEnd = false;

	AudioSource source;

	void Start ()
	{
		source = GetComponent<AudioSource>();
		StartCoroutine(Timeout());
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!checkEnd)
			return ;
		
		if (!source.isPlaying)
			Destroy(gameObject);
	}

	IEnumerator Timeout()
	{
		yield return new WaitForSeconds(startTimeout);
		checkEnd = true;
	}
}
