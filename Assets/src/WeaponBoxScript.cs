using UnityEngine;
using System.Collections;

public class WeaponBoxScript : MonoBehaviour {

	public AnimationClip boxClip;
	// Use this for initialization
	void Start () {
	}

	void OnTriggerEnter(Collider other)
	{
		animation.Play (boxClip.name);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
