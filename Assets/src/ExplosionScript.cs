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
	public static List<string> targets = new List<string>() {"coco_prefab","crash_prefab","crash_prefab(Clone)"};
	public static List<string> boxes = new List<string>() {"weaponBox","appleBox"};
	// Use this for initialization
	void Start () {
	}
	
	
	public void ActionExplosion()
	{
		CapsuleCollider cc = (CapsuleCollider)GetComponent ("CapsuleCollider");
		cc.radius = 6.5f;
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(boxes.IndexOf(other.name)!=-1 || exploded)
			return;
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
		if (exploded)
			rigidbody.velocity = new Vector3();
	}
}