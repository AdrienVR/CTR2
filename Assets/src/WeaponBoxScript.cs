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

	// src : http://crashbandicoot.wikia.com/wiki/Crash_Team_Racing
	public static Dictionary <int, string> normalWeapons =  new Dictionary<int, string> {
		{1,"greenBeaker"},{2,"greenShield"},{3,"bomb"},{4,"triple_bomb"},{5,"triple_missile"},
		{6,"Aku-Aku"},{7,"TNT"},{8,"turbo"}	};
	public static Dictionary <int, string> superWeapons = new Dictionary<int, string> {
		{1,"redBeaker"},{2,"blueShield"},{3,"superBomb"},{4,"triple_superBomb"},{5,"triple_missile"},
		{6,"superAku-Aku"},{7,"nitro"},{8,"superTurbo"}	};
	private Dictionary <int, string> weapons;
	
	private static List<string> characters = new List<string>() {"coco_prefab","crash_prefab"};
	private static List<string> launchWeapons = new List<string>() {"missile", "bomb", "greenShield", "blueShield", "superBomb"};

	// Use this for initialization
	void Start () {
		weapons = normalWeapons;
	}
	void Update()
	{

		if(taker != null)
		{
			if(taker.IsSuper()) weapons=superWeapons;
			else if(!taker.IsSuper()) weapons=normalWeapons;
		}
	}
	void OnTriggerEnter(Collider other)
	{
		//animation
		collider.enabled = false;
		audio.Play ();
		StartCoroutine (Take());

		//find who to give weapon
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
			yield return new WaitForSeconds (randomMusic.length-randomMusic.length/4);
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
			nb = Random.Range (1, normalWeapons.Count);
			taker.GetKart().ws.SetTextureN(nb);
			taker.GetKart().lastWeaponTextureNb=nb;
			nbImgArmes++;
			yield return new WaitForSeconds (0.08f);
			timeLookingWeapon += 0.08f;
		}
		taker.SetWeapon(weapons[nb]);
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
