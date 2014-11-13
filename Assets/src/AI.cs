using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI : MonoBehaviour {
	
	public List<Transform> listTransform = new List<Transform> ();
	public static GameObject kart;
	public static Dictionary <string, Transform> wheels = new Dictionary <string, Transform>();
	private int position = 0;
	private float speed = 0.5f;
	// Use this for initialization
	void Start () {
		
		foreach(Transform child in transform)
		{
			listTransform.Add(child);
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
		if(kart){
			float dist = 1/Vector3.Distance(kart.transform.position, listTransform[position].position);
			kart.transform.position = Vector3.Lerp(kart.transform.position,
				                                       listTransform[position].position,
				                                       dist*speed);


			kart.transform.rotation = Quaternion.Euler(Vector3.Lerp(kart.transform.rotation.eulerAngles,
			                                       listTransform[position].rotation.eulerAngles,
			                                                        dist*speed));
			wheels["steering"].rotation = Quaternion.Euler(
				Vector3.Lerp(wheels["steering"].rotation.eulerAngles,
			                 kart.transform.rotation.eulerAngles + new Vector3(0, listTransform[position].rotation.eulerAngles.y%45f),
			             dist*speed));
			wheels["wheelAL"].rotation = Quaternion.Euler (wheels["steering"].rotation.eulerAngles*3 + new Vector3 (0, 90f)) ;
			wheels["wheelAR"].rotation = Quaternion.Euler (wheels["steering"].rotation.eulerAngles*3 + new Vector3 (0, 90f)) ;

			if (Vector3.Distance(kart.transform.position, listTransform[position].position)< 2.0f)
				position++;
			if (position == listTransform.Count)
				position=0;

		}
	}
}
