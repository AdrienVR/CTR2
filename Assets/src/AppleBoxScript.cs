using UnityEngine;
using System.Collections;

public class AppleBoxScript : MonoBehaviour 
{
	
	void OnTriggerEnter(Collider other)
	{
		if (!other.isTrigger)
			return;
		KartController taker;
		if(Game.characters.IndexOf(other.name) != -1)// si c'est un kart
		{
			taker = (KartController)other.GetComponent ("KartController");
		}
		else if(Game.launchWeapons.IndexOf(other.name) != -1 || Game.shields.IndexOf(other.name) != -1) // si c'est une bombe ou autre
		{
			ExplosionScript es = (ExplosionScript)other.GetComponent ("ExplosionScript");
			GameObject owner = es.owner;
			taker = (KartController)owner.GetComponent ("KartController");
    	}
		else {
			return;
		}
		
		audio.Play ();
		collider.enabled = false;

		if (taker.name == null)
			return;
		taker.addApples ();
		StartCoroutine (Take());
	}
	
	IEnumerator Take()
	{
		animation.Play ("boxDisappear");
		yield return new WaitForSeconds (2f);
		animation.Play ("boxGrow");
		yield return new WaitForSeconds (1.3f);
		collider.enabled = true;
	}
}
