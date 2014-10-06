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
		if(Input.GetKeyDown(KeyCode.P))
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
			float widthLabel=200;
			float heightLabel=50;
			if (GUI.Button(new Rect(Screen.width/2-widthLabel/2, Screen.height/2-heightLabel/2, widthLabel, heightLabel),"QUITTER" ))
			{
				Application.Quit();
			}
			if (GUI.Button(new Rect(Screen.width/2-widthLabel/2, Screen.height/2-heightLabel/2-heightLabel, widthLabel, heightLabel), "REPRENDRE"))
			{
				Pause();
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