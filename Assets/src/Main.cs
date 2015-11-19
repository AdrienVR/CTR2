using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour
{
	public Texture normal;
	public Texture hover;
	public Texture triVolume3;
	public Texture triVolume2;
	public Texture triVolume1;

	public List<Transform> listRespawn;
	public List<Kart> players;
    
	public static Main main;

	private string[] lignesArray;

	public static bool forward;
	public static bool right=false;
	//private bool musicStarted = false;

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

	void Awake()
	{
		statistics = new StatGame (PlayerManager.Instance.CurrentPlayers.Count);

		foreach(Transform respawnPoint in transform)
		{
			listRespawn.Add(respawnPoint);
		}

		gameObject.AddComponent <Game>();
		main = this;
		Init ();

#if UNITY_EDITOR
		Application.runInBackground = true;
#endif
	}

	void Start()
	{

		Debug.Log ("Starting with "+ ControllerManager.Instance.NumberOfController + " controllers.");

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
		Kart.totalPlayers = PlayerManager.Instance.CurrentPlayers.Count;
		forward = false;
		//main.CreateNPersos(PlayerManager.Instance.CurrentPlayers.Count);
		Instantiate (Resources.Load ("guiStartFire"));
		statistics.startGame ();
	}
	
	void CreateNPersos(int n)
	{
		players = new List<Kart>();
		for (int i=0; i<n; i++)
		{
			Kart a = new Kart(listRespawn[i].position, listRespawn[i].rotation, Game.Instance.Players[i]);
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