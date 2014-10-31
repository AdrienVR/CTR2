using UnityEngine;
using System.Collections;

public class AppleBoxScript : MonoBehaviour {
	private bool isGiving = false;
	// Use this for initialization
	void Start () {
	}
	
	void OnTriggerEnter(Collider other)
	{
		KartController taker;
		if (isGiving)
			return;
		isGiving = true;
		collider.enabled = false;
		audio.Play ();
		if(Dictionnaries.characters.IndexOf(other.name) != -1)// si c'est un kart
		{
			taker = (KartController)other.GetComponent ("KartController");
		}
		else if(Dictionnaries.launchWeapons.IndexOf(other.name) != -1) // si c'est une bombe
		{
			ExplosionScript es = (ExplosionScript)other.GetComponent ("ExplosionScript");
			GameObject owner = es.owner;
			taker = (KartController)owner.GetComponent ("KartController");
		}
		else return;
		if (taker.name == null)
			return;
		taker.addApples ();
		StartCoroutine (Take());
		StartCoroutine(animPommes());
	}
	
	IEnumerator animPommes()
	{
		yield return new WaitForSeconds (0.1f);
	}
	
	IEnumerator Take()
	{
		animation.Play ("boxDisappear");
		yield return new WaitForSeconds (2f);
		isGiving = false;
		animation.Play ("boxGrow");
		yield return new WaitForSeconds (1.3f);
		collider.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
