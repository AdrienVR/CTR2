using UnityEngine;
using System.Collections;

public class AppleBoxScript : MonoBehaviour 
{

	void Start()
	{
		m_selfCollider = GetComponent<Collider> ();
	}

	void OnTriggerEnter(Collider other)
	{
		PlayerController taker;
		if(other.tag == "Kart")// si c'est un kart
		{
			taker = other.GetComponent<PlayerController>();
		}
		else if(other.tag == "Weapon") // si c'est une bombe ou autre
		{
			WeaponBehavior es = other.GetComponent <WeaponBehavior>();
			GameObject owner = es.owner;
			taker = owner.GetComponent<PlayerController>();
    	}
		else
		{
			return;
		}
		
		GetComponent<AudioSource>().Play ();
		m_selfCollider.enabled = false;
		int n = Random.Range(4, 8);
		taker.ChangeApples (n);
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
	
	private Collider m_selfCollider;
}
