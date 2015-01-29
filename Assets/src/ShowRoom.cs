using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShowRoom : MonoBehaviour {
	
	public static Dictionary <string, Transform> characters = new Dictionary <string, Transform>();
	public static Transform cam;
	public static ShowRoom sr;
	// Use this for initialization
	void Start () {
		
		foreach (Transform child in transform){
			if (child.name == "camera")
				cam = child;
			characters[child.name] = child;
		}
		sr = this;
	}

	public static void ShowCharacter(string name)
	{
		foreach(Transform child in characters.Values)
			child.gameObject.SetActive(false);
		characters[name].gameObject.SetActive(true);
		sr.StartCoroutine(Anim());
	}

	static IEnumerator Anim()
	{
		while(cam.rotation.eulerAngles.y < 90){
			yield return new WaitForSeconds (0.27f);
			cam.rotation = Quaternion.Euler(cam.rotation.eulerAngles + new Vector3(0,1));
		}
		cam.rotation = Quaternion.Euler(new Vector3(0,-90));
		
		while(cam.rotation.eulerAngles.y < 0){
			yield return new WaitForSeconds (0.27f);
			cam.rotation = Quaternion.Euler(cam.rotation.eulerAngles + new Vector3(0,1));
		}
	}
}
