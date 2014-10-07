using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Feux_depart_Script : MonoBehaviour {

	public List<Texture> textureList;
	// Use this for initialization
	void Start ()
	{
		StartCoroutine(Anim());
	}

	IEnumerator Anim()
	{
		GUITexture gt = (GUITexture)GetComponent ("GUITexture");
		gt.transform.position=new Vector3(gt.transform.position.x,gt.transform.position.y+0.2f,gt.transform.position.z);
		while(gt.transform.position.y>0.7)
		{
			gt.transform.position=new Vector3(gt.transform.position.x,gt.transform.position.y-0.01f,gt.transform.position.z);
			yield return new WaitForSeconds (0.015f);
		}
		gt.texture = textureList [0];
		yield return new WaitForSeconds (0.8f);
		gt.texture = textureList [1];
		yield return new WaitForSeconds (0.8f);
		gt.texture = textureList [2];
		yield return new WaitForSeconds (0.8f);
		gt.texture = textureList [3];
		yield return new WaitForSeconds (0.8f);
		gt.texture = textureList [4];
		yield return new WaitForSeconds (0.8f);
		for(int i=0;i<1000;i++)
		{
			gt.transform.position=new Vector3(gt.transform.position.x,gt.transform.position.y+0.01f,gt.transform.position.z);
			yield return new WaitForSeconds (0.01f);
		}
		Destroy (gt);
		
	}

	// Update is called once per frame
	void Update () {
	
	}
}
