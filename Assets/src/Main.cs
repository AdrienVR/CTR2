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

	public bool inPause=false;
	public float normalTime;
	public GUIStyle gs;
	public GUIStyle gs2;
	void Start()
	{
		Debug.Log ("Demarrage !");
		Kart.SetNbPlayers (nbPlayer);
		InitializeRespawn ();
		
		CreateNPersos(nbPlayer);
		normalTime = Time.timeScale;

	}
	void Update()
	{
		if(Input.GetKeyDown(KartController.playersMapping[1]["start"]) || Input.GetKeyDown(KartController.playersMapping[2]["start"]) )
		{
			Pause ();
		}
	}

	void Pause()
	{
		if(!inPause)
		{
			Debug.Log("En pause");
			Time.timeScale=0f;
			inPause=true;
			AudioListener.pause = true;

		}
		else
		{
			Debug.Log("Reprise");
			Time.timeScale=normalTime;
			inPause=false;
			AudioListener.pause = false;
		}
	}

	void OnGUI()
	{
		if(inPause)
		{
			float widthLabel=400;
			float heightLabel=35;
			GUI.TextArea(new Rect(Screen.width/2-widthLabel/2, Screen.height/2+heightLabel/2-5*heightLabel, widthLabel, heightLabel), "Pause",gs2);
			if (GUI.Button(new Rect(Screen.width/2-widthLabel/2, Screen.height/2+heightLabel/2-4*heightLabel, widthLabel, heightLabel), "REPRENDRE",gs))
			{
				Pause();
			}
			if (GUI.Button(new Rect(Screen.width/2-widthLabel/2, Screen.height/2+heightLabel/2-3*heightLabel, widthLabel, heightLabel), "RECOMMENCER",gs))
			{
				//Application.LoadLevel(Application.loadedLevel);
			}
			if (GUI.Button(new Rect(Screen.width/2-widthLabel/2, Screen.height/2+heightLabel/2-2*heightLabel, widthLabel, heightLabel),"CHANGER PERSONNAGE" ,gs))
			{

			}
			if (GUI.Button(new Rect(Screen.width/2-widthLabel/2, Screen.height/2+heightLabel/2-1*heightLabel, widthLabel, heightLabel), "CHANGER NIVEAU",gs))
			{

			}
			if (GUI.Button(new Rect(Screen.width/2-widthLabel/2, Screen.height/2+heightLabel/2+0*heightLabel, widthLabel, heightLabel), "CHANGER CONFIG.",gs))
			{

			}
			if (GUI.Button(new Rect(Screen.width/2-widthLabel/2, Screen.height/2+heightLabel/2+1*heightLabel, widthLabel, heightLabel),"QUITTER" ,gs))
			{
				Application.Quit();
			}
			if (GUI.Button(new Rect(Screen.width/2-widthLabel/2, Screen.height/2+heightLabel/2+2*heightLabel, widthLabel, heightLabel),"OPTIONS" ,gs))
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
		for (int i=0; i<n; i++) {
			new Kart(listRespawn[i].position, listRespawn[i].rotation);
		}
	}


}