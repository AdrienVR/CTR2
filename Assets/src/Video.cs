using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Video : MonoBehaviour {
	
	public List<Texture> textureList;

	void Start ()
	{
		StartCoroutine(Anim());
	}
	
	IEnumerator Anim()
	{
		GUITexture gt = (GUITexture)GetComponent ("GUITexture");
		gt.transform.localScale = new Vector3 (1, 1, 1);
		for(int i=0;i<textureList.Count;i++)
		{
			gt.texture = textureList [i];
			yield return new WaitForSeconds(1f/25f);
			if(i==textureList.Count-1)
				i=-1;
		}
	}
}
