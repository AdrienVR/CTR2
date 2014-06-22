using UnityEngine;
using System.Collections;

public class KartController : MonoBehaviour
{

	public Kart kart;

	// Use this for initialization
	void Start ()
	{
		rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
	}
	
	// Update is called once per frame
	void Update ()
	{
		controlPosition ();
	}

	public void controlPosition()
	{
		if(Input.GetKey(KeyCode.S))
		{
			rigidbody.position+=this.transform.forward/4*kart.getCoeffVitesse();
			if(Input.GetKey(KeyCode.D))
			{
				transform.Rotate(0,-0.5f*kart.getCoeffManiabilite(),0);
			}
			if(Input.GetKey(KeyCode.Q))
			{
				transform.Rotate(0,0.5f*kart.getCoeffManiabilite(),0);
			}
		}
		if(Input.GetKey(KeyCode.Z))
		{
			rigidbody.position-=this.transform.forward/4*kart.getCoeffVitesse();
			if(Input.GetKey(KeyCode.D))
			{
				transform.Rotate(0,0.5f*kart.getCoeffManiabilite(),0);
			}
			if(Input.GetKey(KeyCode.Q))
			{
				transform.Rotate(0,-0.5f*kart.getCoeffManiabilite(),0);
			}
		}
	}

}
