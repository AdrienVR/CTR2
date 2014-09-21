using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	
	private KartController kc;
	//private GUIText gText;

	// Use this for initialization
	void Start () { 
		//gText = Resources.Load("GUI_Text") as GUIText;
	}

	public void SetKartController(KartController k)
	{
		kc = k;
	}
	
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 t= kc.transform.position+ new Vector3(0f,4f,0f) + 6*kc.transform.forward;
		transform.position = t;
		transform.LookAt (kc.transform.position + new Vector3(0f,2f,0f));
	}
	
}
