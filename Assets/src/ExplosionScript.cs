using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExplosionScript : MonoBehaviour {
	
	public AnimationClip explosionClip;
	public Color explosionColor;
	public Vector3 vitesseInitiale;
	public Vector3 vitesseInstant;
	public GameObject owner;
	public float lifeTime = 12.0f;
	public float explosionRadius = 3f;

	private bool isAlive;
	private bool exploded=false;

	private bool lockExplosion = false;

	// Use this for initialization
	void Start () {
		if (Dictionnaries.protectWeapons.IndexOf(name) != -1 && name != "blueShield")
			StartCoroutine (TimeToLive());
	}

	public void SetName(string n)
	{
		name = n;
	}

	public void activePhysics()
	{
		CapsuleCollider cc = (CapsuleCollider)GetComponent ("CapsuleCollider");
		cc.enabled = true;
	}

	public void ActionExplosion()
	{
		CapsuleCollider cc = (CapsuleCollider)GetComponent ("CapsuleCollider");
		cc.radius = explosionRadius;
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (lockExplosion)
			return;
		// check if other is a valid target, else return
		if((Dictionnaries.boxes.IndexOf(other.name)!=-1 && Dictionnaries.launchWeapons.IndexOf(name)!=-1))
			return;
		if (Dictionnaries.shields.IndexOf(name)!=-1){
			KartController ownerKart = (KartController)owner.GetComponent ("KartController");
			if ((ownerKart.protection != null && other.gameObject == ownerKart.protection.gameObject))
				return;
			if (Dictionnaries.shields.IndexOf(other.name)!=-1){
				Destroy(other.gameObject);
				Destroy(gameObject);
			}
			else if (Dictionnaries.instatiableWeapons.IndexOf(other.name) != -1)
				Destroy(gameObject);
			else if ( Dictionnaries.characters.IndexOf(other.name) != -1 && other.gameObject != owner.gameObject)
				Destroy(gameObject);
			return;
		}

		// explosion if bad/false collision
		if (Dictionnaries.launchWeapons.IndexOf(name)!=-1 && (other.name == "Ground" || Dictionnaries.poseWeapons.IndexOf(other.name)!=-1))
			StartCoroutine (Explode());
		// not a target
		if (Dictionnaries.characters.IndexOf (other.name) == -1 ) {
			if (Dictionnaries.poseWeapons.IndexOf(name)!=-1)
				StartCoroutine (Explode());
			return;
		}

		if (Dictionnaries.protectWeapons.IndexOf (name) == -1)
			StartCoroutine (LockExplosion());

		//find the KartController target
		KartController touched = (KartController)other.GetComponent ("KartController");

		// for bombs, missiles and launched shields
		if (Dictionnaries.launchWeapons.IndexOf(name)!=-1) {
			if (other.gameObject != owner)
				touched.Die (owner, name);
			if (name == "bomb")
				ActionExplosion ();
			if (!exploded)
				StartCoroutine (Explode());
		}
		// nitro tnt, beakers
		else if (Dictionnaries.poseWeapons.IndexOf(name)!=-1) {
			touched.Die (owner,name);
			StartCoroutine (Explode());
		}
	}
	
	void OnTriggerStay(Collider other)
	{
		// don't kill if it's not a target
		if (Dictionnaries.characters.IndexOf (other.name) == -1)
			return;
		
		//find the KartController target
		KartController touched = (KartController)other.GetComponent ("KartController");
		
		// for Aku-Aku and shields
		if (Dictionnaries.protectWeapons.IndexOf (name) != -1) {
			if (other.gameObject != owner){
				touched.Die (owner,name);
			}
		}
	}

	
	void OnCollisionEnter(Collision collision)
	{
		rigidbody.velocity = new Vector3();
	}
	
	IEnumerator LockExplosion()
	{
		yield return new WaitForSeconds (0.01f);
		lockExplosion = true;
	}
	
	IEnumerator Explode()
	{
		exploded = true;
		((KartController)owner.GetComponent ("KartController")).explosiveWeapon = false;
		if (explosionClip != null)
			animation.Play (explosionClip.name);
		audio.Play ();
		gameObject.transform.localScale = new Vector3 (0.01f,0.01f,0.01f);
		foreach (Transform child in gameObject.transform)
		{
			Destroy(child.gameObject);
		}
		gameObject.light.color = explosionColor;
		yield return new WaitForSeconds (0.1f);
		gameObject.light.color = new Color();
		yield return new WaitForSeconds (3f);
		Destroy(gameObject);
	}

	IEnumerator TimeToLive()
	{
		while(lifeTime>0f)
		{
			yield return new WaitForSeconds (0.05f);
			lifeTime -= 0.05f;
		}
		//maybe not clean but works...
		//do not delete the shield if it's launched
		if (Dictionnaries.launchWeapons.IndexOf(name)==-1)
			Destroy(gameObject);
	}

	// Update is called once per frame
	void Update () {
		// for bombs, missiles and launched shields
		if (Dictionnaries.launchWeapons.IndexOf(name) != -1) {
			rigidbody.velocity = new Vector3(vitesseInitiale.x,-20f,vitesseInitiale.z);
			if (exploded && name[0] == 'b')
				rigidbody.velocity = new Vector3();
		}
		// for Aku-Aku and shields
		else if (Dictionnaries.protectWeapons.IndexOf(name) != -1) {
			transform.position = owner.rigidbody.transform.position;
		}
	}
}