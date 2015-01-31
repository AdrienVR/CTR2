using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HighControl : MonoBehaviour {
	private KartController kc;
	private float previousHigh;
	private List<float> moyPente = new List<float>(){0,0,0,0,0,0,0,0,0,0};
	private int i = 0;


	void Start ()
	{
		kc = GetComponent<KartController>();
		
		previousHigh = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		i++;
		i%=10;

		moyPente[i] = transform.position.y - previousHigh;
		float moy = moyPente.Average();

		//Debug.DrawRay(transform.position, new Vector3(0,1), Color.blue, 20, true);

		if ( moyPente[i] < moy-0.1f){
			//Debug.DrawRay(transform.position+new Vector3(0,1), new Vector3(0,1), Color.yellow, 20, true);
			float mix = previousHigh * 1f + moy;
			transform.position = transform.position + new Vector3(0,mix - transform.position.y);
		}
		//Debug.DrawRay(transform.position, new Vector3(0,1), Color.red, 20, true);
		previousHigh = transform.position.y;
	}
}
