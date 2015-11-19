using UnityEngine;
using System.Collections;

public class AppleBoxScript : MonoBehaviour 
{
	
	void OnTriggerEnter(Collider other)
	{
		if (!other.isTrigger)
			return;
		KartScript taker;
		if(other.tag == "Kart")// si c'est un kart
		{
			taker = other.GetComponent<KartScript>();
		}
		else if(other.tag == "Weapon") // si c'est une bombe ou autre
		{
			WeaponBehavior es = other.GetComponent <WeaponBehavior>();
			GameObject owner = es.owner;
			taker = owner.GetComponent<KartScript>();
    	}
		else {
			return;
		}
		
		GetComponent<AudioSource>().Play ();
		GetComponent<Collider>().enabled = false;
		taker.addApples ();
		Main.statistics.nbAppleBox += 1;
		StartCoroutine (Take());
	}
	
	IEnumerator Take()
	{
		GetComponent<Animation>().Play ("boxDisappear");
		yield return new WaitForSeconds (2f);
		GetComponent<Animation>().Play ("boxGrow");
		yield return new WaitForSeconds (1.3f);
		GetComponent<Collider>().enabled = true;
	}
}
