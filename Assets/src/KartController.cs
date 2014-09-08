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
			//rigidbody.AddForce (0, -30000, 0);
		}
	}


	void OnCollisionStay(Collision collision)
	{
		if(collision.gameObject.name=="Ground")
		{
			dansLesAirs = false;
			Debug.Log (dansLesAirs);
		}
		if(collision.gameObject.name=="accelerateur")
		{
			if(kart.numeroJoueur==1)
			{
				if(Input.GetKey(KeyCode.S))
				{
					rigidbody.position+=this.transform.forward*2*kart.getCoeffVitesse();
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
					rigidbody.position-=this.transform.forward*2*kart.getCoeffVitesse();
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
			if(kart.numeroJoueur==2)
			{
				if(Input.GetKey(KeyCode.K))
				{
					rigidbody.position+=this.transform.forward*2*kart.getCoeffVitesse();
					if(Input.GetKey(KeyCode.L))
					{
						transform.Rotate(0,-0.5f*kart.getCoeffManiabilite(),0);
					}
					if(Input.GetKey(KeyCode.J))
					{
						transform.Rotate(0,0.5f*kart.getCoeffManiabilite(),0);
					}
				}
				if(Input.GetKey(KeyCode.I))
				{
					rigidbody.position-=this.transform.forward*2*kart.getCoeffVitesse();
					if(Input.GetKey(KeyCode.L))
					{
						transform.Rotate(0,0.5f*kart.getCoeffManiabilite(),0);
					}
					if(Input.GetKey(KeyCode.J))
					{
						transform.Rotate(0,-0.5f*kart.getCoeffManiabilite(),0);
					}
				}
				if(Input.GetKeyUp(KeyCode.B))
				{
					if(dansLesAirs==false)
					{
						rigidbody.AddForce(0,600000,0);
					}
				}
			}
		}
	}

	void OnCollisionEnter(Collision collision)
	{

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
		if(kart.numeroJoueur==1)
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
		if(kart.numeroJoueur==2)
		{
			if(Input.GetKey(KeyCode.K))
			{
				rigidbody.position+=this.transform.forward/4*kart.getCoeffVitesse();
				if(Input.GetKey(KeyCode.L))
				{
					transform.Rotate(0,-0.5f*kart.getCoeffManiabilite(),0);
				}
				if(Input.GetKey(KeyCode.J))
				{
					transform.Rotate(0,0.5f*kart.getCoeffManiabilite(),0);
				}
			}
			if(Input.GetKey(KeyCode.I))
			{
				rigidbody.position-=this.transform.forward/4*kart.getCoeffVitesse();
				if(Input.GetKey(KeyCode.L))
				{
					transform.Rotate(0,0.5f*kart.getCoeffManiabilite(),0);
				}
				if(Input.GetKey(KeyCode.J))
				{
					transform.Rotate(0,-0.5f*kart.getCoeffManiabilite(),0);
				}
			}
			if(Input.GetKeyUp(KeyCode.B))
			{
				if(dansLesAirs==false)
				{
					rigidbody.AddForce(0,600000,0);
				}
			}
		}
	}

}
