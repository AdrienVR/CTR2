using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour
{
	public int nbMap;

	void Start()
	{
		Debug.Log ("Demarrage !");

		Kart crash = new Kart(1,2,4);
		if (nbMap == 2)
			crash.objet.rigidbody.transform.Translate(50, 0, -40);
		Kart coco = new Kart(2,2,4);
	}

}