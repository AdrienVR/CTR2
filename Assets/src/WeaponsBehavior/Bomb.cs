using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bomb : MonoBehaviour {

	// object pointers
	public GameObject owner;
	private KartController kartCollided;

	public Color explosionColor;
	public Vector3 vitesseInitiale;
	public float explosionRadius = 3f;

	private bool exploded=false;
	private bool lockExplosion = false;

	// Use this for initialization
	void Start () {
	}

	public void ActionExplosion()
	{
		GetComponent <CapsuleCollider> ().radius = 3;
		
		if (!exploded)
			StartCoroutine (Explode());
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.isTrigger == false)
			return;
		else if (other.name == "Ground_trigger"){
			ActionExplosion();
			return;
		}
		Debug.Log ("n" + other.name);
		if (lockExplosion)
			return;

		if (Game.listKarts.IndexOf(other.name)!=-1){
			//find the KartController target
			KartController touched = other.GetComponent <KartController>();
			kartCollided = touched;
			if (other.gameObject != owner)
				touched.Die (owner, name);
			if (!exploded)
				ActionExplosion ();
		}
	}
	
	IEnumerator LockExplosion()
	{
		yield return new WaitForSeconds (0.01f);
		lockExplosion = true;
		GetComponent <CapsuleCollider> ().enabled = false;
		Debug.Log ("Locked");
	}
	
	IEnumerator Explode()
	{
		StartCoroutine (LockExplosion ());
		exploded = true;
		owner.GetComponent <KartController>().explosiveWeapon = false;
		audio.Play ();
		gameObject.transform.localScale = new Vector3 (0.001f,0.001f,0.001f);
		gameObject.light.color = explosionColor;
		yield return new WaitForSeconds (0.1f);
		gameObject.light.color = new Color();
		yield return new WaitForSeconds (audio.clip.length);
		Destroy(gameObject);
	}

	// Update is called once per frame
	void Update () {
		rigidbody.velocity = new Vector3(vitesseInitiale.x,-20f,vitesseInitiale.z);
		if (exploded && name[0] == 'b')
			rigidbody.velocity = new Vector3();
	}
}