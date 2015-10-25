using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public enum WeaponType
{
	AkuAku,
	GreenShield,
	BlueShield,
	TNT,
	Nitro,
	GreenBeaker,
	RedBeaker,
	Bomb,
	Misille,
	Turbo
}

public class ExplosionScript : MonoBehaviour {

	public WeaponType weaponType;
	public AnimationClip explosionClip;
	public Color explosionColor;
	public Vector3 vitesseInitiale;
	public Vector3 vitesseInstant;
	public GameObject owner;
	public float lifeTime = 12;
	public float explosionRadius = 3f;

	private bool isAlive;
	private bool exploded=false;

	private bool lockExplosion = false;
	private KartScript kartCollided;

	public bool disamorced = false;


	// Use this for initialization
	void Start () {
		if (name == "Aku-Aku")
		{
			AudioManager.Instance.Play("akuaku", true);
		}
		else if (name == "blueShield" || name == "greenShield")
		{
			AudioManager.Instance.Play("shieldOn");
		}
		if (Game.protectWeapons.IndexOf(name) != -1 && name != "blueShield")
			StartCoroutine (TimeToLive());
	}

	public void SetAllCollidersStatus (bool active) 
	{
		foreach(Collider c in GetComponents<Collider> ()) 
		{
			c.enabled = active;
		}
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
	
	public void BombActionExplosion()
	{
		ActionExplosion ();
		if (!exploded)
			StartCoroutine (Explode());
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.name == "Ground" && name != "tntExploded" && name != "tntDropped" && name != "tnt")
						return;
		else if (other.name == "Ground" && (name == "tntExploded" || name == "tntDropped" || name== "tnt")){
			if (name == "tntDropped")
				StartCoroutine(tntExplosion());
			else
				StartCoroutine (Explode());
		}

		if (lockExplosion)
			return;
		// check if other is a valid target, else return
		if((Game.boxes.IndexOf(other.name)!=-1 && Game.launchWeapons.IndexOf(name)!=-1))
			return;
		if (Game.shields.IndexOf(name)!=-1){
			KartScript ownerKart = owner.GetComponent <KartScript>();
			if ((ownerKart.protection != null && other.gameObject == ownerKart.protection.gameObject))
				return;
			if (Game.shields.IndexOf(other.name)!=-1){
				Destroy(other.gameObject);
				Destroy(gameObject);
			}
			else if (Game.instatiableWeapons.IndexOf(other.name) != -1)
				StartCoroutine(ShieldExplosion());
			else if ( Game.characters.IndexOf(other.name) != -1 && other.gameObject != owner.gameObject){
				KartScript toKill = other.GetComponent <KartScript>();
				toKill.Die(owner, name);
				StartCoroutine(ShieldExplosion());
			}
			return;
		}

		// explosion if bad/false collision
		if (Game.launchWeapons.IndexOf(name)!=-1 && ( other.name == "Wall" || Game.poseWeapons.IndexOf(other.name)!=-1))
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
		KartScript touched = other.GetComponent <KartScript>();
		kartCollided = touched;
		
		if (name == "TNT")
			if (touched.protection || touched.shield || touched.tnt)
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
		KartScript touched = other.GetComponent <KartScript>();
		
		// for Aku-Aku and shields
		if (Game.protectWeapons.IndexOf (name) != -1) {
			if (other.gameObject != owner){
				touched.Die (owner,name);
			}
		}
	}

	
	void OnCollisionEnter(Collision collision)
	{
		GetComponent<Rigidbody>().velocity = Vector3.zero;
		if (name == "tntDropped"){
			StartCoroutine(tntExplosion());
		}

	}
	
	IEnumerator LockExplosion()
	{
		yield return new WaitForSeconds (0.01f);
		lockExplosion = true;
	}
	
