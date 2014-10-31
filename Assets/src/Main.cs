using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour
{
	public int nbPlayer;
	public Texture normal;
	public Texture hover;
	public Texture triVolume3;
	public Texture triVolume2;
	public Texture triVolume1;

	public GameObject respawn1;
	public GameObject respawn2;
	public GameObject respawn3;
	public GameObject respawn4;

	private List<Transform> listRespawn;
	public List<Kart> players;

	public static int nbPtsPartie = 15;

	public float speedCoeff;
	public float turnCoeff;
	public static Main main;

	void Start()
	{
		gameObject.AddComponent ("Dictionnaries");
		Debug.Log ("Demarrage !");
		main = this;
		Init ();
	}

	public static void Init()
	{
		Kart.nbPlayers = main.nbPlayer;
		KartController.setCoefficients (main.speedCoeff, main.turnCoeff);
		main.InitializeRespawn ();
		
		main.CreateNPersos(main.nbPlayer);
		main.InitMenus ();
		Instantiate (Resources.Load ("guiStartFire"));
	}

	void InitMenus()
	{
		Menus m =(Menus)gameObject.AddComponent ("Menus");
		m.normal = normal;
		m.hover = hover;
		m.triVolume1 = triVolume1;
		m.triVolume2 = triVolume2;
		m.triVolume3 = triVolume3;
	}

	void InitializeRespawn()
	{
		listRespawn = new List<Transform> {respawn1.transform,
			respawn2.transform,
			respawn3.transform,
			respawn4.transform};
	}
	
	void CreateNPersos(int n)
	{
		players = new List<Kart>();
		for (int i=0; i<n; i++)
		{
			Kart a = new Kart(listRespawn[i].position, listRespawn[i].rotation, Dictionnaries.listKarts[i]);
			players.Add(a);
		}
	}

	public static void Restart(){
		Application.LoadLevel (Application.loadedLevel);
		Kart.nPlayer = 0;
		KartController.stop = true;
		AudioListener.pause = false;
	}


}