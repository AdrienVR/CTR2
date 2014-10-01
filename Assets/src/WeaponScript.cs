using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponScript : MonoBehaviour {

	public List<Texture> textureList;
	// Use this for initialization
	void Start () {
	
	}

	public void SetTextureN( int n)
	{
		gameObject.guiTexture.texture = textureList[n-1];
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
