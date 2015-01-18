using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponBoxScript : MonoBehaviour {

	public AudioClip randomMusic;
	public AudioClip endMusic;
	private int nbImgArmes;
	private float timeLookingWeapon;

	private KartController taker;

	public bool baddie = false;
	
	void OnTriggerEnter(Collider other)
	{
		if (other.isTrigger == false)
			return;

		collider.enabled = false;
		audio.Play ();
		StartCoroutine (Take());

		//find who to give weapon
		if(Game.characters.IndexOf(other.name) != -1)// si c'est un kart
		{
			taker = (KartController)other.GetComponent ("KartController");
		}
		else if(Game.launchWeapons.IndexOf(other.name) != -1 || Game.shields.IndexOf(other.name) != -1) // si c'est une bombe
		{
			GameObject owner;
			if (other.name!="bomb"){
				ExplosionScript es = other.GetComponent<ExplosionScript>();
				owner = es.owner;
			}
			else{
				Bomb es = other.GetComponent<Bomb>();
				owner = es.owner;
			}
			taker = owner.GetComponent<KartController>();
		}
		else return;

		if (taker.name == null)
			return;
		if (taker.IsArmed() || taker.IsWaitingWeapon())
			return;
		taker.setWaitingWeapon (true);
		taker.SetWeaponBox(this);
		//animation of giving weapon
		StartCoroutine(AnimArmes());
		StartCoroutine(PlaySound());
	}
	
	IEnumerator PlaySound()
	{
		while (nbImgArmes<25 && nbImgArmes>0) {
			audio.PlayOneShot(randomMusic);
			yield return new WaitForSeconds (randomMusic.length*3/4);
		}
		audio.PlayOneShot(endMusic);
	}

	public bool selectRandomWeapon()
	{
		if(timeLookingWeapon>1f)
			nbImgArmes = 25;
		else 
			return false;
		return true;
	}

	IEnumerator AnimArmes()
	{
		nbImgArmes = 0;
		timeLookingWeapon = 0;
		int nb = 1;
		while (nbImgArmes < 25) {
			nb = Random.Range (1, Game.normalWeapons.Count+1);
			//{1,"redBeaker"},{2,"blueShield"},{3,"bomb"},{4,"triple_bomb"},{5,"triple_missile"},
			//{6,"Aku-Aku"},{7,"nitro"},{8,"turbo"}	};
			//nb = 4;
			taker.GetKart().lastWeaponTextureNb=nb;
			taker.GetKart().drawWeaponGui();
			nbImgArmes++;
			yield return new WaitForSeconds (0.08f);
			timeLookingWeapon += 0.08f;
		}
		if (!taker.IsSuper())
			taker.SetWeapon(Game.normalWeapons[nb]);
		else
			taker.SetWeapon(Game.superWeapons[nb]);
		taker.setWaitingWeapon (false);
		nbImgArmes = 0;
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