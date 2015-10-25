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
		gt.transform.position += new Vector3(0, 0.2f, 0);
		while(gt.transform.position.y > 0.7f)
		{
			gt.transform.position += new Vector3(0, -0.01f, 0);
			yield return new WaitForSeconds (0.015f);
		}
		gt.texture = textureList [0];
		yield return new WaitForSeconds (0.8f);
		gt.texture = textureList [1];
		AudioManager.Instance.Play("bip1");
		yield return new WaitForSeconds (0.8f);
		gt.texture = textureList [2];
		AudioManager.Instance.Play("bip1");
		yield return new WaitForSeconds (0.8f);
		gt.texture = textureList [3];
		AudioManager.Instance.Play("bip1");
		yield return new WaitForSeconds (0.8f);
		gt.texture = textureList [4];
		AudioManager.Instance.Play("bip2");
		AudioManager.Instance.PlayDefaultMapMusic();
		KartController.stop = false;

		yield return new WaitForSeconds (0.8f);
		for(int i=0;i<1000;i++)
		{
			gt.transform.position += new Vector3(0, 0.01f, 0);
			yield return new WaitForSeconds (0.01f);
		}


		Destroy (gt);
		Destroy (this);
		
	}
}
