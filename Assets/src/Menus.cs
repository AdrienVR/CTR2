using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Menus : MonoBehaviour
{
	public Texture normal;
	public Texture hover;
	public static GameObject cameraMenu;
	private GameObject greyT;
	private float normalTime;
	private int heightLabel=100;
	private int position=0;
	private bool authorizeNavigate = false;
	private bool inPause = false;
	private static GameObject titreAffiche;
	private static List <GameObject> textureAffichees =  new List <GameObject>();
	private static List <GameObject> textAffiches =  new List <GameObject>();
	private static Dictionary <int, string> menuCourant = new Dictionary<int, string>();

	// menus :
	private static Dictionary <int, string> menuPause =  new Dictionary<int, string>
	{
		{0,"Start"},
		{1,"REPRENDRE"},
		{2,"RECOMMENCER"},
		{3,"CHANGER PERSONNAGE"},
		{4,"CHANGER NIVEAU"},
		{5,"CHANGER CONFIG"},
		{6,"QUITTER"},
		{7,"OPTIONS"}
	};
	private static Dictionary <int, string> menuOptions =  new Dictionary<int, string>
	{
		{0,"Options"},
		{1,"VOLUME"},
		{2,"REGLAGES CONTROLES"},
		{3,"RETOUR"}
	};

	// Use this for initialization
	void Start ()
	{
		normalTime = Time.timeScale;
		cameraMenu = (GameObject)GameObject.Instantiate (Resources.Load("cameraMenu"));
	}

	
	// Update is called once per frame
	void Update ()
	{
		navigateMenu ();
		testPause ();

	}

	void testPause()
	{
		bool pressStart = false;
		for (int i = 1; i<5; i++) pressStart |= Input.GetKeyDown (KartController.playersMapping [i] ["start"]) ;
		if (pressStart)
		{
			Pause();
		}
	}

	void Pause()
	{
		if (!inPause)
		{
			greyT = (GameObject)GameObject.Instantiate(Resources.Load("guiTexture1"));
			inPause=true;
			displayMenu(menuPause);
			Time.timeScale=0f;
			AudioListener.pause = true;
		}
		else if (inPause)
		{
			inPause=false;
			viderMenu ();
			Destroy(greyT);
			Time.timeScale=normalTime;
			AudioListener.pause = false;
		}

	}
	void displayMenu(Dictionary <int, string> menu)
	{
		viderMenu ();
		cameraMenu.SetActive (true);
		position = 0;
		GameObject textTitre =(GameObject)Instantiate (Resources.Load ("textTitreMenu"),new Vector3(0.5f,0.5f+(((float)heightLabel/2)/(float)Screen.height)*menu.Count/2f,0),Quaternion.identity);
		textTitre.guiText.text=menu[0];
		titreAffiche = textTitre;
		for (int i = 1; i<menu.Count; i++)
		{
			Vector3 pos =new Vector3(0.5f,0.5f+(((float)heightLabel/2)/(float)Screen.height)*(menu.Count/2-i),-1);
			textureAffichees.Add((GameObject)Instantiate (Resources.Load ("button"),pos,Quaternion.identity));
			GameObject textbutton =(GameObject)Instantiate (Resources.Load ("textButton"),new Vector3(pos.x,pos.y,0),Quaternion.identity);
			textbutton.guiText.text=menu[i];
			textAffiches.Add(textbutton);
		}
		menuCourant = menu;
		authorizeNavigate = true;
	}

	void navigateMenu()
	{
		if(authorizeNavigate)
		{
			if(textureAffichees.Count>position)
			{
				for(int i=0; i<textAffiches.Count;i++)
				{
					textureAffichees [i].guiTexture.texture = normal;
					textureAffichees [position].guiTexture.texture = hover;
					textAffiches[i].guiText.color=Color.white;
					textAffiches[position].guiText.color=Color.black;

				}
			}
			bool down = false;
			for (int i = 1; i<5; i++)
			{
				down |= (Input.GetAxis (KartController.axisMapping[i]["stop"]) == 1 && KartController.controllersEnabled[i]);
				down |= (Input.GetKeyDown (KartController.playersMapping [i] ["moveBack"]) && !KartController.controllersEnabled[i]);
			}
			if (down && position<menuCourant.Count-2) position++;
			bool up = false;
			for (int i = 1; i<5; i++)
			{
				up |= (Input.GetAxis (KartController.axisMapping[i]["stop"]) == -1 && KartController.controllersEnabled[i]);
				up |= (Input.GetKeyDown (KartController.playersMapping [i] ["moveForward"]) && !KartController.controllersEnabled[i]);
			}
			if (up && position>0) position--;
			bool ok = false;
			for (int i = 1; i<5; i++)
			{
				ok |= (Input.GetKeyDown (KartController.playersMapping [i] ["moveForward"]) && KartController.controllersEnabled[i]);
				ok |= (Input.GetKeyDown (KartController.playersMapping [i] ["action"]) && !KartController.controllersEnabled[i]);
			}
			if(ok)
			{
				action(menuCourant,position);
			}
		}
	}

	void action(Dictionary <int, string> menu,int p)
	{
		if(menu[0]=="Start")
		{
			switch (menu[p+1])
			{
			case "REPRENDRE":
				Pause();
				break;
			case "RECOMMENCER":
				Application.LoadLevel(Application.loadedLevel);
				Kart.nPlayer=0;
				KartController.stop = true;
				viderMenu();
				Destroy(greyT);
				Time.timeScale=normalTime;
				AudioListener.pause = false;
				break;
			case "OPTIONS":
				displayMenu(menuOptions);
				break;
			default:
				break;
			}
		}
		if(menu[0]=="Options")
		{
			switch (menu[p+1])
			{
			case "RETOUR":
				displayMenu(menuPause);
				break;
			default:
				break;
			}
		}
	}

	void viderMenu()
	{
		authorizeNavigate = false;
		Destroy (titreAffiche);
		foreach (GameObject g in textureAffichees) Destroy (g);
		foreach (GameObject g in textAffiches) Destroy (g);
		textureAffichees =  new List <GameObject>();
		textAffiches =  new List <GameObject>();
		cameraMenu.SetActive (false);
	}

}
