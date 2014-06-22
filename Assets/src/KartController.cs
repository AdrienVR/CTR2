using UnityEngine;
using System.Collections;

public class KartController : MonoBehaviour
{

	public Kart kart;
	public bool dansLesAirs = false;

	// Use this for initialization
	void Start ()
	{
		rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
	}
	
	// Update is called once per frame
	void Update ()
	{
		controlPosition ();
		if(dansLesAirs==true)
		{
			rigidbody.AddForce (0, -30000, 0);
		}
	}


	void OnCollisionStay(Collision collision)
	{
		if(collision.gameObject.name=="Ground")
		{
			dansLesAirs = false;
			Debug.Log (dansLesAirs);
		}
	}
	void OnCollisionExit(Collision collision)
	{
		if(collision.gameObject.name=="Ground")
		{
			dansLesAirs = true;
			Debug.Log (dansLesAirs);
		}
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
			if(Input.GetKeyUp(KeyCode.Space))
			{
				if(dansLesAirs==false)
				{
					rigidbody.AddForce(0,600000,0);
				}
			}
	}

}
