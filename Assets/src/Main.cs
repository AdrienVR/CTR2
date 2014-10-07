using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour
{
	public int nbPlayer;

	public GameObject respawn1;
	public GameObject respawn2;
	public GameObject respawn3;
	public GameObject respawn4;
	private List<Transform> listRespawn;
	public List<Kart> players;
	public bool inPause=false;
	public float normalTime;
	public GUIStyle gs; //normal button pause style
	public GUIStyle gs2; //title "pause" style
	public GUIStyle gs3; // hover button pause style (for keyboard)
	private List<GUIStyle> b; // blank styles for pause button
	delegate void func();
	public bool initPause=false;

	void Start()
	{
		Debug.Log ("Demarrage !");
		Kart.SetNbPlayers (nbPlayer);
		InitializeRespawn ();
		
		CreateNPersos(nbPlayer);
		normalTime = Time.timeScale;
		b=new List<GUIStyle>();
		b.Add(gs3);

	}
	void Update()
	{
		bool start = (Input.GetKeyDown (KartController.playersMapping [1] ["start"]) || Input.GetKeyDown (KartController.playersMapping [2] ["start"]) || Input.GetKeyDown (KartController.playersMapping [3] ["start"]) || Input.GetKeyDown (KartController.playersMapping [4] ["start"]));
		if(start)
		{
			Pause ();
		}
		keyboardMenu ();
	}

	void Pause()
	{
		if(!inPause)
		{
			Debug.Log("En pause");
			Time.timeScale=0f;
			inPause=true;
			AudioListener.pause = true;
			foreach (Kart k in players)
			{
				k.blackScreen();
			}

		}
		else
		{
			Debug.Log("Reprise");
			Time.timeScale=normalTime;
			inPause=false;
			AudioListener.pause = false;
			foreach (Kart k in players)
			{
				k.normalScreen();
			}
		}
	}

	public void keyboardMenu()
	{

		if(inPause)
		{
			if(b[0]==gs3 && !initPause)
			{
				b.Add(gs); //lol
				b.Add(gs);
				b.Add(gs);
				b.Add(gs);
				b.Add(gs);
				b.Add(gs);
				initPause=true;
			}
			bool down = (Input.GetKeyDown (KartController.playersMapping [1] ["moveBack"]) || Input.GetKeyDown (KartController.playersMapping [2] ["moveBack"]) || Input.GetKeyDown (KartController.playersMapping [3] ["moveBack"]) || Input.GetKeyDown (KartController.playersMapping [4] ["moveBack"]));
			if(down)
			{
				int r = b.IndexOf(gs3);
				if (r<b.Count-1)
				{
					b[r]=gs;
					b[r+1]=gs3;
				}
			}
			bool up = (Input.GetAxis (KartController.axisMapping[1]["stop"]) > 0 || Input.GetKeyDown(KeyCode.W) || Input.GetAxis (KartController.axisMapping[2]["stop"]) > 0 );
			if(up)
			{
				int r = b.IndexOf(gs3);
				if (r>0)
				{
					b[r]=gs;
					b[r-1]=gs3;
				}
			}
			bool ok = (Input.GetKeyDown (KartController.playersMapping [1] ["moveForward"]) || Input.GetKeyDown (KartController.playersMapping [2] ["moveForward"]) || Input.GetKeyDown (KartController.playersMapping [3] ["moveForward"]) || Input.GetKeyDown (KartController.playersMapping [4] ["moveForward"]));
			if(ok)
			{
				int r = b.IndexOf(gs3);
				switch (r)
				{
				case 0:
					Pause ();
					break;
				case 6:
					Application.Quit();
					break;
				default:
					break;
				}
			}
		}
	}

	void OnGUI()
	{
		if(inPause)
		{
			float widthLabel=400;
			float heightLabel=35;
			GUI.TextArea(new Rect(Screen.width/2-widthLabel/2, Screen.height/2+heightLabel/2-5*heightLabel, widthLabel, heightLabel), "Pause",gs2);
		
			if (GUI.Button(new Rect(Screen.width/2-widthLabel/2, Screen.height/2+heightLabel/2-4*heightLabel, widthLabel, heightLabel), "REPRENDRE",b[0]))
			{
				Pause();
			}
			if (GUI.Button(new Rect(Screen.width/2-widthLabel/2, Screen.height/2+heightLabel/2-3*heightLabel, widthLabel, heightLabel), "RECOMMENCER",b[1]))
			{
				//Application.LoadLevel(Application.loadedLevel);
			}
			if (GUI.Button(new Rect(Screen.width/2-widthLabel/2, Screen.height/2+heightLabel/2-2*heightLabel, widthLabel, heightLabel),"CHANGER PERSONNAGE" ,b[2]))
			{

			}
			if (GUI.Button(new Rect(Screen.width/2-widthLabel/2, Screen.height/2+heightLabel/2-1*heightLabel, widthLabel, heightLabel), "CHANGER NIVEAU",b[3]))
			{

			}
			if (GUI.Button(new Rect(Screen.width/2-widthLabel/2, Screen.height/2+heightLabel/2+0*heightLabel, widthLabel, heightLabel), "CHANGER CONFIG.",b[4]))
			{

			}
			if (GUI.Button(new Rect(Screen.width/2-widthLabel/2, Screen.height/2+heightLabel/2+1*heightLabel, widthLabel, heightLabel),"QUITTER" ,b[5]))
			{
				Application.Quit();
			}
			if (GUI.Button(new Rect(Screen.width/2-widthLabel/2, Screen.height/2+heightLabel/2+2*heightLabel, widthLabel, heightLabel),"OPTIONS" ,b[6]))
			{

			}
		}
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
			Kart a = new Kart(listRespawn[i].position, listRespawn[i].rotation);
			players.Add(a);
		}
	}


}