using UnityEngine;
using System.Collections;

public class WeaponBoxScript : MonoBehaviour {

	public AudioClip randomMusic;
	public AudioClip endMusic;

	private bool exited=true;
	private int nbImgArmes=0;
	private KartController taker;
	private float delay;

	// Use this for initialization
	void Start () {
	}

	void OnTriggerEnter(Collider other)
	{
		exited = false;
		if (animation.isPlaying || delay > 0)
			return;
		audio.Play ();
		delay += 3f;
		
		StartCoroutine (ComputeTime());
		StartCoroutine (Take());
//		Debug.Log ("j'ai touché une caisse");
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
			yield return new WaitForSeconds (randomMusic.length);
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
	}

	IEnumerator ComputeTime()
	{
		while (delay>0f) {
			yield return new WaitForSeconds (0.1f);
			if (exited)
				delay -= 0.1f;
		}
	}

	void OnTriggerExit(Collider other)
	{
		exited = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
