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

	private static int nbPlayers;
	private int nbPoints = 0;
	private KartController kc;
	private static Dictionary <int, List<Rect>> cameraMap = new Dictionary <int, List<Rect>>{
		{1, new List<Rect>(){new Rect(0, 0, 1, 1)}},
		{2, new List<Rect>(){new Rect(0, 0.51f, 1, 0.49f), new Rect(0, 0, 1, 0.49f)}},
		{3, new List<Rect>(){new Rect(0, 0.51f, 0.49f, 0.49f), new Rect(0, 0, 0.49f, 0.49f), 
				new Rect(0.51f, 0, 1, 0.49f)}},
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
	}
	
	public void InitGuiLayer()
	{
		armeGui = GameObject.Instantiate (Resources.Load ("arme"), new Vector3 (0.025f, 0.55f, 0), Quaternion.identity) as GameObject;
		armeGui.layer = LayerMask.NameToLayer ("layer_j" + numeroJoueur);
		ws = (WeaponScript)armeGui.GetComponent ("WeaponScript");
	}

	public void AddPoint(int n)
	{
		nbPoints+=n;
		Debug.Log (numeroJoueur + " a " + nbPoints + " points !");
	}
}










