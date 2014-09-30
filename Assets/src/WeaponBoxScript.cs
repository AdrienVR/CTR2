using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponBoxScript : MonoBehaviour {

	public AudioClip randomMusic;
	public AudioClip endMusic;

	private int nbImgArmes=0;
	private KartController taker;

	// src : http://crashbandicoot.wikia.com/wiki/Crash_Team_Racing
	private static Dictionary <int, string> normalWeapons =  new Dictionary<int, string> {
		{1,"greenBeaker"},{2,"greenShield"},{3,"bomb"},{4,"tripleBombs"},{5,"tripleMissiles"},
		{6,"Aku-Aku"},{7,"TNT"},{8,"turbo"}	};
	private static Dictionary <int, string> superWeapons = new Dictionary<int, string> {
		{1,"redBeaker"},{2,"redShield"},{3,"superBomb"},{4,"superTripleBombs"},{5,"superTripleMissiles"},
		{6,"superAku-Aku"},{7,"nitro"},{8,"superTurbo"}	};

	// Use this for initialization
	void Start () {
	}

	void OnTriggerEnter(Collider other)
	{
		collider.enabled = false;
		audio.Play ();

		StartCoroutine (Take());
		if (other.name[0] != 'c')
						return;
		taker = (KartController)other.GetComponent ("KartController");
		if (taker.name == null)
						return;
		if (taker.IsArmed ())
			return;
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

	IEnumerator AnimArmes()
	{
		if (nbImgArmes < 25) {
			float nb = Random.Range (1, 15);
			GameObject Arme = Instantiate (Resources.Load ("arme" + ((int)nb).ToString ()), new Vector3 (0.025f, 0.55f, 0), Quaternion.identity) as GameObject;
			nbImgArmes++;
			yield return new WaitForSeconds (0.08f);
			Destroy (Arme);
			StartCoroutine (AnimArmes2 ());
		} 
		else 
			nbImgArmes = 0;
		yield return new WaitForSeconds(0.01f);
	}

	IEnumerator AnimArmes2()
	{
		if(nbImgArmes<25)
		{
			float nb = Random.Range(1, 15);
			GameObject Arme = Instantiate(Resources.Load("arme"+((int)nb).ToString()), new Vector3(0.025f,0.55f,0), Quaternion.identity) as GameObject;
			nbImgArmes++;
			yield return new WaitForSeconds(0.08f);
			Destroy (Arme);
			StartCoroutine(AnimArmes());
		}
		else
		{
			float nb = Random.Range(1, 15);
			GameObject Arme = Instantiate(Resources.Load("arme"+((int)nb).ToString()), new Vector3(0.025f,0.55f,0), Quaternion.identity) as GameObject;
			taker.SetWeapon("bomb");
			nbImgArmes = 0;
		}
		yield return new WaitForSeconds(0.01f);
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
