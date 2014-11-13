using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour
{
	public AudioClip soundUp;
	public AudioClip soundOk;
	public AudioClip soundCancel;
	public AudioClip mainMusic;
	public int nbPlayer;
	public Texture normal;
	public Texture hover;
	public Texture triVolume3;
	public Texture triVolume2;
	public Texture triVolume1;

	public AudioSource sourceMusic;

	public GameObject respawn1;
	public GameObject respawn2;
	public GameObject respawn3;
	public GameObject respawn4;

	public List<Transform> listRespawn;
	public List<Kart> players;

	public static int nbPtsPartie = 1;

	public float speedCoeff;
	public float turnCoeff;
	public static Main main;

	private string[] lignesArray;

	public static bool forward;
	public static bool right=false;
	private bool musicStarted = false;

	public void executeIA()
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
	}

	void Update()
	{
		if(KartController.stop==false && musicStarted==false)
		{
			musicStarted=true;
			if(sourceMusic)
				sourceMusic.enabled=true;
		}
	}

	void Start()
	{
		gameObject.AddComponent ("Game");
		Debug.Log ("Demarrage !");
		main = this;
		Kart.setCoefficients (speedCoeff, turnCoeff);
		Init ();
		AudioSource[] sources = gameObject.GetComponents<AudioSource> ();
		for (int i =0; i<sources.Length; i++)
			if (sources [i].loop == true)
				sourceMusic = sources [i];
	}


	public static void Init()
	{
		Kart.nbPlayers = main.nbPlayer;
		main.InitializeRespawn ();
		forward = false;
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
		m.main = this;
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