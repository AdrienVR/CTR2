using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Kart 
{
	public static int nPlayer=0;
	private static int nbPlayers;

	public int numeroJoueur;

	private KartController kc;
	private CameraController cm1c;
	private static Dictionary <int, List<Rect>> cameraMap;

	public Kart(Vector3 pos, Quaternion q)
	{
		nPlayer++;
		numeroJoueur = nPlayer;
		initObjet (pos, q);
		initCamera ();
	}

	public static void SetNbPlayers(int n)
	{
		nbPlayers = n;
	}

	public void initObjet(Vector3 pos, Quaternion q)
	{
		int j = numeroJoueur;
		Dictionary <int, string> prefabMap = new Dictionary <int, string>{{1,"crash_prefab"},{2,"coco_prefab"},{3,"crash_prefab"},{4,"crash_prefab"}};

		GameObject kart= Resources.Load(prefabMap[j]) as GameObject;
		GameObject objet = GameObject.Instantiate (kart, pos, q) as GameObject;
		//objet.transform.localScale = new Vector3 (5f, 5f, 5f);
		kc = (KartController)objet.GetComponent ("KartController");
		kc.SetKart(this);
	}

	public void initCamera()
	{
		GameObject camera = new GameObject ();
		camera.AddComponent ("Camera");
		
		if (cameraMap == null) {
			cameraMap = new Dictionary <int, List<Rect>>{
				{1, new List<Rect>(){new Rect(0, 0, 1, 1)}},
				{2, new List<Rect>(){new Rect(0, 0.51f, 1, 0.49f), new Rect(0, 0, 1, 0.49f)}},
				{3, new List<Rect>(){new Rect(0, 0.51f, 0.49f, 0.49f), new Rect(0, 0, 0.49f, 0.49f), 
						new Rect(0.51f, 0, 1, 0.49f)}},
				{4, new List<Rect>(){new Rect(0, 0.51f, 0.49f, 1), new Rect(0, 0, 0.49f, 0.49f), 
						new Rect(0.51f, 0.51f, 1, 1), new Rect(0.51f, 0, 1, 0.49f)}}
			};
		}

		camera.camera.rect = cameraMap[nbPlayers][numeroJoueur-1];
		camera.AddComponent ("CameraController");
		cm1c = (CameraController)camera.GetComponent ("CameraController");
		cm1c.SetKartController(kc);
		camera.AddComponent ("GUILayer");
		GUILayer guiLayer = (GUILayer)camera.GetComponent ("GUILayer");
		//camera.camera.cullingMask = numeroJoueur;
		//guiLayer.

	}

}










