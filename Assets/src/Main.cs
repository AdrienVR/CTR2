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

	void Start()
	{
		Debug.Log ("Demarrage !");
		Kart.SetNbPlayers (nbPlayer);
		InitializeRespawn ();
		
		CreateNPersos(nbPlayer);

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