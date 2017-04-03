using UnityEngine;
using System.Collections;

public class AppleBoxScript : MonoBehaviour 
{
	void Start()
	{
		m_selfCollider = GetComponent<Collider> ();
        m_animation = GetComponent<Animation>();
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
			taker = es.Owner;
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

        m_animation.Play ("boxDisappear");
		yield return new WaitForSeconds (2f);
        m_animation.Play ("boxGrow");
		yield return new WaitForSeconds (1.3f);
        m_selfCollider.enabled = true;
	}

    private Animation m_animation;
	private Collider m_selfCollider;
}
