using UnityEngine;
using System.Collections;

public class ExplosionScript : MonoBehaviour {
	
	public AnimationClip explosionClip;
	public Color explosionColor;
	// Use this for initialization
	void Start () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		StartCoroutine (Explode());
	}

	IEnumerator Explode()
	{
		if (explosionClip != null)
			animation.Play (explosionClip.name);
		gameObject.light.color = explosionColor;
		yield return new WaitForSeconds (0.1f);
		Destroy(gameObject);
	}

	
	void OnTriggerExit(Collider other)
	{
	}

	// Update is called once per frame
	void Update () {
	
	}
}
