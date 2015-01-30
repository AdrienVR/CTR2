using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShowRoom : MonoBehaviour {
	
	public static Dictionary <string, Transform> characters = new Dictionary <string, Transform>();
	public static Transform cam;
	public static int current_anim_nb = 0;
	public static ShowRoom sr;
	// Use this for initialization
	void Start () {
		
		foreach (Transform child in transform){
			if (child.name == "camera"){
				cam = child;
				continue;
			}
			characters[child.name] = child;
		}
		sr = this;
	}

	public static void ShowModel(string name)
	{
		foreach(Transform child in characters.Values){
			child.gameObject.SetActive(false);
			child.transform.rotation = new Quaternion();
		}
		characters[name].gameObject.SetActive(true);
		sr.StartCoroutine(Anim());
	}
	
	
	public static void Leave()
	{
		foreach(Transform child in characters.Values)
			child.gameObject.SetActive(false);
	}

	static IEnumerator Anim()
	{
		current_anim_nb ++;
		yield return 0;
		//cam.rotation = Quaternion.Euler(new Vector3(0,-20));
		
		/*while(cam.rotation.eulerAngles.y < 0 && current_anim_nb <2){
			yield return new WaitForSeconds (0.01f);
			cam.rotation = Quaternion.Euler(cam.rotation.eulerAngles + new Vector3(0,1));

		}*/
		current_anim_nb --;
	}
}
