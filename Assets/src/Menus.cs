using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Menus : MonoBehaviour
{
	
	public Texture normal;
	public Texture hover;
	public Texture triVolume3;
	public Texture triVolume2;
	public Texture triVolume1;
	public static GameObject cameraMenu;
	private GameObject greyT;
	private float normalTime;
	private int heightLabel=100;
	private int position=0;
	private int positionH=1;
	private bool authorizeNavigate = false;
	private bool waitingForKey = false;
	private bool inPause = false;
	private static GameObject titreAffiche;
	private static GameObject nameMap;
	private static GameObject triangleFond;
	private static GameObject triangleVolume;
	private static GameObject textPlayer;
	private static GameObject fleches;
	private static List <GameObject> flechesD =  new List <GameObject>();
	private static List <GameObject> textureAffichees =  new List <GameObject>();
	private static List <GameObject> textAffiches =  new List <GameObject>();
	private static List <GameObject> controlAffiches =  new List <GameObject>();
	private static List <string> menuCourant = new List <string>();
	private static Dictionary <string, bool> booleans = new Dictionary<string, bool>{
		{"up", false},{"down", false},{"left", false},{"right", false},
		{"ok",false}, {"back", false},{"start", false},
		{"up_down", false},{"down_down", false},{"left_down", false},{"right_down", false}
	};
	public Main main;
	public Kart winner;
	public List <Kart> loosers=  new List <Kart>();
	public GameObject cadre1;
	public GameObject cadre5;

	private bool readyToMove = true;
	private bool lockMove = false;
	private int j = 5;
	public bool falseok=true;
	public int numSelection = 1;
	private static List <GameObject> tempAffiches =  new List <GameObject>();
	private static List<bool> configWeaponsStates; // /!\
	private static List <string> listMapForMenu =  new List <string>(){"Parking","Skull Rock"};
	
	// variables statiques pour la config d'une battle
	private static List<string> persos=new List <string>();
	private static List <string> weapons =  new List <string>();
	private static string map;
	private static int nbPts=8;

	// menus :
	private static List <string> menuPause =  new List <string> 
	{
		"Start",
		"REPRENDRE",
		"RECOMMENCER",
		"CHANGER PERSONNAGE",
		"CHANGER NIVEAU",
		"CHANGER CONFIG",
		"QUITTER",
		"OPTIONS"
	};
	private static List <string>  menuOptions =  new List <string> 
	{
		"Options",
		"VOLUME :",
		"REGLAGES CONTROLES",
		"RETOUR"
	};
	private static List <string>  menuReglages =  new List <string> 
	{
		"Reglages Controles",
		"JOUEUR :",
		"Avancer",
		"Actionner Arme",
		"Reculer",
		"Gauche",
		"Droite",
		"Sauter",
		"Inverser Camera",
		"Changer Vue",
		"Mettre en Pause",
		"RETOUR"
	};
	private static List <string> menuFin =  new List <string> 
	{
		"Fin",
		"RECOMMENCER",
		"STATISTIQUES",
		"CHANGER PERSONNAGE",
		"CHANGER NIVEAU",
		"CHANGER CONFIG",
		"QUITTER"
	};
	public static List <string> mainMenu = new List <string> 
	{
		"Crash Team Racing II",
		"BATAILLE",
		"CREDITS",
		"QUITTER"
	};
	private static List <string> menuCredits = new List <string> 
	{
		"Credits",
		"RETOUR"
	};
	private static List <string> menuPersos = new List <string> 
	{
		"Personnages",
		"Crash",
		"Coco",
		"Crash",
		"Crash",
		"VALIDER",
		"RETOUR"
	};
	private static List <string> menuConfig = new List <string> 
	{
		"Choix des Armes",
		"Bouclier",
		"Bombe",
		"Bombe x 3",
		"Potion",
		"Turbo",
		"Missile",
		"Missile x 3",
		"Aku-Aku",
		"TNT",
		"8",
		"VALIDER",
		"RETOUR"
	};
	private static List <string> menuMaps =  new List <string> 
	{
		"Niveaux",
		"VALIDER",
		"RETOUR"
	};

	public static List <string> menuToGo = mainMenu;

	private static bool m_translationDone = false;

	private static List<List<string>> translatableLists;

	//called before start
	void Awake()
	{
        m_menuParent = new GameObject("Menu").transform;

        if (m_translationDone == false)
		{
			translatableLists = new List<List<string>>
			{
				menuConfig, menuCourant, menuCredits, menuFin, 
				menuMaps, menuOptions, menuPause, menuPersos,
				menuReglages, menuToGo
			};

			foreach(List<string> list in translatableLists)
			{
				for (int i = 0; i < list.Count; i++)
				{
					list[i] = tr(list[i]);
				}
			}
			m_translationDone = true;
		}
	}

	void InitReglageMenu()
	{
		menuReglages =  new List <string> 
		{
			tr("Reglages Controles"),
			tr("JOUEUR :"),
			tr("RETOUR")
		};
		foreach(string action in ControllerResources.ActionNames)
		{
			menuReglages.Insert(2, tr(action));
		}
		configActionNames = new List<string>(new string[ControllerResources.ActionNames.Count + 3]);
	}

	List<string> configActionNames;
    private Transform m_menuParent;

    void UpdateReglageMenu(ControllerBase controller)
	{
		configActionNames[1] = tr("Joueur")+" "+positionH;
		for(int i = 0 ; i < ControllerResources.ActionNames.Count ; i++)
		{
			string action = ControllerResources.ActionNames[i];
			menuReglages[i + 2] = tr (action);
			configActionNames[i + 2] = controller.GetNameKey(action);
		}

	}

	// Use this for initialization
	void Start ()
	{
		InitReglageMenu();
		if (Application.loadedLevelName == "mainmenu") configWeaponsStates= new List <bool> ();
		normalTime = Time.timeScale;
		cameraMenu = (GameObject)GameObject.Instantiate (Resources.Load("cameraMenu"));
		//AudioListener.volume = 0.5f;
		//soundUp =(AudioClip)Instantiate (Resources.Load ("Audio/down_menu"));
		if(textureAffichees.Count>position && textureAffichees[position])
			cadre1 = (GameObject)GameObject.Instantiate (Resources.Load ("cadre1"), textureAffichees [position].transform.position, Quaternion.identity);
		cadre5=(GameObject)Instantiate (Resources.Load ("cadre5"),new Vector3(0.85f,0.5f,0),Quaternion.identity);
		if(cadre5)
			cadre5.GetComponent<GUITexture>().enabled = false;
		if(cadre1)
		{
			cadre1.GetComponent<GUITexture>().enabled = false;
			foreach( Transform child in cadre1.transform)
			{	
				child.GetComponent<GUITexture>().enabled = false;
			}
		}
		if(menuToGo==menuPersos)
		{
			persos=new List<string>();
			numSelection=1;
			falseok=false;
			if(cadre1) cadre1.GetComponent<GUITexture>().enabled=true;
			if(cadre5) cadre5.GetComponent<GUITexture>().enabled = true;
			configWeaponsStates=new List<bool>();
			weapons = new List<string>();
		}
		if(menuToGo==menuMaps)
		{
			if(cadre1) cadre1.GetComponent<GUITexture>().enabled = false;
			j = 5;
			configWeaponsStates=new List<bool>();
			weapons = new List<string>();
		}
		if(menuToGo==menuConfig)
		{
			falseok=true;
			if(textureAffichees [position]) cadre1 = (GameObject)GameObject.Instantiate (Resources.Load ("cadre6"), textureAffichees [position].transform.position+new Vector3(0,0,2f), Quaternion.identity);
			if(cadre1) cadre1.GetComponent<GUITexture>().enabled = true;
			weapons=new List<string>();	
			configWeaponsStates=new List<bool>();
		}
	}

	// Update is called once per frame
	void Update ()
	{
		//Debug.Log (configWeaponsStates.Count);
		if (waitingForKey == false)
		{
			CheckKeys();

			if (readyToMove == true && lockMove == false)
				navigateMenu ();

			ResetBooleans();
		}
		else
		{
			if (ControllerManager.Instance.GetController(positionH - 1).newKey != null)
			{
				StartCoroutine(GetKey());
				ControllerManager.Instance.GetController(positionH - 1).newKey = null;
			}
		}
		if(main!=null)
		{
			testPause ();
			testEnd ();
		}
	}

	void ResetBooleans(){
		var buffer = new List<string>(booleans.Keys);
		foreach (string key in buffer){
			booleans[key] = false;
		}
	}
	
	void testPause()
	{
		if (ControllerManager.Instance.GetKeyDown("start"))
		{
			AudioManager.Instance.Play("validateMenu");
			Pause();
		}
	}
	
	void testEnd()
	{
		foreach(Kart k in main.players)
		{
			if(k.isWinner)
			{
				k.isWinner=false;
				winner = k;
				greyT = (GameObject)GameObject.Instantiate(Resources.Load("guiBlack"));
				inPause=true;
				StartCoroutine(waitAndPause());
				Main.statistics.getStatPerso(k.numeroJoueur).score=k.nbPoints;
			}
			if(winner!=null)
			{
				k.c2d.GetComponent<Camera>().enabled=false;
				if(k != winner)
				{
					Main.statistics.getStatPerso(k.numeroJoueur).score=k.nbPoints;
					k.camera.GetComponent<Camera>().rect=new Rect ();
					if(loosers.IndexOf(k)==-1)
						loosers.Add(k);
				}
			}
		}
	}
	
	IEnumerator waitAndPause()
	{
		Instantiate (Resources.Load ("videoFond"));
		KartController.stop = true;
		
		StartCoroutine(interpolateCamera());
		yield return new WaitForSeconds (1);
		displayMenu(menuFin);
		
		Destroy (greyT);
		GameObject j1 =(GameObject)Instantiate (Resources.Load ("textTitreMenu"),new Vector3(0.27f,0.35f,0),Quaternion.identity);
		string player = tr("Joueur") + " ";
		j1.GetComponent<GUIText>().text = player+winner.numeroJoueur+" : "+winner.nbPoints+" Pts";
		Main.statistics.score.Add (player + winner.numeroJoueur, winner.nbPoints);
		j1.GetComponent<GUIText>().color = Color.yellow;
		loosers = loosers.OrderBy(x => x.nbPoints).ToList();
		loosers.Reverse ();
		foreach(Kart k in loosers)
		{
			GameObject j =(GameObject)Instantiate (Resources.Load ("textTitreMenu"),new Vector3(0.27f,0.35f-0.08f*(loosers.IndexOf(k)+1),0),Quaternion.identity);
			j.GetComponent<GUIText>().text = player+k.numeroJoueur+" : "+k.nbPoints+" Pts";
			Main.statistics.score.Add(player+k.numeroJoueur,k.nbPoints);
		}
		KartController.stop = false;
		winner.kart_script.GetTransform().position = main.listRespawn [0].position;
		winner.kart_script.GetTransform().rotation =main.listRespawn[0].rotation;
		AI.kart = winner.kart_script.gameObject;
		//AI.children = winner.kc.wheels ["wheelAL"];
		AI.wheels = winner.kart_script.wheels;
		Main.statistics.getReport (true);
		//main.executeIA winner.kc.wheels ();
	}
	
	
	IEnumerator interpolateCamera()
	{
		Rect obj = new Rect (0.04f, 0.5f, 0.5f, 0.4f);
		float ellapsed = 0;
		float total = 200f;
		float lastTime = Time.time;
		float percent=0f;
		while(ellapsed < total)
		{
			percent = ellapsed / total;
			ellapsed += (Time.time - lastTime);
			yield return new WaitForSeconds (0.01f);
			winner.camera.GetComponent<Camera>().rect = Lerp(winner.camera.GetComponent<Camera>().rect,obj, percent); 
		}
	}
	
	Rect Lerp(Rect a, Rect b, float f)
	{
		Rect s = new Rect (a.xMin + f * (b.xMin - a.xMin), a.yMin + f * (b.yMin - a.yMin),
		                   a.width + f * (b.width - a.width), a.height + f * (b.height - a.height));
		
		return s;
	}
	
	void Pause()
	{
		if (!inPause)
		{
			greyT = (GameObject)GameObject.Instantiate(Resources.Load("guiBlack"));
			inPause=true;
			displayMenu(menuPause);
			Time.timeScale=0f;
			AudioManager.Instance.SetCategoryVolume("musics", 0.25f);
			Main.statistics.pauseGame(true);
			//AudioListener.pause = true;
		}
		else if (inPause)
		{
			Main.statistics.pauseGame(false);
			inPause=false;
			viderMenu ();
			Destroy(greyT);
			Time.timeScale=normalTime;
			AudioManager.Instance.SetCategoryVolume("musics", 1f);
			//AudioListener.pause = false;
		}
		
	}
	public void displayMenu(List<string> menu)
	{
		viderMenu ();
		if(cameraMenu) cameraMenu.SetActive (true);
		position = 0;
		GameObject textTitre =(GameObject)Instantiate (Resources.Load ("textTitreMenu"),new Vector3(0.5f,0.5f+(((float)heightLabel/2)/(float)Screen.height)*menu.Count/2f,0),Quaternion.identity);
		textTitre.GetComponent<GUIText>().text=menu[0];
		titreAffiche = textTitre;
		for (int i = 1 ; i < menu.Count ; i++)
		{
			if(specialButton(menu,i) == false)
			{
				Vector3 pos =new Vector3(0.5f,0.5f+(((float)heightLabel/2)/(float)Screen.height)*(menu.Count/2-i),-1);
				textureAffichees.Add((GameObject)Instantiate (Resources.Load ("menuButton"),pos,Quaternion.identity));
				GameObject textbutton =(GameObject)Instantiate (Resources.Load ("textButton"),new Vector3(pos.x,pos.y,0),Quaternion.identity);
                textbutton.transform.parent = m_menuParent;
                textbutton.GetComponent<GUIText>().text=menu[i];
				textAffiches.Add(textbutton);
			}
		}
		menuCourant = menu;
		authorizeNavigate = true;
	}

	string tr(string original)
	{
		return TranslationResources.GetTraductionOf(original);
	}
	
	bool specialButton(List <string> menu, int i)
	{
		if(menu[i]==tr("VOLUME :"))
		{
			Vector3 pos =new Vector3(0.5f,0.5f+(((float)heightLabel/2)/(float)Screen.height)*(menu.Count/2-i),-1);
            GameObject newButton = (GameObject)Instantiate(Resources.Load("menuButton"), pos, Quaternion.identity);
            newButton.transform.parent = m_menuParent;
            textureAffichees.Add(newButton);
			triangleFond = (GameObject)Instantiate (Resources.Load ("menuBackgroundTriangle"),new Vector3(pos.x,pos.y,2),Quaternion.identity);
			triangleVolume = (GameObject)Instantiate (Resources.Load ("menuVolumeTriangle"),new Vector3(pos.x,pos.y,3),Quaternion.identity);
			Rect t = new Rect(triangleVolume.GetComponent<GUITexture>().pixelInset.x,triangleVolume.GetComponent<GUITexture>().pixelInset.y,250*AudioListener.volume*1.42f,25*AudioListener.volume*1.42f);
			triangleVolume.GetComponent<GUITexture>().pixelInset=t;
			GameObject textbutton =(GameObject)Instantiate (Resources.Load ("textButton"),new Vector3(pos.x-(float)((float)400/(float)((float)Screen.width*(float)3)),pos.y,0),Quaternion.identity);
			textbutton.GetComponent<GUIText>().text=menu[i];
            textbutton.transform.parent = m_menuParent;
            textAffiches.Add(textbutton);
			return true;
		}
		else if(menu[0]==tr("Reglages Controles") && menu[i]!=tr("RETOUR"))
		{
			positionH = main.players[0].numeroJoueur;
			UpdateReglageMenu(ControllerManager.Instance.GetController(positionH - 1));

			if(menu[i]==tr("JOUEUR :"))
			{
				Vector3 pos =new Vector3(0.5f,0.5f+(((float)heightLabel/2)/(float)Screen.height)*(menu.Count/2-i),-1);
                GameObject newButton = (GameObject)Instantiate(Resources.Load("menuButton"), pos, Quaternion.identity);
                newButton.transform.parent = m_menuParent;
                textureAffichees.Add(newButton);

                fleches = (GameObject)Instantiate (Resources.Load ("menuFleches"),new Vector3(pos.x+(float)((float)400/(float)((float)Screen.width*(float)5)),pos.y,5),Quaternion.identity);
				GameObject textbutton =(GameObject)Instantiate (Resources.Load ("textButton"),new Vector3(pos.x-(float)((float)400/(float)((float)Screen.width*(float)4)),pos.y,0),Quaternion.identity);
				textbutton.GetComponent<GUIText>().text=menu[i];
				textAffiches.Add(textbutton);
                textbutton.transform.parent = m_menuParent;

                textPlayer = (GameObject)Instantiate (Resources.Load ("textButton"),new Vector3(pos.x+(float)((float)400/(float)((float)Screen.width*(float)5)),pos.y,6),Quaternion.identity);
				textPlayer.GetComponent<GUIText>().text = configActionNames[i];
			}
			else if(i > 1)
			{
				Vector3 pos =new Vector3(0.5f,0.5f+(((float)heightLabel/2)/(float)Screen.height)*(menu.Count/2-i),-1);
                GameObject newButton = (GameObject)Instantiate(Resources.Load("menuButton"), pos, Quaternion.identity);
                newButton.transform.parent = m_menuParent;
                textureAffichees.Add(newButton);

                GameObject textbutton =(GameObject)Instantiate (Resources.Load ("textButton"),new Vector3(pos.x-(float)((float)400/(float)((float)Screen.width*(float)2.2f)),pos.y,0),Quaternion.identity);
				textbutton.GetComponent<GUIText>().text=menu[i];
				textbutton.GetComponent<GUIText>().anchor=TextAnchor.MiddleLeft;
				textAffiches.Add(textbutton);
                textbutton.transform.parent = m_menuParent;

                GameObject textcontrol =(GameObject)Instantiate (Resources.Load ("textControl"),new Vector3(pos.x+(float)((float)400/(float)((float)Screen.width*(float)5)),pos.y,0),Quaternion.identity);
				textcontrol.GetComponent<GUIText>().text = configActionNames[i];
				controlAffiches.Add(textcontrol);

				GameObject flecheD = (GameObject)Instantiate (Resources.Load ("menuFlecheD"),new Vector3(pos.x+(float)((float)400/(float)((float)Screen.width*(float)2.5f)),pos.y,5),Quaternion.identity);
				flechesD.Add(flecheD);
			}
			return true;
		}
		else if(menu[0]==tr("Fin"))
		{
			if(titreAffiche.transform.position.x != 0.8f)
				titreAffiche.transform.position = new Vector3(0.8f,0.5f+(((float)heightLabel/2)/(float)Screen.height)*7/2f,0);
			Vector3 pos =new Vector3(0.8f,0.5f+(((float)heightLabel/2)/(float)Screen.height)*(menu.Count/2-i),-1);
            GameObject newButton = (GameObject)Instantiate(Resources.Load("menuButton"), pos, Quaternion.identity);
            newButton.transform.parent = m_menuParent;
            textureAffichees.Add(newButton);
            GameObject textbutton =(GameObject)Instantiate (Resources.Load ("textButton"),new Vector3(pos.x,pos.y,0),Quaternion.identity);
			textbutton.GetComponent<GUIText>().text=menu[i];
			textAffiches.Add(textbutton);
            textbutton.transform.parent = m_menuParent;
            return true;
		}
		else if(menu[0]==mainMenu[0])
		{		
			if(i==1) triangleFond=(GameObject)GameObject.Instantiate (Resources.Load("404"));
			titreAffiche.transform.position = new Vector3(0.5f,0.5f+(((float)heightLabel/2)/(float)Screen.height)*7/2f,0);
			Vector3 pos =new Vector3(0.8f,0.5f+(((float)heightLabel/2)/(float)Screen.height)*(menu.Count/2-i),-1);
            GameObject newButton = (GameObject)Instantiate(Resources.Load("menuButton"), pos, Quaternion.identity);
            newButton.transform.parent = m_menuParent;
            textureAffichees.Add(newButton);
            GameObject textbutton =(GameObject)Instantiate (Resources.Load ("textButton"),new Vector3(pos.x,pos.y,0),Quaternion.identity);
			textbutton.GetComponent<GUIText>().text=menu[i];
			textAffiches.Add(textbutton);
            textbutton.transform.parent = m_menuParent;
            return true;
		}
		else if(menu[0]==menuCredits[0])
		{
			titreAffiche.transform.position = new Vector3(0.5f,0.55f+(((float)heightLabel/2)/(float)Screen.height)*7/2f,0);
			Vector3 pos =new Vector3(0.5f,0.15f+(((float)heightLabel/2)/(float)Screen.height)*(menu.Count/2-i),-1);
            GameObject newButton = (GameObject)Instantiate(Resources.Load("menuButton"), pos, Quaternion.identity);
            newButton.transform.parent = m_menuParent;
            textureAffichees.Add(newButton);
            GameObject textbutton =(GameObject)Instantiate (Resources.Load ("textButton"),new Vector3(pos.x,pos.y,0),Quaternion.identity);
			textbutton.GetComponent<GUIText>().text=menu[i];
			textAffiches.Add(textbutton);
            textbutton.transform.parent = m_menuParent;
            return true;
		}
		else if(menu[0]==menuMaps[0])
		{
			titreAffiche.transform.position = new Vector3(0.5f,0.55f+(((float)heightLabel/2)/(float)Screen.height)*7/2f,0);
			Vector3 pos =new Vector3(0.5f,0.15f+(((float)heightLabel/2)/(float)Screen.height)*(menu.Count/2-i),-1);
			if(i==1)
			{
				positionH=0;
				nameMap =(GameObject)Instantiate (Resources.Load ("textButton"),new Vector3(pos.x,0.25f,0),Quaternion.identity);
				nameMap.GetComponent<GUIText>().text=Game.listMapForMenu[positionH];
				fleches=(GameObject)Instantiate (Resources.Load ("menuFleches"),new Vector3(pos.x,0.25f,0),Quaternion.identity);
				
            }
            GameObject newButton = (GameObject)Instantiate(Resources.Load("menuButton"), pos, Quaternion.identity);
            newButton.transform.parent = m_menuParent;
            textureAffichees.Add(newButton);
            GameObject textbutton =(GameObject)Instantiate (Resources.Load ("textButton"),new Vector3(pos.x,pos.y,0),Quaternion.identity);
			textbutton.GetComponent<GUIText>().text=menu[i];
			textAffiches.Add(textbutton);
            textbutton.transform.parent = m_menuParent;
            return true;
		}
		else if(menu[0]==menuPersos[0])
		{	
			titreAffiche.transform.position = new Vector3(0.5f,0.5f+(((float)heightLabel/2)/(float)Screen.height)*7/2f,0);
			if(i>0 && i<menuPersos.Count()-2)
			{
				Vector3 pos =new Vector3(0.5f-(((float)70)/(float)Screen.width)*2*(((float)(menu.Count-4)/2f-i)+1),0.3f,-1);
				GameObject p;
				switch (i)
				{
				case 1: //crash
					p =(GameObject)Instantiate (Resources.Load ("iconCrash"),pos,Quaternion.identity);
					break;
				case 2://coco
					p =(GameObject)Instantiate (Resources.Load ("iconCoco"),pos,Quaternion.identity);
					break;
				case 3://crash
					p =(GameObject)Instantiate (Resources.Load ("iconCrash"),pos,Quaternion.identity);
					break;
				case 4://crash
					p =(GameObject)Instantiate (Resources.Load ("iconCrash"),pos,Quaternion.identity);
					break;
				default:
					p=new GameObject();
					break;
				}

				textureAffichees.Add(p);
				GameObject textbutton =(GameObject)Instantiate (Resources.Load ("textButton"),new Vector3(pos.x,pos.y-(float)80/(float)Screen.width,0),Quaternion.identity);
				textbutton.GetComponent<GUIText>().text=menu[i];
				textAffiches.Add(textbutton);
                textbutton.transform.parent = m_menuParent;
            }
			else
			{
				Vector3 pos =new Vector3(0.5f,0.5f+(((float)heightLabel/2)/(float)Screen.height)*(1-j),-1);
				if(j==5) j++;
				else if(j==6) j--;
                GameObject newButton = (GameObject)Instantiate(Resources.Load("menuButton"), pos, Quaternion.identity);
                newButton.transform.parent = m_menuParent;
                textureAffichees.Add(newButton);
                GameObject textbutton =(GameObject)Instantiate (Resources.Load ("textButton"),new Vector3(pos.x,pos.y,0),Quaternion.identity);
				textbutton.GetComponent<GUIText>().text=menu[i];
				textAffiches.Add(textbutton);
			}
			return true;
		}
		else if(menu[0]==menuConfig[0])
		{	
			titreAffiche.transform.position = new Vector3(0.5f,0.5f+(((float)heightLabel/2)/(float)Screen.height)*7/2f,0);
			if(i==1)
			{
				for(int j=0;j<9;j++)
				{
					configWeaponsStates.Add(true);
					Debug.Log("AJOUT D'UN BOOLEEN");
				}
				GameObject textTitre =(GameObject)Instantiate (Resources.Load ("textTitreMenu"),new Vector3(0.5f,0.4f,2f),Quaternion.identity);
				textTitre.GetComponent<GUIText>().text=tr("Nombre de Points");
				triangleFond = textTitre;
				position=menuConfig.Count-3;
			}
			if(i>0 && i<menuConfig.Count()-3)
			{
				Vector3 pos =new Vector3(0.5f-(((float)70)/(float)Screen.width)*2*(((float)(menu.Count-5)/2f-i)+1),0.65f,-1);
				GameObject p;
				switch (i)
				{
				case 1: 
					p =(GameObject)Instantiate (Resources.Load ("iconBouclier"),pos,Quaternion.identity);
					break;
				case 2:
					p =(GameObject)Instantiate (Resources.Load ("iconBombe"),pos,Quaternion.identity);
					break;
				case 3:
					p =(GameObject)Instantiate (Resources.Load ("iconBombex3"),pos,Quaternion.identity);
					break;
				case 4:
					p =(GameObject)Instantiate (Resources.Load ("iconPotion"),pos,Quaternion.identity);
					break;
				case 5:
					p =(GameObject)Instantiate (Resources.Load ("iconTurbo"),pos,Quaternion.identity);
					break;
				case 6:
					p =(GameObject)Instantiate (Resources.Load ("iconMissile"),pos,Quaternion.identity);
					break;
				case 7:
					p =(GameObject)Instantiate (Resources.Load ("iconMissilex3"),pos,Quaternion.identity);
					break;
				case 8:
					p =(GameObject)Instantiate (Resources.Load ("iconAkuaku"),pos,Quaternion.identity);
					break;
				case 9:
					p =(GameObject)Instantiate (Resources.Load ("iconTnt"),pos,Quaternion.identity);
					break;
				default:
					p=new GameObject();
					break;
				}
				GameObject q =(GameObject)Instantiate (Resources.Load ("Fv"),pos,Quaternion.identity);
				textureAffichees.Add(p);
				tempAffiches.Add(q);
				GameObject textbutton =(GameObject)Instantiate (Resources.Load ("textButton"),new Vector3(pos.x,pos.y-(float)80/(float)Screen.width,0),Quaternion.identity);
				textbutton.GetComponent<GUIText>().text=menu[i];
				textAffiches.Add(textbutton);
                textbutton.transform.parent = m_menuParent;
            }
			else if (i==menuConfig.Count()-3)
			{
				Vector3 pos =new Vector3(0.5f,0.65f+(((float)heightLabel/2)/(float)Screen.height)*(1-j),-1);
				j++;
				GameObject r =(GameObject)Instantiate (Resources.Load ("menuButtonPetit"),pos,Quaternion.identity);
                r.transform.parent = m_menuParent;
                r.GetComponent<GUITexture>().pixelInset.Set(-25,-25,50,50);
				fleches=(GameObject)Instantiate (Resources.Load ("menuFleches"),pos,Quaternion.identity);
				textureAffichees.Add(r);
				GameObject textbutton =(GameObject)Instantiate (Resources.Load ("textButton"),new Vector3(pos.x,pos.y,0),Quaternion.identity);
				textbutton.GetComponent<GUIText>().text=menu[i];
				textAffiches.Add(textbutton);
                textbutton.transform.parent = m_menuParent;
            }
			else
			{
				Vector3 pos =new Vector3(0.5f,0.6f+(((float)heightLabel/2)/(float)Screen.height)*(1-j),-1);
				if(j==7) j--;
				else j++;
                GameObject newButton = (GameObject)Instantiate(Resources.Load("menuButton"), pos, Quaternion.identity);
                newButton.transform.parent = m_menuParent;
                textureAffichees.Add(newButton);
                GameObject textbutton =(GameObject)Instantiate (Resources.Load ("textButton"),new Vector3(pos.x,pos.y,0),Quaternion.identity);
				textbutton.GetComponent<GUIText>().text=menu[i];
				textAffiches.Add(textbutton);
                textbutton.transform.parent = m_menuParent;
            }
			return true;
		}
		else return false;
	}
	
	void navigateMenu()
	{
		if(authorizeNavigate)
		{
			if(position < textureAffichees.Count)
			{
				if(menuCourant[0]!=menuPersos[0] && menuCourant[0]!=menuConfig[0])
				{
					for(int i=0; i<textAffiches.Count;i++)
					{
						textureAffichees [i].GetComponent<GUITexture>().texture = normal;
						textureAffichees [position].GetComponent<GUITexture>().texture = hover;
						textAffiches[i].GetComponent<GUIText>().color=Color.white;
						textAffiches[position].GetComponent<GUIText>().color=Color.black;
						if(menuCourant[0]==tr("Reglages Controles"))
						{
							textPlayer.GetComponent<GUIText>().color=Color.white; 
							if(menuCourant[position+1]==tr("JOUEUR :"))
							{
								textPlayer.GetComponent<GUIText>().color=Color.black;
							}
							if(i<controlAffiches.Count) controlAffiches[i].GetComponent<GUIText>().color=Color.white;
							if(position>0 && position<menuCourant.Count-2)
							{
								controlAffiches[position-1].GetComponent<GUIText>().color=Color.black;
							}
						}
					}
				}
				else if (menuCourant[0]==menuConfig[0])
				{
					textureAffichees [menuConfig.Count-3].GetComponent<GUITexture>().texture = normal;
					textureAffichees [menuConfig.Count-2].GetComponent<GUITexture>().texture = normal;
					textureAffichees [menuConfig.Count-4].GetComponent<GUITexture>().texture = normal;
					textAffiches[menuConfig.Count-3].GetComponent<GUIText>().color=Color.white;
					textAffiches[menuConfig.Count-2].GetComponent<GUIText>().color=Color.white;
					textAffiches[menuConfig.Count-4].GetComponent<GUIText>().color=Color.white;
					textAffiches[menuConfig.Count-4].GetComponent<GUIText>().text=nbPts.ToString();
					if(position>=menuConfig.Count-4)
					{
						textureAffichees [position].GetComponent<GUITexture>().texture = hover;
						textAffiches[position].GetComponent<GUIText>().color=Color.black;
						cadre1.GetComponent<GUITexture>().enabled = false;
					}
					else if (cadre1)
					{
						cadre1.transform.position=textureAffichees[position].transform.position+new Vector3(0,0,2f);
						cadre1.GetComponent<GUITexture>().enabled = true;
						foreach( Transform child in cadre1.transform)
						{	
							child.GetComponent<GUITexture>().enabled = true;
						}
					}
				}
				else
				{
					textureAffichees [menuPersos.Count-3].GetComponent<GUITexture>().texture = normal;
					textureAffichees [menuPersos.Count-2].GetComponent<GUITexture>().texture = normal;
					textAffiches[menuPersos.Count-3].GetComponent<GUIText>().color=Color.white;
					textAffiches[menuPersos.Count-2].GetComponent<GUIText>().color=Color.white;
					if(position>=menuPersos.Count-3)
					{
						textureAffichees [position].GetComponent<GUITexture>().texture = hover;
						textAffiches[position].GetComponent<GUIText>().color=Color.black;
						if(cadre1)
						{
							cadre1.GetComponent<GUITexture>().enabled = false;
							foreach( Transform child in cadre1.transform)
							{	
								child.GetComponent<GUITexture>().enabled = false;
							}
						}
					}
					else if (cadre1)
					{
						cadre1.transform.position=textureAffichees[position].transform.position+new Vector3(0,0,2f);
						cadre1.GetComponent<GUITexture>().enabled = true;
						foreach( Transform child in cadre1.transform)
						{	
							child.GetComponent<GUITexture>().enabled = true;
						}
					}
				}

			}
			
			if((menuCourant[0]==tr("Reglages Controles")) && (menuCourant[position+1]!=tr("Reglages Controles")) && (menuCourant[position+1]!=tr("JOUEUR :")) && (menuCourant[position+1]!=tr("RETOUR")))
			{
				/*
				foreach(string a in ControllerAPI.buttonProfiles[Game.playersMapping[positionH]].Keys)
				{
					listKeys.Add(ControllerAPI.buttonProfiles[Game.playersMapping[positionH]][a]);
				}*/
				if(booleans["right_down"])
				{
					AudioManager.Instance.Play("validateMenu");
					authorizeNavigate=false;
					//Destroy(flechesD[position-1]);
					flechesD[position-1].SetActive(false);
					controlAffiches[position-1].GetComponent<GUIText>().text="?";
					controlAffiches[position-1].GetComponent<GUIText>().color=Color.blue;
					waitingForKey = true;
					
					int controllerNumber = positionH - 1;
					string name = ControllerResources.ActionNames[position-1];
					
					StartCoroutine(SetKey(controllerNumber, name));
					return;
				}
			}

			if (booleans["down"])
			{
				AudioManager.Instance.Play("downMenu");
				StartCoroutine(RestrictMovement());
			}
			if(menuCourant[0]==menuPersos[0] && position<menuPersos.Count-3)
			{
				if (booleans["down"]) position=menuPersos.Count-3;
			}
			else if(menuCourant[0]==menuConfig[0] && position<menuConfig.Count-4)
			{
				if (booleans["down"]) position=menuConfig.Count-4;
			}
			else
			{
				if (booleans["down"] && position<menuCourant.Count-2) position++;
				else if (booleans["down"] && !(position<menuCourant.Count-2)) position = 0;
			}

			if (booleans["up"] && readyToMove)
			{
				AudioManager.Instance.Play("downMenu");
				StartCoroutine(RestrictMovement());
			}
			if(menuCourant[0]==menuPersos[0] && position<menuPersos.Count-3)
			{
				if(booleans["up"]) position=menuPersos.Count-2;
			}
			else if(menuCourant[0]==menuConfig[0] && position<menuConfig.Count-4)
			{
				if(booleans["up"]) position=menuConfig.Count-3;
			}
			else
			{
				if (booleans["up"] && position>0) position--;
				else if(booleans["up"] && !(position>0)) position = menuCourant.Count-2;
			}
			if(booleans["ok"])
			{
				if(menuCourant[position+1]==tr("RETOUR"))
					AudioManager.Instance.Play("cancelMenu");
				else
					AudioManager.Instance.Play("validateMenu");
				action(menuCourant,position);
			}
			if(booleans["right"] && menuCourant[position+1]==tr("VOLUME :") && AudioListener.volume<=0.692) AudioListener.volume+=0.008f;
			   if(booleans["left"] && menuCourant[position+1]==tr("VOLUME :") && AudioListener.volume>=0.008f) AudioListener.volume-=0.008f;;
			if(menuCourant[position+1]==tr("VOLUME :"))
			{
				Destroy(triangleVolume);
				Vector3 pos =new Vector3(0.5f,0.5f+(((float)heightLabel/2)/(float)Screen.height)*(menuCourant.Count/2-1),-1);
				triangleVolume = (GameObject)Instantiate (Resources.Load ("menuVolumeTriangle"),new Vector3(pos.x,pos.y,3),Quaternion.identity);
				Rect t = new Rect(triangleVolume.GetComponent<GUITexture>().pixelInset.x,triangleVolume.GetComponent<GUITexture>().pixelInset.y,250*AudioListener.volume*1.42f,25*AudioListener.volume*1.42f);
				triangleVolume.GetComponent<GUITexture>().pixelInset=t;
				if(AudioListener.volume<0.4f) triangleVolume.GetComponent<GUITexture>().texture=triVolume1;
				else if (AudioListener.volume>=0.4f && AudioListener.volume<0.6f) triangleVolume.GetComponent<GUITexture>().texture=triVolume2;
				else triangleVolume.GetComponent<GUITexture>().texture=triVolume3;
			}
			else if(menuCourant[position+1]==tr("JOUEUR :"))
			{
				textPlayer.GetComponent<GUIText>().text="Joueur "+positionH;
				for(int i=0;i<controlAffiches.Count;i++)
				{
					string name = ControllerResources.ActionNames[i];
					string action = ControllerManager.Instance.GetController(positionH - 1).GetNameKey(name);
					controlAffiches[i].GetComponent<GUIText>().text = action;
				}
				if(booleans["right_down"])
				{
					AudioManager.Instance.Play("downMenu");
					if(positionH<main.players.Count) positionH++;
					else positionH=1;
				}
				else if(booleans["left_down"])
				{
					AudioManager.Instance.Play("downMenu");
					if(positionH>1) positionH--;
					else positionH=main.players.Count;
				}
				
			}
			else if(menuCourant[0] == menuPersos[0] && position<menuPersos.Count - 3)
			{
				if(booleans["right_down"])
				{
					AudioManager.Instance.Play("downMenu");
					if(position==menuPersos.Count-4) position=0;
					else position++;
					StartCoroutine(RestrictMovement());
				}
				else if(booleans["left_down"])
				{
					AudioManager.Instance.Play("downMenu");
					if(position==0) position=menuPersos.Count-4;
					else position--;
					StartCoroutine(RestrictMovement());
				}
				else if(booleans["ok"])
				{
					if(falseok==false)
					{
						persos.Add(menuCourant[position+1]);
						Destroy(cadre1);
						Color c;
						switch (numSelection)
						{
							case 1:
							c=new Color(2f/255f,41f/255f,237f/255f);
								break;
							case 2:
							c=new Color(238f/255f,19f/255f,2f/255f);
								break;
							case 3:
							c=new Color(238f/255f,230f/255f,2f/255f);
								break;
							case 4:
							c=new Color(32f/255f,214f/255f,2f/255f);
								break;
							default:
								c=new Color();
								break;
						}
						GameObject textbutton =(GameObject)Instantiate (Resources.Load ("textButton"),new Vector3(0.85f,0.66f-0.07f*numSelection,0),Quaternion.identity);
						textbutton.GetComponent<GUIText>().text="J"+numSelection+" : "+menuCourant[position+1];
						textbutton.GetComponent<GUIText>().color=c;
						textbutton.GetComponent<GUIText>().fontSize=40;
						textAffiches.Add(textbutton);
                        textbutton.transform.parent = m_menuParent;
                        if (numSelection<4)
						{
							numSelection++;
							cadre1 = (GameObject)GameObject.Instantiate (Resources.Load ("cadre"+numSelection), textureAffichees [position].transform.position+new Vector3(0,0,2f), Quaternion.identity);
						}
						else
							position=menuCourant.Count-3;
					}
					falseok=false;
				}
			}
			else if (menuCourant[0]==menuMaps[0])
			{
				if(booleans["right_down"])
				{
					AudioManager.Instance.Play("downMenu");

					if(positionH==Game.listMapForMenu.Count-1) positionH=0;
					else positionH++;
					nameMap.GetComponent<GUIText>().text=Game.listMapForMenu[positionH];
				}
				if(booleans["left_down"])
				{
					AudioManager.Instance.Play("downMenu");
					if(positionH==0) positionH=Game.listMapForMenu.Count-1;
					else positionH--;
					nameMap.GetComponent<GUIText>().text=Game.listMapForMenu[positionH];
				}
			}
			else if (menuCourant[0]==menuConfig[0] )//&& configWeaponsStates.Count==8)
			{
				if(position<menuConfig.Count-4)
				{
					if(booleans["right_down"])
					{
						AudioManager.Instance.Play("downMenu");
						if(position==menuConfig.Count-5) position=0;
						else position++;
					}
					else if(booleans["left_down"])
					{
						AudioManager.Instance.Play("downMenu");
						if(position==0) position=menuConfig.Count-5;
						else position--;
					}
				}
				if(position==menuConfig.Count-4)
				{
					if(booleans["right_down"])
					{
						AudioManager.Instance.Play("downMenu");
						if(nbPts==99) nbPts=1;
						else nbPts++;
					}
					else if(booleans["left_down"])
					{
						AudioManager.Instance.Play("downMenu");
						if(nbPts==1) nbPts=99;
						else nbPts--;
					}
				}
				if(booleans["ok"] && !falseok && position<menuConfig.Count-4)// && configWeaponsStates.Count==menuConfig.Count-3)
				{
					configWeaponsStates[position]=!configWeaponsStates[position];
				}
				else if(booleans["ok"])
					falseok=false;
				if(position<menuConfig.Count-4)
				{
					for(int rg=0;rg<configWeaponsStates.Count;rg++)
					{
						if(configWeaponsStates[position])// && !falseok)
						{
							tempAffiches[position].GetComponent<GUITexture>().color=new Color(241f/255f,255f/255f,0f/255f);
						}
						else
						{
							tempAffiches[position].GetComponent<GUITexture>().color=new Color(255f/255f,0f/255f,0f/255f);
						}
					}
				}
			}
		}
	}

	IEnumerator WaitIgnoringTimeScale(float duration)
	{
		float timeStart = Time.realtimeSinceStartup;
		while(Time.realtimeSinceStartup - timeStart <duration)
		{
			yield return new WaitForEndOfFrame ();
		}
	}
	
	IEnumerator SetKey(int i, string action)
	{
		yield return StartCoroutine(WaitIgnoringTimeScale(0.25f));

		ControllerManager.Instance.GetController(i).ListenNewKey(action);
	}
	
	IEnumerator GetKey()
	{
		AudioManager.Instance.Play("validateMenu");
		yield return StartCoroutine(WaitIgnoringTimeScale(0.1f));
		waitingForKey = false;
		flechesD[position - 1].SetActive(true);
		authorizeNavigate = true;
		string name = ControllerResources.ActionNames[position - 1];
		string action = ControllerManager.Instance.GetController(positionH - 1).GetNameKey(name);
		controlAffiches[position - 1].GetComponent<GUIText>().text=action;
	}

	void CheckKeys()
	{

		if (readyToMove){
			booleans["up"] = ControllerManager.Instance.GetKey("up");
			booleans["down"] = ControllerManager.Instance.GetKey("down");
			booleans["right"] = ControllerManager.Instance.GetKey("right");
			booleans["left"] = ControllerManager.Instance.GetKey("left");

		}
		//Debug.Log(readyToMove +","+ booleans["up"]);
		
		booleans["ok"] = ControllerManager.Instance.GetKeyDown("validate");
		booleans["back"] = ControllerManager.Instance.GetKeyDown("action");
		booleans["start"] = ControllerManager.Instance.GetKeyDown("start");
		
		booleans["up_down"] = ControllerManager.Instance.GetKeyDown("up");
		booleans["down_down"] = ControllerManager.Instance.GetKeyDown("down");
		booleans["right_down"] = ControllerManager.Instance.GetKeyDown("right");
		booleans["left_down"] = ControllerManager.Instance.GetKeyDown("left");
	}
	
	IEnumerator RestrictMovement()
	{
		lockMove = true;
		yield return StartCoroutine(WaitIgnoringTimeScale(0.2f));
		lockMove = false;
	}

	IEnumerator changeLevel(string level)
	{
		yield return StartCoroutine(WaitIgnoringTimeScale(0.2f));
		if(level=="loaded")
		{
			Application.LoadLevel (Application.loadedLevel);
		}
		else
		{
			Application.LoadLevel (level);
		}
	}
	
	void action(List <string> menu,int p)
	{
		if(menu[0]==tr("Start"))
		{
			if(menu[p+1].Equals( tr("REPRENDRE") ))
			{
				Pause();
			}
			else if (menu[p+1].Equals( tr("RECOMMENCER")))
			{
					StartCoroutine(changeLevel("loaded"));
					Restart();
			}
			else if (menu[p+1].Equals( tr("OPTIONS")))
			{
					displayMenu(menuOptions);
			}
			else if (menu[p+1].Equals( tr("CHANGER CONFIG")))
			{
				Restart();
				Menus.menuToGo=menuConfig;
				StartCoroutine(changeLevel("mainmenu"));
			}
			else if (menu[p+1].Equals( tr("CHANGER NIVEAU")))
			{
					if (Application.loadedLevelName == "plage")
						//Application.LoadLevel("dinoRace");
						StartCoroutine(changeLevel("parking"));
					else
						StartCoroutine(changeLevel("plage"));
					//Application.LoadLevel("plage");
					Restart();
			}
			else if (menu[p+1].Equals(tr("QUITTER")))
			{
					Application.Quit();
			}
		}
		if(menu[0]==tr("Fin"))
		{
			if(menu[p+1].Equals( tr("RECOMMENCER")))
			{
					StartCoroutine(changeLevel("loaded"));
					Restart();
			}
			else if (menu[p+1].Equals( tr("CHANGER NIVEAU")))
			{
					Restart();
					Menus.menuToGo=menuMaps;
					StartCoroutine(changeLevel("mainmenu"));
			}
			else if (menu[p+1].Equals( tr("CHANGER PERSONNAGE")))
			{
					Restart();
					Menus.menuToGo=menuPersos;
					StartCoroutine(changeLevel("mainmenu"));
			}
			else if (menu[p+1].Equals( tr("CHANGER CONFIG")))
			{
					Restart();
					Menus.menuToGo=menuConfig;
					StartCoroutine(changeLevel("mainmenu"));
			}
			else if (menu[p+1].Equals( tr("QUITTER")))
			{
					Application.Quit();
			}
		}
		if(menu[0]==tr("Options"))
		{
			if(menu[p+1].Equals( tr("REGLAGES CONTROLES")))
			{
					displayMenu(menuReglages);
			}
			else if (menu[p+1].Equals(  tr("RETOUR")))
			{
					displayMenu(menuPause);
			}
		}
		if(menu[0]==tr("Reglages Controles"))
		{
			if(menu[p+1].Equals( tr("RETOUR") ))
			{
				displayMenu(menuOptions);
			}
		}
		if(menu[0]==mainMenu[0])
		{
			if(menu[p+1].Equals( tr("QUITTER")))
			{
				Application.Quit();
			}
			else if (menu[p+1].Equals( tr("CREDITS")))
			{
				displayMenu(menuCredits);
			}
			else if (menu[p+1].Equals(tr("BATAILLE")))
			{
				displayMenu(menuPersos);
				cadre1.GetComponent<GUITexture>().enabled=true;
				cadre5.GetComponent<GUITexture>().enabled = true;
			}
		}
		if(menu[0]==menuCredits[0])
		{
			if(menu[p+1].Equals( tr("RETOUR") ))
			{
				displayMenu(mainMenu);
			}
		}
		if(menu[0]==menuMaps[0])
		{
			if(menu[p+1].Equals(tr("RETOUR")))
			{
				displayMenu(menuPersos);
				persos=new List<string>();
				numSelection=0;
				falseok=false;
				cadre5.GetComponent<GUITexture>().enabled = true;
			}
			else if (menu[p+1].Equals(tr("VALIDER")))
			{
				falseok=true;
				map=Game.listMapForMenu[positionH];
				if(map!=null)
					displayMenu(menuConfig);
				cadre1 = (GameObject)GameObject.Instantiate (Resources.Load ("cadre6"), textureAffichees [position].transform.position+new Vector3(0,0,2f), Quaternion.identity);
				cadre1.GetComponent<GUITexture>().enabled = true;
				weapons=new List<string>();
			}
		}
		if(menu[0]==menuPersos[0])
		{
			if(menu[p+1].Equals(tr("RETOUR")))
			{
				cadre5.GetComponent<GUITexture>().enabled = false;
				for(int n=0;n<persos.Count;n++)
					persos.RemoveAt(n);
				displayMenu(mainMenu);
				numSelection=1;
				cadre1 = (GameObject)GameObject.Instantiate (Resources.Load ("cadre1"), textureAffichees [position].transform.position+new Vector3(0,0,2f), Quaternion.identity);
				cadre1.GetComponent<GUITexture>().enabled=false;
				foreach( Transform child in cadre1.transform)
				{	
					child.GetComponent<GUITexture>().enabled = false;
				}
				falseok=true;
				persos=new List<string>();
			}
			else if(menu[p+1].Equals( tr("VALIDER")))
			{
				if(persos.Count>1)
				{
					cadre5.GetComponent<GUITexture>().enabled = false;
					displayMenu(menuMaps);
					positionH=0;
				}
			}
		}
		if(menu[0]==menuConfig[0])
		{
			if(menu[p+1].Equals( tr("RETOUR") ))
			{
				cadre1.GetComponent<GUITexture>().enabled = false;
				displayMenu(menuMaps);
				j = 5;
				configWeaponsStates=new List<bool>();
			}
			else if(menu[p+1].Equals( tr("VALIDER") ))
			{
				transformBoolToString();
				for(int i=0;i< weapons.Count;i++)
				{
					foreach(string key in Game.translateNameWeapons.Keys)
					{
						if (tr (key) == weapons[i])
						{
							weapons[i] = Game.translateNameWeapons[key];
							break;
						}
					}
					//weapons[i]=Game.translateNameWeapons[weapons[i]];
				}
				if (weapons.Count < 1)
				{
					Game.gameWeapons =  new List <string> {
						"greenBeaker","greenShield","bomb","triple_bomb","triple_missile","Aku-Aku","TNT","turbo"};
				}
				else
				{
					Game.gameWeapons=weapons;
				}
				/*for(int i =0;i< persos.Count;i++)
				{
					Game.listKarts[i]=persos[i];
				}*/
				Game.listKarts=persos;
				Game.nbPoints=nbPts;
				StartCoroutine(changeLevel(Game.translateNameMaps[map]));
			}
		}
	}

	void transformBoolToString()
	{
		Debug.Log (configWeaponsStates.Count);
		for(int i=0;i<configWeaponsStates.Count;i++) // /!\ aucun booléan dans la liste après un "changer config"
		{
			Debug.Log("ajout1");
			if(configWeaponsStates[i])
			{
				Debug.Log("ajout2");
				weapons.Add(menuConfig[i+1]);
			}
		}
	}

	void Restart()
	{
		viderMenu ();
		Destroy (greyT);
		Time.timeScale = normalTime;
		Main.Restart();
	}
	
	void viderMenu()
	{
		if(cadre1)
		{
			cadre1.GetComponent<GUITexture>().enabled = false;
			foreach( Transform child in cadre1.transform)
			{	
				child.GetComponent<GUITexture>().enabled = false;
			}
		}
		authorizeNavigate = false;
		Destroy (titreAffiche);
		Destroy (triangleFond);
		Destroy (triangleVolume);
		Destroy (fleches);
		Destroy (textPlayer);
		Destroy (nameMap);
		foreach (GameObject g in textureAffichees) Destroy (g);
		foreach (GameObject g in tempAffiches) Destroy (g);
		foreach (GameObject g in textAffiches) Destroy (g);
		foreach (GameObject g in controlAffiches) Destroy (g);
		foreach (GameObject g in flechesD) Destroy (g);
		textureAffichees =  new List <GameObject>();
		textAffiches =  new List <GameObject>();
		tempAffiches =  new List <GameObject>();
		controlAffiches =  new List <GameObject>();
		flechesD =  new List <GameObject>();
		if(cameraMenu)
			cameraMenu.SetActive (false);
	}
	
}