	public IEnumerator ShieldExplosion()
	{
		if (exploded)
			yield break;
		else
		{
			exploded = true;
			gameObject.GetComponent<Renderer>().enabled = false;
			
			if (name == "blueShield" || name == "greenShield")
			{
				AudioManager.Instance.Play("shieldOff");
			}
			Destroy(gameObject, 1f);
		}
	}
	
	IEnumerator Explode()
	{
		if (exploded)
		{
			yield break;
		}
		else
		{
			
			if (weaponType == WeaponType.BlueShield || weaponType == WeaponType.GreenShield)
			{
				AudioManager.Instance.Play("shieldOff");
			}
			//Debug.Log(name);
			if (name != "bomb")
				SetAllCollidersStatus (false);
			exploded = true;
			if (explosionClip != null)
				GetComponent<Animation>().Play (explosionClip.name);
			if (name != "TNT" || !kartCollided){
				GetComponent<AudioSource>().Play ();

				bool hadChildren = false;
				foreach (Transform child in gameObject.transform)
				{
					Destroy(child.gameObject);
					hadChildren = true;
				}
				if (!hadChildren)
					gameObject.GetComponent<Renderer>().enabled = false;

				gameObject.GetComponent<Light>().color = explosionColor;
				yield return new WaitForSeconds (0.1f);
				SetAllCollidersStatus (false);
				gameObject.GetComponent<Light>().color = new Color();
				Destroy(gameObject, 3f);
			}
			else
			{
				if(kartCollided.tnt){
					kartCollided.Die (owner,name);
					Destroy(gameObject);
				}
				gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
				name = "tntExploded";
				gameObject.transform.rotation = kartCollided.GetRotation();
				gameObject.transform.parent = kartCollided.GetTransform();
				kartCollided.tnt = gameObject;
				yield return new WaitForSeconds (3f);
				if (!disamorced){
					StartCoroutine(tntExplosion());
				}
			}
		}
	}

	IEnumerator tntExplosion(){
		GetComponent<AudioSource>().Play ();
		gameObject.GetComponent<Light>().color = explosionColor;
		yield return new WaitForSeconds (0.1f);
		gameObject.GetComponent<Light>().color = new Color();
		if (!disamorced)
			kartCollided.Die (owner,name);
		gameObject.GetComponent<Renderer>().enabled = false;
		Destroy(gameObject, 3f);
	}

	public IEnumerator TimeToLive()
	{
		while(lifeTime>0f)
		{
			yield return new WaitForSeconds (0.05f);
			lifeTime -= 0.05f;
		}
		if (Game.protectWeapons.IndexOf(name)!=-1){
			KartScript ownerKart = owner.GetComponent <KartScript>();
			ownerKart.protection = null;
		}

		if (name == "Aku-Aku")
		{
			AudioManager.Instance.PlayDefaultMapMusic();
		}

		//maybe not clean but works...
		//do not delete the shield if it's launched
		if (Game.launchWeapons.IndexOf(name)==-1)
			Destroy(gameObject);
		/*if(Game.protectors.IndexOf(name)>-1 && !Main.isPlayingAku())
		{
			Main.sourceMusic.clip=(AudioClip)Instantiate(Resources.Load("Audio/skullrock"));
			Main.sourceMusic.Play();
		}*/
	}

	// Update is called once per frame
	void Update () {
		// for bombs, missiles and launched shields
		if (Game.launchWeapons.IndexOf(name) != -1) {
			GetComponent<Rigidbody>().velocity = new Vector3(vitesseInitiale.x,-20f,vitesseInitiale.z);
			if (exploded && name[0] == 'b')
				GetComponent<Rigidbody>().velocity = Vector3.zero;
		}
		// for Aku-Aku and shields
		else if (Game.protectWeapons.IndexOf(name) != -1) {
			transform.position = owner.GetComponent<Rigidbody>().transform.position + new Vector3(0f,-0.2f);
		}
		else if (name == "tntExploded") {
			if (!disamorced)
				transform.position = kartCollided.GetTransform().position + new Vector3(0f,+0.2f) - kartCollided.GetForward()*0.2f;
		}
	}
}