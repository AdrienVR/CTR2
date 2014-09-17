using UnityEngine;
using System.Collections;

public class KartController : MonoBehaviour
{
	
	public Kart kart;
	public bool dansLesAirs = false;
	
	private float ky;
	
	// Use this for initialization
	void Start ()
	{
		rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
	}
	
	void Update()
	{
		float limit = 0.2f;
		if (rigidbody.velocity.y>limit || rigidbody.velocity.y<-limit)
			rigidbody.velocity = new Vector3(rigidbody.velocity.x,-9f,rigidbody.velocity.z);
		else
			rigidbody.velocity = new Vector3(rigidbody.velocity.x,0f,rigidbody.velocity.z);
		
		rigidbody.angularVelocity = Vector3.zero;
		
		/*if (rigidbody.transform.rotation.eulerAngles.z > 30f) {
			Quaternion q = rigidbody.transform.rotation;
			float z = 30f;
			Vector3 vz = new Vector3(0,0,1f);
			q.ToAngleAxis(out z, out vz);
			rigidbody.transform.rotation = q;
				}
		if (rigidbody.transform.rotation.eulerAngles.x>30) {
			Quaternion q = rigidbody.transform.rotation;
			float z = 30f;
			Vector3 vz = new Vector3(1f,0,0);
			q.ToAngleAxis(out z, out vz);
			rigidbody.transform.rotation = q;
		}//*/
	}
	
	// Update is called once per frame
	void FixedUpdate ()
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
			//Debug.Log (dansLesAirs);
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
		//rigidbody.velocity = Vector3.zero;
		//rigidbody.angularVelocity = Vector3.zero;
	}
	
	void OnCollisionExit(Collision collision)
	{
		if(collision.gameObject.name=="Ground")
		{
			dansLesAirs = true;
			//Debug.Log (dansLesAirs);
		}
	}
	
	public Vector3 normalizeVector(Vector3 a)
	{
		float div = Mathf.Sqrt (a.x * a.x + a.y * a.y + a.z * a.z);
		a.x /= div;
		a.y /= div;
		a.z /= div;
		return a;
	}
	
	public void controlPosition()
	{
		Vector3 forwardNormal = rigidbody.transform.forward;
		forwardNormal.y = 0;
		forwardNormal = normalizeVector (forwardNormal);
		if(kart.numeroJoueur==1)
		{
			if(Input.GetKey(KeyCode.S))
			{
				rigidbody.position+=forwardNormal/4*kart.getCoeffVitesse();
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
				rigidbody.position-=forwardNormal/4*kart.getCoeffVitesse();
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
