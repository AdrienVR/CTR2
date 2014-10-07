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
	public GUIStyle gs3; // over button pause style (for keyboard)
	private List<GUIStyle> b; // blank styles for pause button
	delegate void func();
	public bool initPause=false;
	private static Dictionary <int, string> menuDispostion =  new Dictionary<int, string> {
		{0,"REPRENDRE"},{1,"RECOMMENCER"},{2,"CHANGER PERSONNAGE"},{3,"CHANGER NIVEAU"},{4,"CHANGER CONFIG"},{5,"QUITTER"},{6,"OPTIONS"}
	};

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
		//if any player push on start : pause event
		bool start = false;
		for (int i = 1; i<5; i++)
			start |= Input.GetKeyDown (KartController.playersMapping [i] ["start"]) ;
		if(start)
			Pause ();

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
			// si on vient d'arrive en pause
			if(b[0]==gs3 && !initPause)	{
				for (int i = 0; i< 6 ; i++)
					b.Add(gs); //lol
				initPause=true;
			}
			bool down = false;
			// down means 1 player 
			for (int i = 1; i<5; i++)
				down |= Input.GetKeyDown (KartController.playersMapping [i] ["moveBack"]) ;
			if(down)
			{
				int r = b.IndexOf(gs3);
				if (r<b.Count-1)
				{
					b[r]=gs;
					b[r+1]=gs3;
				}
			}
			bool up = false;
			for (int i = 1; i<5; i++)
			{
				up |= (Input.GetAxis (KartController.axisMapping[i]["stop"]) > 0 && KartController.controllersEnabled[i]);
				up |= (Input.GetKeyDown (KartController.playersMapping [i] ["moveForward"]) && !KartController.controllersEnabled[i]);
			}
			if(up)
			{
				int r = b.IndexOf(gs3);
				if (r>0)
				{
					b[r]=gs;
					b[r-1]=gs3;
				}
			}
			bool ok = false;
			for (int i = 1; i<5; i++)
			{
				ok |= (Input.GetKeyDown (KartController.playersMapping [i] ["moveForward"]) && KartController.controllersEnabled[i]);
				ok |= (Input.GetKeyDown (KartController.playersMapping [i] ["action"]) && !KartController.controllersEnabled[i]);
			}
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
		
			for (int i = 0; i<menuDispostion.Count; i++)
			{
				if (GUI.Button(new Rect(Screen.width/2-widthLabel/2, Screen.height/2+heightLabel/2-(4-i)*heightLabel, widthLabel, heightLabel), menuDispostion[i],b[i]))
				{
					if (menuDispostion[i]=="REPRENDRE")
						Pause();
					else if (menuDispostion[i]=="QUITTER")
						Application.Quit();
				}
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