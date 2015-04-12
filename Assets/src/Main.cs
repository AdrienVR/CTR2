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

	public List<Transform> listRespawn;
	public List<Kart> players;

	public static int nbPtsPartie = 8;

	public float speedCoeff;
	public float turnCoeff;
	public static Main main;

	private string[] lignesArray;

	public static bool forward;
	public static bool right=false;
	private bool musicStarted = false;

	public static StatGame statistics;

	private static Kart kartAkuPlaying;

	/*public void executeIA()
	{
		lignesArray = System.IO.File.ReadAllLines(@"Resources\log.txt");
		StartCoroutine(execute());
	}

	IEnumerator execute()
	{
		forward = true;
		for(int i=0;i<lignesArray.Length;i++)
		{
			forward = true;
			if(lignesArray[i]=="j'ai appuye sur D")
			{
				right=true;
			}
			yield return new WaitForSeconds(1f/70f);
			right=false;
			if(i==lignesArray.Length-1)
				i=0;
		}
		forward = false;
	}*/

	void FixedUpdate()
	{
		ControllerAPI.CheckJoysticks ();
	}

	IEnumerator SetCommon()
	{
		if (AudioManager.Instance == null && ControllerInterface.Instance == null)
		{
			yield return new WaitForSeconds(0.1f);
			Application.LoadLevelAdditive("commonScene");
		}
	}

	void Start()
	{

		StartCoroutine(SetCommon());

		nbPtsPartie = Game.nbPoints;
		nbPlayer = Game.listKarts.Count;
		nbPlayer = System.Math.Max (nbPlayer, 1);
		nbPlayer = System.Math.Min (nbPlayer, 4);
		statistics = new StatGame (nbPlayer);
		ControllerAPI.InitJoysticks ();

		foreach(Transform respawnPoint in transform)
		{
			listRespawn.Add(respawnPoint);
		}

		gameObject.AddComponent ("Game");
		main = this;
		Kart.setCoefficients (speedCoeff, turnCoeff);
		Init ();

		Screen.showCursor = false; 
		Debug.Log ("Starting with "+ ControllerAPI.nControllers + " controllers.");

	}

	public static bool isPlayingAku()
	{
		bool b=false;
		foreach(Kart k in main.players)
		{
			if(k.kart_script.protection!=null)
				b=true;
		}
		return b;
	}

	public static void Init()
	{
		Kart.totalPlayers = main.nbPlayer;
		forward = false;
		main.CreateNPersos(main.nbPlayer);
		main.InitMenus ();
		Instantiate (Resources.Load ("guiStartFire"));
		statistics.startGame ();
	}

	void InitMenus()
	{
		Menus m =(Menus)gameObject.AddComponent ("Menus");
		m.normal = normal;
		m.hover = hover;
		m.triVolume1 = triVolume1;
		m.triVolume2 = triVolume2;
		m.triVolume3 = triVolume3;
		m.main = this;
	}
	
	void CreateNPersos(int n)
	{
		players = new List<Kart>();
		for (int i=0; i<n; i++)
		{
			Kart a = new Kart(listRespawn[i].position, listRespawn[i].rotation, Game.listKarts[i]);
			players.Add(a);
		}
	}

	public static void Restart(){
		Kart.nPlayer = 0;
		KartController.stop = true;
		KartController.IA_enabled = false;
		AudioListener.pause = false;
	}


}