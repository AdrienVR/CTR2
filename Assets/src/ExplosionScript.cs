using UnityEngine;
using System.Collections;

public class ExplosionScript : MonoBehaviour {
	
	public AnimationClip explosionClip;
	public Color explosionColor;
	public Vector3 vitesseInitiale;
	public GameObject owner;
	public bool isAlive;
	// Use this for initialization
	void Start () {
	}
	
	public void ActionExplosion()
	{
		Debug.Log ("BOUM");
		CapsuleCollider cc = (CapsuleCollider)GetComponent ("CapsuleCollider");
		cc.radius = 6.5f;
		StartCoroutine (Explode());
	}

	void OnTriggerEnter(Collider other)
	{
		StartCoroutine (Explode());
		KartController touched = (KartController)other.GetComponent ("KartController");
		if (touched!= null && other.gameObject != owner)touched.Die ();
		ActionExplosion ();
	}

	IEnumerator Explode()
	{
		((KartController)owner.GetComponent ("KartController")).pipi = false;
		if (explosionClip != null)
			animation.Play (explosionClip.name);
		gameObject.light.color = explosionColor;
		yield return new WaitForSeconds (0.1f);
		Destroy(gameObject);
	}

	
	void OnTriggerExit(Collider other)
	{
	}

	// Update is called once per frame
	void Update () {
		if (rigidbody != null)
			rigidbody.velocity = -rigidbody.transform.forward*50f;
	}
}
