using UnityEngine;
using System.Collections;

public class BoxExplosionScript : MonoBehaviour {
	
	public AnimationClip boxClip;
	// Use this for initialization
	void Start () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		animation.Play (boxClip.name);
		GameObject.Find ("nitro").light.color = new Color (.0f, 1.0f, .0f);
	}

	
	void OnTriggerExit(Collider other)
	{
		GameObject.Find ("nitro").light.color = new Color (.0f, .0f, .0f);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
