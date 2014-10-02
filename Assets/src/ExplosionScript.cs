using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExplosionScript : MonoBehaviour {
	
	public AnimationClip explosionClip;
	public Color explosionColor;
	public Vector3 vitesseInitiale;
	public GameObject owner;
	public bool isAlive;
	private bool dansLesAirs = true;
	private bool exploded=false;
	private float lifeTime = 12.0f;
	public static List<string> targets = new List<string>() {"coco_prefab","crash_prefab","crash_prefab(Clone)"};
	public static List<string> boxes = new List<string>() {"weaponBox","appleBox"};
	private static List<string> launchWeapons = new List<string>() {"missile", "missile(Clone)", "bomb", "bomb(Clone)"};
	private static List<string> protectWeapons = new List<string>() {"Aku-Aku", "Aku-Aku(Clone)", "greenShield", "greenShield(Clone)"};
	// Use this for initialization
	void Start () {
		if (protectWeapons.IndexOf(name) != -1)
			StartCoroutine (TimeToLive());

	}
	
	
	public void ActionExplosion()
	{
		if (name[0] == 'b') {
			CapsuleCollider cc = (CapsuleCollider)GetComponent ("CapsuleCollider");
			cc.radius = 6.5f;
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		if((boxes.IndexOf(other.name)!=-1 && launchWeapons.IndexOf(name)!=-1) || protectWeapons.IndexOf(name) != -1)
			return;
		Debug.Log(other.name + "has been destroyed mouahaha !");
		if (name [0] == 'b' && exploded) {
				}
		else
			StartCoroutine (Explode());
		if (targets.IndexOf (other.name) == -1)
			return;
		KartController touched = (KartController)other.GetComponent ("KartController");
		if(touched.gameObject!=owner.gameObject &&  touched.state.IndexOf("invincible") == -1)
		{
			((KartController)owner.GetComponent ("KartController")).GetKart().AddPoint ();
		}
		if (other.gameObject != owner)
			touched.Die ();
		ActionExplosion ();
	}
	
	IEnumerator Explode()
	{
		((KartController)owner.GetComponent ("KartController")).explosiveWeapon = false;
		if (explosionClip != null)
			animation.Play (explosionClip.name);
		audio.Play ();
		exploded = true;
		gameObject.transform.localScale = new Vector3 ();
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
		Destroy(gameObject);
		
	}
	
	void OnCollisionStay(Collision collision)
	{
		if(collision.gameObject.name=="Ground")
			dansLesAirs = false;
	}
	
	void OnCollisionExit(Collision collision)
	{
		if(collision.gameObject.name=="Ground")
		{
			dansLesAirs = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (launchWeapons.IndexOf(name) != -1)
		{
			if (rigidbody != null)
				rigidbody.velocity = -((KartController)owner.GetComponent ("KartController")).facteurSens*rigidbody.transform.forward*50f;
			if (dansLesAirs)
				rigidbody.velocity = new Vector3(rigidbody.velocity.x,-9.81f,rigidbody.velocity.z);
			if (exploded && name[0] == 'b')
				rigidbody.velocity = new Vector3();
		}
		else if (protectWeapons.IndexOf(name) != -1)
		{
			transform.position = owner.rigidbody.transform.position;
		}
		else
		{
			if (dansLesAirs)
				rigidbody.velocity = new Vector3(rigidbody.velocity.x,-9.81f,rigidbody.velocity.z);
			else
				rigidbody.velocity = new Vector3();
		}
	}
}