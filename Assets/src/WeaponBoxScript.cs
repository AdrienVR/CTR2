using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponBoxScript : MonoBehaviour {

	public AudioClip randomMusic;
	public AudioClip endMusic;
	private int nbImgArmes;
	private float timeLookingWeapon;

	private KartScript taker;


	void OnTriggerEnter(Collider other)
	{
		
		if (!other.isTrigger)
			return;
		//animation
		collider.enabled = false;
		audio.Play ();
		StartCoroutine (Take());
		Main.statistics.nbWeaponBox++;
		//find who to give weapon
		if(Game.characters.IndexOf(other.name) != -1)// si c'est un kart
		{
			taker = other.GetComponent <KartScript>();
		}
		else if(Game.launchWeapons.IndexOf(other.name) != -1 || Game.shields.IndexOf(other.name) != -1) // si c'est une bombe
		{
			ExplosionScript es = other.GetComponent<ExplosionScript>();
			GameObject owner = es.owner;
			taker = owner.GetComponent<KartScript>();
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
		string weapon = "bomb";
		/*
		for(int i=0;i<Game.gameWeapons.Count;i++)
		{
			Debug.Log(Game.gameWeapons[i]);
		}*/
		while (nbImgArmes < 25) {
			int rand = Random.Range (0, Game.gameWeapons.Count);
			weapon = Game.gameWeapons[rand];
			weapon = "greenShield";
			taker.GetKart().lastWeaponTextureNb = Game.GetWeaponNumber(weapon);
			taker.GetKart().drawWeaponGui();
			nbImgArmes++;
			yield return new WaitForSeconds (0.08f);
			timeLookingWeapon += 0.08f;
		}
		taker.SetWeapon(weapon);
		Main.statistics.getStatPerso (taker.GetKart ().numeroJoueur).addWeapon(weapon);
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