using UnityEngine;
using System.Collections;

public class AppleBoxScript : MonoBehaviour {
	
	private bool exited=true;
	
	// Use this for initialization
	void Start () {
	}
	
	void OnTriggerEnter(Collider other)
	{
		exited = false;
		if (animation.isPlaying)
			return;
		audio.Play ();
		
		StartCoroutine (Take());
		//		Debug.Log ("j'ai touché une caisse");
		StartCoroutine(animPommes());
	}
	
	IEnumerator animPommes()
	{
		yield return new WaitForSeconds (0.1f);
	}
	
	IEnumerator Take()
	{
		animation.Play ("boxDisappear");
		yield return new WaitForSeconds (3f);
		while (!exited)
			yield return new WaitForSeconds (0.2f);
		animation.Play ("boxGrow");
	}
	
	void OnTriggerExit(Collider other)
	{
		exited = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
