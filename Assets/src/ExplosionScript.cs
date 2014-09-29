using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExplosionScript : MonoBehaviour {
	
	public AudioClip explosionClip;
	public Color explosionColor;
	public Vector3 vitesseInitiale;
	public AudioClip bombExplose; // ceci est un attribut
	public GameObject owner;
	public bool isAlive;
	private bool dansLesAirs;
	public List<string> targets;
	// Use this for initialization
	void Start () {
		targets = new List<string>() {"coco_prefab","crash_prefab","crash_prefab(Clone)"};
		dansLesAirs = true;
	}
	
	
	public void ActionExplosion()
	{
		CapsuleCollider cc = (CapsuleCollider)GetComponent ("CapsuleCollider");
		cc.radius = 6.5f;
		StartCoroutine (Explode());
	}
	
	void OnTriggerEnter(Collider other)
	{
		StartCoroutine (Explode());
		if (targets.IndexOf (other.name) == -1)
			return;
		KartController touched = (KartController)other.GetComponent ("KartController");
		if(touched.gameObject!=owner.gameObject &&  touched.state.IndexOf("invincible") == -1)
		{
			((KartController)owner.GetComponent ("KartController")).GetKart().addPoint ();
		}
		if (other.gameObject != owner)
			touched.Die ();
		ActionExplosion ();
	}
	
	IEnumerator Explode()
	{

		((KartController)owner.GetComponent ("KartController")).pipi = false;
		if (explosionClip != null)
			animation.Play (explosionClip.name);
		audio.PlayOneShot (bombExplose);
		gameObject.light.color = explosionColor;
		yield return new WaitForSeconds (0.1f);
		yield return new WaitForSeconds (3f);
		Destroy(gameObject);
		
	}
	
	
	void OnTriggerExit(Collider other)
	{
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
		if (rigidbody != null)
			rigidbody.velocity = -((KartController)owner.GetComponent ("KartController")).facteurSens*rigidbody.transform.forward*50f;
		
		if (dansLesAirs)
			rigidbody.velocity = new Vector3(rigidbody.velocity.x,-9.81f,rigidbody.velocity.z);
	}
}