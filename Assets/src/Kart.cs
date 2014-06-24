using UnityEngine;
using System.Collections;

public class Kart 
{
	private float coeffVitesse;
	private float coeffManiabilite;
	public int numeroJoueur;
	private KartController kc;
	private Camera1Controller cm1c;
	public GameObject objet;
	public GameObject target; // objet cible que la caméra va suivre et qui suit lui meme le kart
	public GameObject camera1;

	public Kart(int n, float cv, float cm)
	{
		numeroJoueur = n;
		setCoeffVitesse(cv);
		setCoeffManiabilite (cm);
		initObjet (numeroJoueur);
		initCamera1 (numeroJoueur);
	}

	public void initObjet(int j)
	{
		if(j==1)
		{
			GameObject kart= Resources.Load("kart") as GameObject;
			Vector3 n = new Vector3 (0,1.3f, -5); // vecteur position
			Quaternion m = new Quaternion(0,0,0,0); // quaternion rotation
			objet = GameObject.Instantiate (kart,n,m) as GameObject;
		}
		else if(j==2)
		{
			GameObject kart2= Resources.Load("kart2") as GameObject;
			Vector3 n = new Vector3 (0,1.3f, 5); // vecteur position
			Quaternion m = new Quaternion(0,0,0,0); // quaternion rotation
			objet = GameObject.Instantiate (kart2,n,m) as GameObject;
		}
		objet.rigidbody.transform.Rotate(0,0,0);
		//objet.transform.localScale = new Vector3 (5f, 5f, 5f);
		//objet.AddComponent ("Rigidbody");
		//objet.AddComponent ("BoxCollider");
		objet.AddComponent ("KartController");
		kc = (KartController)objet.GetComponent ("KartController");
		kc.kart = this;
	}

	public void initCamera1(int n)
	{
		camera1 = new GameObject ();
		camera1.AddComponent ("Camera");
		if(n==2) camera1.camera.rect= new Rect (0, 0, 1, 0.49f);
		else if (n==1) camera1.camera.rect= new Rect (0, 0.51f, 1, 0.49f);
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
