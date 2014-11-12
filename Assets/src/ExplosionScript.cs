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
	private KartController kartCollided;

	public bool disamorced = false;

	// Use this for initialization
	void Start () {
		if (Game.protectWeapons.IndexOf(name) != -1 && name != "blueShield")
			StartCoroutine (TimeToLive());
	}

	public void SetName(string n)
	{
		name = n;
	}

	public void EnablePhysics()
	{
		SphereCollider cc = GetComponent <SphereCollider> ();
		cc.enabled = true;
	}

	public void ActionExplosion()
	{
		CapsuleCollider cc = GetComponent <CapsuleCollider> ();
		cc.radius = explosionRadius;
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (lockExplosion)
			return;
		// check if other is a valid target, else return
		if((Game.boxes.IndexOf(other.name)!=-1 && Game.launchWeapons.IndexOf(name)!=-1))
			return;
		if (Game.shields.IndexOf(name)!=-1){
			KartController ownerKart = owner.GetComponent <KartController>();
			if ((ownerKart.protection != null && other.gameObject == ownerKart.protection.gameObject))
				return;
			if (Game.shields.IndexOf(other.name)!=-1){
				Destroy(other.gameObject);
				Destroy(gameObject);
			}
			else if (Game.instatiableWeapons.IndexOf(other.name) != -1)
				Destroy(gameObject);
			else if ( Game.characters.IndexOf(other.name) != -1 && other.gameObject != owner.gameObject){
				KartController toKill = other.GetComponent <KartController>();
				toKill.Die(owner, name);
				Destroy(gameObject);
			}
			return;
		}

		// explosion if bad/false collision
		if (Game.launchWeapons.IndexOf(name)!=-1 && (other.name == "Ground" || Game.poseWeapons.IndexOf(other.name)!=-1))
			StartCoroutine (Explode());
		// not a target
		if (Game.characters.IndexOf (other.name) == -1 ) {
			if (Game.poseWeapons.IndexOf(name)!=-1)
				StartCoroutine (Explode());
			return;
		}

		if (Game.protectWeapons.IndexOf (name) == -1)
			StartCoroutine (LockExplosion());

		//find the KartController target
		KartController touched = other.GetComponent <KartController>();
		kartCollided = touched;
		
		if (name == "TNT")
			if (touched.protection || touched.shield || touched.tnt || touched.state.IndexOf("invincible")!=-1)
				name = "nitro";
		// for bombs, missiles and launched shields
		if (Game.launchWeapons.IndexOf(name)!=-1) {
			if (other.gameObject != owner)
				touched.Die (owner, name);
			if (name == "bomb")
				ActionExplosion ();
			if (!exploded)
				StartCoroutine (Explode());
		}
		else if (name == "TNT"){
			StartCoroutine (Explode());
		}
		// nitro tnt, beakers
		else if (Game.poseWeapons.IndexOf(name)!=-1) {
			touched.Die (owner,name);
			StartCoroutine (Explode());
		}
	}
	
	void OnTriggerStay(Collider other)
	{
		// don't kill if it's not a target
		if (Game.characters.IndexOf (other.name) == -1)
			return;
		
		//find the KartController target
		KartController touched = other.GetComponent <KartController>();
		
		// for Aku-Aku and shields
		if (Game.protectWeapons.IndexOf (name) != -1) {
			if (other.gameObject != owner){
				touched.Die (owner,name);
			}
		}
	}

	
	void OnCollisionEnter(Collision collision)
	{
		rigidbody.velocity = new Vector3();
		if (name == "tntDropped"){
			StartCoroutine(tntExplosion());
		}

	}
	
	IEnumerator LockExplosion()
	{
		yield return new WaitForSeconds (0.01f);
		lockExplosion = true;
	}
	
	IEnumerator Explode()
	{
		exploded = true;
		owner.GetComponent <KartController>().explosiveWeapon = false;
		if (explosionClip != null)
			animation.Play (explosionClip.name);
		if (name != "TNT" || !kartCollided){
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
		else
		{
			if(kartCollided.tnt){
				kartCollided.Die (owner,name);
				Destroy(gameObject);
			}
			gameObject.transform.rotation = Quaternion.Euler(new Vector3());
			name = "tntExploded";
			gameObject.transform.rotation = kartCollided.wheels["steering"].transform.rotation;
			gameObject.transform.parent = kartCollided.wheels["steering"].transform;
			kartCollided.tnt = gameObject;
			yield return new WaitForSeconds (3f);
			if (!disamorced){
				StartCoroutine(tntExplosion());
			}
		}
	}

	IEnumerator tntExplosion(){
		audio.Play ();
		gameObject.light.color = explosionColor;
		yield return new WaitForSeconds (0.1f);
		gameObject.light.color = new Color();
		if (!disamorced)
			kartCollided.Die (owner,name);
		gameObject.transform.localScale = new Vector3 (0.01f,0.01f,0.01f);
		yield return new WaitForSeconds (3f);
		Destroy(gameObject);
	}

	public IEnumerator TimeToLive()
	{
		while(lifeTime>0f)
		{
			yield return new WaitForSeconds (0.05f);
			lifeTime -= 0.05f;
		}
		//maybe not clean but works...
		//do not delete the shield if it's launched
		if (Game.launchWeapons.IndexOf(name)==-1)
			Destroy(gameObject);
	}

	// Update is called once per frame
	void Update () {
		// for bombs, missiles and launched shields
		if (Game.launchWeapons.IndexOf(name) != -1) {
			rigidbody.velocity = new Vector3(vitesseInitiale.x,-20f,vitesseInitiale.z);
			if (exploded && name[0] == 'b')
				rigidbody.velocity = new Vector3();
		}
		// for Aku-Aku and shields
		else if (Game.protectWeapons.IndexOf(name) != -1) {
			transform.position = owner.rigidbody.transform.position + new Vector3(0f,-0.2f);
		}
		else if (name == "tntExploded") {
			if (!disamorced)
				transform.position = kartCollided.wheels["steering"].transform.position + new Vector3(0f,+0.2f) - kartCollided.forwardNormal*0.2f;
		}
	}
}