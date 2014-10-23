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

	private bool isAlive;
	private bool exploded=false;

	private static List<string> targets = new List<string>() {"coco_prefab","crash_prefab"};

	private static List<string> boxes = new List<string>() {"weaponBox","appleBox"};
	private static List<string> launchWeapons = new List<string>() {"missile", "bomb","superBomb"};
	private static List<string> protectWeapons = new List<string>() {"Aku-Aku", "greenShield", "blueShield","superAku-Aku"};
	private static List<string> poseWeapons = new List<string>() {"nitro", "TNT", "greenBeaker", "redBeaker"};
	private static List<string> protectors = new List<string>() {"Aku-Aku","superAku-Aku", "Uka-Uka","superUka-Uka"};


	// Use this for initialization
	void Start () {
		if (protectWeapons.IndexOf(name) != -1 && name != "blueShield")
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
		cc.radius = 6.5f;
	}
	
	void OnTriggerEnter(Collider other)
	{
		// check if other is a valid target, else return
		if((boxes.IndexOf(other.name)!=-1 && launchWeapons.IndexOf(name)!=-1))
			return;
		if (other.name == "Ground")
			if (launchWeapons.IndexOf(name)!=-1)
				StartCoroutine (Explode());

		// not a target
		if (targets.IndexOf (other.name) == -1 ) {
			if (poseWeapons.IndexOf(name)!=-1)
				StartCoroutine (Explode());
			return;
		}

		//find the KartController target
		KartController touched = (KartController)other.GetComponent ("KartController");

		// for bombs, missiles and launched shields
		if (launchWeapons.IndexOf(name)!=-1) {
			if (other.gameObject != owner)
				touched.Die (owner, name);
			if (name == "bomb")
				ActionExplosion ();
			if (!exploded)
				StartCoroutine (Explode());
		}
		// nitro tnt, beakers
		else if (poseWeapons.IndexOf(name)!=-1) {
			touched.Die (owner,name);
			StartCoroutine (Explode());
		}
	}
	
	void OnTriggerStay(Collider other)
	{
		// don't kill if it's not a target
		if (targets.IndexOf (other.name) == -1)
			return;
		
		//find the KartController target
		KartController touched = (KartController)other.GetComponent ("KartController");
		
		// for Aku-Aku and shields
		if (protectWeapons.IndexOf (name) != -1) {
			if (other.gameObject != owner){
				touched.Die (owner,name);
				if (protectors.IndexOf(name)==-1)
					Destroy(gameObject);
			}
		}
	}
	
	IEnumerator Explode()
	{
		exploded = true;
		((KartController)owner.GetComponent ("KartController")).explosiveWeapon = false;
		if (explosionClip != null)
			animation.Play (explosionClip.name);
		audio.Play ();
		gameObject.transform.localScale = new Vector3 (0.01f,0.01f,0.01f);
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
		if (launchWeapons.IndexOf(name)==-1)
			Destroy(gameObject);
		((KartController)owner.GetComponent ("KartController")).coeffVitesse = ((KartController)owner.GetComponent ("KartController")).coeffVInit;
	}

	// Update is called once per frame
	void Update () {
		// for bombs, missiles and launched shields
		if (launchWeapons.IndexOf(name) != -1) {
			rigidbody.velocity = new Vector3(vitesseInitiale.x,-19.81f,vitesseInitiale.z);
			if (exploded && name[0] == 'b')
				rigidbody.velocity = new Vector3();
		}
		// for Aku-Aku and shields
		else if (protectWeapons.IndexOf(name) != -1) {
			transform.position = owner.rigidbody.transform.position;
		}
	}
}