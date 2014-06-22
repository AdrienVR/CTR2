using UnityEngine;
using System.Collections;

public class Kart 
{
	private float coeffVitesse;
	private float coeffManiabilite;
	private KartController kc;
	private Camera1Controller cm1c;
	public GameObject objet;
	public GameObject target; // objet cible que la caméra va suivre et qui suit lui meme le kart
	public GameObject camera1;

	public Kart(float cv, float cm)
	{
		setCoeffVitesse(cv);
		setCoeffManiabilite (cm);
		initObjet ();
		initCamera1 ();
	}

	public void initObjet()
	{
		//objet = GameObject.CreatePrimitive(PrimitiveType.Cube);
		GameObject kart= Resources.Load("kart") as GameObject;
		Vector3 n = new Vector3 (0,1.3f, 0); // vecteur position
		Quaternion m = new Quaternion(0,0,0,0); // quaternion rotation
		objet = GameObject.Instantiate (kart,n,m) as GameObject;
		objet.rigidbody.transform.Rotate(0,0,0);
		//objet.transform.localScale = new Vector3 (5f, 5f, 5f);
		//objet.AddComponent ("Rigidbody");
		//objet.AddComponent ("BoxCollider");
		objet.AddComponent ("KartController");
		kc = (KartController)objet.GetComponent ("KartController");
		kc.kart = this;
	}

	public void initCamera1()
	{
		camera1 = new GameObject ();
		camera1.AddComponent ("Camera");
		camera1.AddComponent ("Camera1Controller");
		cm1c = (Camera1Controller)camera1.GetComponent ("Camera1Controller");
		cm1c.kc = kc;
		camera1.AddComponent ("GUILayer");
		camera1.transform.position = objet.rigidbody.position+new Vector3(0,1,0);

	}

	public void setCoeffVitesse(float cv)
	{
		if(cv>=1 && cv<=8)
		{
			coeffVitesse=cv;
		}
		else if(cv<1) cv=1;
		else cv=8;
	}
	
	public void setCoeffManiabilite(float cm)
	{
		if(cm>=1 && cm<=5)
		{
			coeffManiabilite=cm;
		}
		else if(cm<1) cm=1;
		else cm=5;
	}

	public float getCoeffVitesse()
	{
		return coeffVitesse;
	}
	public float getCoeffManiabilite()
	{
		return coeffManiabilite;
	}

}
