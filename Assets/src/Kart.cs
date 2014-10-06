using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Kart 
{
	public static int nPlayer=0;
	public CameraController cm1c;
	public int numeroJoueur;

	public GameObject armeGui;
	public WeaponScript ws;
	private GUIText pointText;
	public GUIText pommeText;

	private static int nbPlayers;
	private int nbPoints = 0;
	public int nbApples = 0;
	private KartController kc;
	private static Dictionary <int, List<Rect>> cameraMap = new Dictionary <int, List<Rect>>{
		{1, new List<Rect>(){new Rect(0, 0, 1, 1)}},
		{2, new List<Rect>(){new Rect(0, 0.51f, 1, 0.49f), new Rect(0, 0, 1, 0.49f)}},
		{3, new List<Rect>(){new Rect(0, 0.51f, 1, 1), new Rect(0, 0, 0.495f, 0.49f), 
				new Rect(0.505f, 0, 1, 0.49f)}},
		{4, new List<Rect>(){new Rect(0, 0.51f, 0.49f, 1), new Rect(0, 0, 0.49f, 0.49f), 
				new Rect(0.51f, 0.51f, 1, 1), new Rect(0.51f, 0, 1, 0.49f)}}
	};

	public Kart(Vector3 pos, Quaternion q)
	{
		nbPoints = 0;
		nPlayer++;
		numeroJoueur = nPlayer;
		InitObjet (pos, q);
		InitCamera ();
		InitGuiLayer ();
	}

	public static void SetNbPlayers(int n)
	{
		nbPlayers = n;
	}

	public void InitObjet(Vector3 pos, Quaternion q)
	{
		int j = numeroJoueur;
		Dictionary <int, string> prefabMap = new Dictionary <int, string>{{1,"crash_prefab"},{2,"crash_prefab"},{3,"crash_prefab"},{4,"crash_prefab"}};

		GameObject kart= Resources.Load(prefabMap[j]) as GameObject;
		kart = GameObject.Instantiate (kart, pos, q) as GameObject;
		kc = (KartController)kart.GetComponent ("KartController");
		kc.SetKart(this);
	}

	public void InitCamera()
	{
		GameObject camera = Resources.Load("Camera_prefab") as GameObject;
		camera = GameObject.Instantiate (camera) as GameObject;
		camera.camera.rect = cameraMap[nbPlayers][numeroJoueur-1];
		cm1c = (CameraController) camera.AddComponent ("CameraController");
		cm1c.SetKartController(kc);
		camera.camera.cullingMask |= (1 << LayerMask.NameToLayer("layer_j"+numeroJoueur));

		Camera c2d = null;
		foreach (Transform child in kc.transform)
			if (child.name == "Camera2D")
				c2d = (Camera)child.gameObject.GetComponent("Camera");
		c2d.cullingMask |= (1 << LayerMask.NameToLayer("layer2d_j"+numeroJoueur));
		c2d.rect = cameraMap[nbPlayers][numeroJoueur-1];
	}
	
	public void InitGuiLayer()
	{
		armeGui = GameObject.Instantiate (Resources.Load ("arme"), new Vector3 (0.025f, 0.55f, 0), Quaternion.identity) as GameObject;
		armeGui.layer = LayerMask.NameToLayer ("layer_j" + numeroJoueur);
		GameObject pointGui = GameObject.Instantiate (Resources.Load ("pointTexture")) as GameObject;
		pointGui.layer= LayerMask.NameToLayer ("layer_j" + numeroJoueur);
		pointText = (GUIText)pointGui.GetComponent ("GUIText");
		pointText.text="0";
		if (nbPlayers > 2)
			pointText.transform.position = new Vector3(0.8f,pointText.transform.position.y,pointText.transform.position.z) ;
		ws = (WeaponScript)armeGui.GetComponent ("WeaponScript");

		Transform pomme = null;
		foreach (Transform child in kc.transform)
			if (child.name == "apple")
				pomme = child;
		pomme.gameObject.layer = LayerMask.NameToLayer ("layer2d_j" + numeroJoueur);
		
		GameObject nbAppleGui = GameObject.Instantiate (Resources.Load ("pommeText")) as GameObject;
		nbAppleGui.layer= LayerMask.NameToLayer ("layer_j" + numeroJoueur);
		pommeText = (GUIText)nbAppleGui.GetComponent ("GUIText");
		pommeText.text = "x 0";

	}

	public void AddPoint(int n)
	{
		nbPoints+=n;
		//Debug.Log (numeroJoueur + " a " + nbPoints + " points !");
		pointText.text = nbPoints.ToString();
	}
}










