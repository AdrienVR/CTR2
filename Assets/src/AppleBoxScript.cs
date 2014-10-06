using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AppleBoxScript : MonoBehaviour {
	private KartController taker;
	private static List<string> characters = new List<string>() {"coco_prefab","crash_prefab","crash_prefab(Clone)"};
	private static List<string> launchWeapons = new List<string>() {"missile", "missile(Clone)", "bomb", "bomb(Clone)"};
	// Use this for initialization
	void Start () {
	}
	
	void OnTriggerEnter(Collider other)
	{
		collider.enabled = false;
		audio.Play ();
		if(characters.IndexOf(other.name) != -1)// si c'est un kart
		{
			taker = (KartController)other.GetComponent ("KartController");
		}
		else if(launchWeapons.IndexOf(other.name) != -1) // si c'est une bombe
		{
			ExplosionScript es = (ExplosionScript)other.GetComponent ("ExplosionScript");
			GameObject owner = es.owner;
			taker = (KartController)owner.GetComponent ("KartController");
		}
		else return;
		if (taker.name == null)
			return;
		taker.GetKart().addApples ();
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
		yield return new WaitForSeconds (3f);
		animation.Play ("boxGrow");
		yield return new WaitForSeconds (2f);
		collider.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
