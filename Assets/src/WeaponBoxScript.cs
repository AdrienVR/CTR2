using UnityEngine;
using System.Collections;

public class WeaponBoxScript : MonoBehaviour {

	private bool exited=true;
	private int nbImgArmes=0;

	// Use this for initialization
	void Start () {
	}

	void OnTriggerEnter(Collider other)
	{
		exited = false;
		if (animation.isPlaying)
			return;
		audio.Play ();
		
		StartCoroutine (Take());
//		Debug.Log ("j'ai touché une caisse");
		StartCoroutine(animArmes());
	}

	IEnumerator animArmes()
	{
		if(nbImgArmes<25)
		{
			float nb = Random.Range(1, 15);
			GameObject Arme = Instantiate(Resources.Load("arme"+((int)nb).ToString()), new Vector3(0.025f,0.55f,0), Quaternion.identity) as GameObject;
			nbImgArmes++;
			yield return new WaitForSeconds(0.08f);
			Destroy (Arme);
			StartCoroutine(animArmes2());
		}
		yield return new WaitForSeconds(0.01f);
	}
	IEnumerator animArmes2()
	{
		if(nbImgArmes<25)
		{
			float nb = Random.Range(1, 15);
			GameObject Arme = Instantiate(Resources.Load("arme"+((int)nb).ToString()), new Vector3(0.025f,0.55f,0), Quaternion.identity) as GameObject;
			nbImgArmes++;
			yield return new WaitForSeconds(0.08f);
			Destroy (Arme);
			StartCoroutine(animArmes());
		}
		else
		{
			float nb = Random.Range(1, 15);
			GameObject Arme = Instantiate(Resources.Load("arme"+((int)nb).ToString()), new Vector3(0.025f,0.55f,0), Quaternion.identity) as GameObject;
		}
		yield return new WaitForSeconds(0.01f);
	}

	
	IEnumerator Take()
	{
		animation.Play ("boxDisappear");
		yield return new WaitForSeconds (3f);
		while (!exited)
			yield return new WaitForSeconds (0.2f);
		animation.Play ("boxGrow");
	}

	void OnTriggerExit(Collider other)
	{
		exited = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
