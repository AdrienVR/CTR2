using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Party : MonoBehaviour {

	public int parties = 0;
	public static List<string> colors = new List<string>() {"red", "blue", "green", "yellow", "pink"};
	private bool ready = true;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (parties>1000 || !ready)
			return;
		string color;
		{
			for(int i = 0; i<50;i++){
				float x = (float)Random.Range (-100, 100);
				float z = (float)Random.Range (-100,100);
				Vector3 pos = new Vector3(x,0,z);
				color = colors[i%colors.Count];
				GameObject kart = GameObject.Instantiate (Resources.Load("Papers/"+color+"Paper"), pos, new Quaternion()) as GameObject;
				kart.transform.position = pos;
				parties++;
			}
		}
		StartCoroutine(wait());
	}

	
	IEnumerator wait()
	{
				ready = false;
				yield return new WaitForSeconds (1.25f);
				ready = true;
		}
}
