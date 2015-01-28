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
	private static GameObject triangleFond;
	private static GameObject triangleVolume;
	private static GameObject textPlayer;
	private static GameObject fleches;
	private static List <KeyCode> listKeys=  new List <KeyCode>();
	private static List <GameObject> flechesD =  new List <GameObject>();
	private static List <GameObject> textureAffichees =  new List <GameObject>();
	private static List <GameObject> textAffiches =  new List <GameObject>();
	private static List <GameObject> controlAffiches =  new List <GameObject>();
	private static Dictionary <int, string> menuCourant = new Dictionary<int, string>();
	public Main main;
	public Kart winner;
	public List <Kart> loosers=  new List <Kart>();
	
	
	private bool readyToMove = true;
	
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
		{1,"VOLUME :"},
		{2,"REGLAGES CONTROLES"},
		{3,"RETOUR"}
	};
	private static Dictionary <int, string> menuReglages =  new Dictionary<int, string>
	{
		{0,"Reglages Controles"},
		{1,"JOUEUR :"},
		{2,"Avancer"},
		{3,"Reculer"},
		{4,"Tourner Gauche"},
		{5,"Tourner Droite"},
		{6,"Actionner Arme"},
		{7,"Sauter"},
		{8,"Inverser Camera"},
		{9,"Changer Vue"},
		{10,"Mettre en Pause"},
		{11,"RETOUR"}
	};
	private static Dictionary <int, string> menuFin =  new Dictionary<int, string>
	{
		{0,"Fin"},
		{1,"RECOMMENCER"},
		{2,"STATISTIQUES"},
		{3,"CHANGER PERSONNAGE"},
		{4,"CHANGER NIVEAU"},
		{5,"CHANGER CONFIG"},
		{6,"QUITTER"}
	};
	// Use this for initialization
	void Start ()
	{
		normalTime = Time.timeScale;
		cameraMenu = (GameObject)GameObject.Instantiate (Resources.Load("cameraMenu"));
		//AudioListener.volume = 0.5f;
		//soundUp =(AudioClip)Instantiate (Resources.Load ("Audio/down_menu"));
	}
	
	
	// Update is called once per frame
	void Update ()
	{
		if (!waitingForKey)
			navigateMenu ();
		else
			CheckNewKey();
		testPause ();
		testEnd ();
	}
	
	void testPause()
	{
		bool pressStart = false;
		for (int i = 1; i<Kart.totalPlayers+1; i++) pressStart |=  ControllerAPI.StaticGetKeyDown(i, "start");
		if (pressStart)
		{
			main.gameObject.audio.PlayOneShot (main.soundOk);
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
			}
			if(winner!=null)
			{
				k.c2d.camera.enabled=false;
				if(k != winner)
				{
					k.camera.camera.rect=new Rect ();
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
		j1.guiText.text = "Joueur "+winner.numeroJoueur+" : "+winner.nbPoints+" Pts";
		j1.guiText.color = Color.yellow;
		loosers = loosers.OrderBy(x => x.nbPoints).ToList();
		loosers.Reverse ();
		foreach(Kart k in loosers)
		{
			GameObject j =(GameObject)Instantiate (Resources.Load ("textTitreMenu"),new Vector3(0.27f,0.35f-0.08f*(loosers.IndexOf(k)+1),0),Quaternion.identity);
			j.guiText.text = "Joueur "+k.numeroJoueur+" : "+k.nbPoints+" Pts";
		}
		KartController.stop = false;
		winner.kc.gameObject.transform.position = main.listRespawn [0].position;
		winner.kc.gameObject.transform.rotation =main.listRespawn[0].rotation;
		AI.kart = winner.kc.gameObject;
		//AI.children = winner.kc.wheels ["wheelAL"];
		AI.wheels = winner.kc.wheels;
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
			winner.camera.camera.rect = Lerp(winner.camera.camera.rect,obj, percent); 
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
			if(Main.sourceMusic)
				Main.sourceMusic.enabled=false;
			//AudioListener.pause = true;
		}
		else if (inPause)
		{
			inPause=false;
			viderMenu ();
			Destroy(greyT);
			Time.timeScale=normalTime;
			if(Main.sourceMusic)
				Main.sourceMusic.enabled=true;
			//AudioListener.pause = false;
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
			if(!specialButton(menu,i))
			{
				Vector3 pos =new Vector3(0.5f,0.5f+(((float)heightLabel/2)/(float)Screen.height)*(menu.Count/2-i),-1);
				textureAffichees.Add((GameObject)Instantiate (Resources.Load ("menuButton"),pos,Quaternion.identity));
				GameObject textbutton =(GameObject)Instantiate (Resources.Load ("textButton"),new Vector3(pos.x,pos.y,0),Quaternion.identity);
				textbutton.guiText.text=menu[i];
				textAffiches.Add(textbutton);
			}
		}
		menuCourant = menu;
		authorizeNavigate = true;
	}
	
	bool specialButton(Dictionary <int, string> menu, int i)
	{
		if(menu[i]=="VOLUME :")
		{
			Vector3 pos =new Vector3(0.5f,0.5f+(((float)heightLabel/2)/(float)Screen.height)*(menu.Count/2-i),-1);
			textureAffichees.Add((GameObject)Instantiate (Resources.Load ("menuButton"),pos,Quaternion.identity));
			triangleFond = (GameObject)Instantiate (Resources.Load ("menuBackgroundTriangle"),new Vector3(pos.x,pos.y,2),Quaternion.identity);
			triangleVolume = (GameObject)Instantiate (Resources.Load ("menuVolumeTriangle"),new Vector3(pos.x,pos.y,3),Quaternion.identity);
			Rect t = new Rect(triangleVolume.guiTexture.pixelInset.x,triangleVolume.guiTexture.pixelInset.y,250*AudioListener.volume*1.42f,25*AudioListener.volume*1.42f);
			triangleVolume.guiTexture.pixelInset=t;
			GameObject textbutton =(GameObject)Instantiate (Resources.Load ("textButton"),new Vector3(pos.x-(float)((float)400/(float)((float)Screen.width*(float)3)),pos.y,0),Quaternion.identity);
			textbutton.guiText.text=menu[i];
			textAffiches.Add(textbutton);
			return true;
		}
		else if(menu[0]=="Reglages Controles" && menu[i]!="RETOUR")
		{
			if(menu[i]=="JOUEUR :")
			{
				Vector3 pos =new Vector3(0.5f,0.5f+(((float)heightLabel/2)/(float)Screen.height)*(menu.Count/2-i),-1);
				textureAffichees.Add((GameObject)Instantiate (Resources.Load ("menuButton"),pos,Quaternion.identity));
				fleches = (GameObject)Instantiate (Resources.Load ("menuFleches"),new Vector3(pos.x+(float)((float)400/(float)((float)Screen.width*(float)5)),pos.y,5),Quaternion.identity);
				GameObject textbutton =(GameObject)Instantiate (Resources.Load ("textButton"),new Vector3(pos.x-(float)((float)400/(float)((float)Screen.width*(float)4)),pos.y,0),Quaternion.identity);
				textbutton.guiText.text=menu[i];
				textAffiches.Add(textbutton);
				textPlayer = (GameObject)Instantiate (Resources.Load ("textButton"),new Vector3(pos.x+(float)((float)400/(float)((float)Screen.width*(float)5)),pos.y,6),Quaternion.identity);
				positionH=main.players[0].numeroJoueur;
				textPlayer.guiText.text="Joueur "+positionH;
			}
			else if(i>1)
			{
				Vector3 pos =new Vector3(0.5f,0.5f+(((float)heightLabel/2)/(float)Screen.height)*(menu.Count/2-i),-1);
				textureAffichees.Add((GameObject)Instantiate (Resources.Load ("menuButton"),pos,Quaternion.identity));
				GameObject textbutton =(GameObject)Instantiate (Resources.Load ("textButton"),new Vector3(pos.x-(float)((float)400/(float)((float)Screen.width*(float)2.2f)),pos.y,0),Quaternion.identity);
				textbutton.guiText.text=menu[i];
				textbutton.guiText.anchor=TextAnchor.MiddleLeft;
				textAffiches.Add(textbutton);
				GameObject textcontrol =(GameObject)Instantiate (Resources.Load ("textControl"),new Vector3(pos.x+(float)((float)400/(float)((float)Screen.width*(float)5)),pos.y,0),Quaternion.identity);
				string action = ControllerAPI.GetActionName(menu[i]);
				string name  = ControllerAPI.KeyIs(positionH, action);
				textcontrol.guiText.text=name;
				
				controlAffiches.Add(textcontrol);
				GameObject flecheD = (GameObject)Instantiate (Resources.Load ("menuFlecheD"),new Vector3(pos.x+(float)((float)400/(float)((float)Screen.width*(float)2.5f)),pos.y,5),Quaternion.identity);
				flechesD.Add(flecheD);
			}
			return true;
		}
		else if(menu[0]=="Fin")
		{
			if(titreAffiche.transform.position.x != 0.8f)
				titreAffiche.transform.position = new Vector3(0.8f,0.5f+(((float)heightLabel/2)/(float)Screen.height)*7/2f,0);
			Vector3 pos =new Vector3(0.8f,0.5f+(((float)heightLabel/2)/(float)Screen.height)*(menu.Count/2-i),-1);
			textureAffichees.Add((GameObject)Instantiate (Resources.Load ("menuButton"),pos,Quaternion.identity));
			GameObject textbutton =(GameObject)Instantiate (Resources.Load ("textButton"),new Vector3(pos.x,pos.y,0),Quaternion.identity);
			textbutton.guiText.text=menu[i];
			textAffiches.Add(textbutton);
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
				for(int i=0; i<textAffiches.Count;i++)
				{
					textureAffichees [i].guiTexture.texture = normal;
					textureAffichees [position].guiTexture.texture = hover;
					textAffiches[i].guiText.color=Color.white;
					textAffiches[position].guiText.color=Color.black;
					if(menuCourant[0]=="Reglages Controles")
					{
						textPlayer.guiText.color=Color.white; 
						if(menuCourant[position+1]=="JOUEUR :")
						{
							textPlayer.guiText.color=Color.black;
						}
						if(i<controlAffiches.Count) controlAffiches[i].guiText.color=Color.white;
						if(position>0 && position<menuCourant.Count-2)
						{
							controlAffiches[position-1].guiText.color=Color.black;
						}
					}
				}
			}
			bool down = false;
			for (int i = 1; i<Kart.totalPlayers+1; i++)
			{
				down |=  ControllerAPI.StaticIsPressed(i, "moveBack");
			}
			if (!readyToMove)
				down = false;
			else if (down && readyToMove)
			{
				main.gameObject.audio.PlayOneShot (main.soundUp);
				StartCoroutine(RestrictMovement());
			}
			if (down && position<menuCourant.Count-2) position++;
			else if(down && !(position<menuCourant.Count-2)) position = 0;
			bool up = false;
			for (int i = 1; i<Kart.totalPlayers+1; i++)
				up |=  ControllerAPI.StaticIsPressed(i, "throw");
			if (!readyToMove)
				up = false;
			else if (up && readyToMove)
			{
				main.gameObject.audio.PlayOneShot (main.soundUp);
				StartCoroutine(RestrictMovement());
			}
			if (up && position>0) position--;
			else if(up && !(position>0)) position = menuCourant.Count-2;
			bool ok = false;
			for (int i = 1; i<Kart.totalPlayers+1; i++)
			{
				ok |=  ControllerAPI.StaticGetKeyDown(i, "moveForward");
			}
			if(ok)
			{
				if(menuCourant[position+1]=="RETOUR")
					main.gameObject.audio.PlayOneShot (main.soundCancel);
				else
					main.gameObject.audio.PlayOneShot (main.soundOk);
				action(menuCourant,position);
			}
			
			bool right = false;
			bool left = false;
			for (int i = 1; i<Kart.totalPlayers+1; i++)
			{
				right |=  ControllerAPI.StaticIsPressed(i, "turnRight");
				left |=  ControllerAPI.StaticIsPressed(i, "turnLeft");
			}
			bool rightDown = false;
			bool leftDown = false;
			for (int i = 1; i<Kart.totalPlayers+1; i++)
			{
				rightDown |=  ControllerAPI.StaticGetKeyDown(i, "turnRight");
				leftDown |=  ControllerAPI.StaticGetKeyDown(i, "turnLeft");
			}
			if(right && menuCourant[position+1]=="VOLUME :" && AudioListener.volume<=0.692) AudioListener.volume+=0.008f;
			if(left && menuCourant[position+1]=="VOLUME :" && AudioListener.volume>=0.008f) AudioListener.volume-=0.008f;;
			if(menuCourant[position+1]=="VOLUME :")
			{
				Destroy(triangleVolume);
				Vector3 pos =new Vector3(0.5f,0.5f+(((float)heightLabel/2)/(float)Screen.height)*(menuCourant.Count/2-1),-1);
				triangleVolume = (GameObject)Instantiate (Resources.Load ("menuVolumeTriangle"),new Vector3(pos.x,pos.y,3),Quaternion.identity);
				Rect t = new Rect(triangleVolume.guiTexture.pixelInset.x,triangleVolume.guiTexture.pixelInset.y,250*AudioListener.volume*1.42f,25*AudioListener.volume*1.42f);
				triangleVolume.guiTexture.pixelInset=t;
				if(AudioListener.volume<0.4f) triangleVolume.guiTexture.texture=triVolume1;
				else if (AudioListener.volume>=0.4f && AudioListener.volume<0.6f) triangleVolume.guiTexture.texture=triVolume2;
				else triangleVolume.guiTexture.texture=triVolume3;
			}
			else if(menuCourant[position+1]=="JOUEUR :")
			{
				textPlayer.guiText.text="Joueur "+positionH;
				for(int i=0;i<controlAffiches.Count;i++)
				{
					string action =ControllerAPI.GetActionName(menuCourant[i+2]);
					string name  = ControllerAPI.KeyIs(positionH, action);
					controlAffiches[i].guiText.text=name;
				}
				if(rightDown)
				{
					if(positionH<main.players.Count) positionH++;
					else positionH=1;
				}
				else if(leftDown)
				{
					if(positionH>1) positionH--;
					else positionH=main.players.Count;
				}
				
			}
			else if((menuCourant[0]=="Reglages Controles") && (menuCourant[position+1]!="Reglages Controles") && (menuCourant[position+1]!="JOUEUR :") && (menuCourant[position+1]!="RETOUR"))
			{
				listKeys=  new List <KeyCode>();
				foreach(string a in ControllerAPI.buttonProfiles[Game.playersMapping[positionH]].Keys)
				{
					listKeys.Add(ControllerAPI.buttonProfiles[Game.playersMapping[positionH]][a]);
				}
				if(rightDown)
				{
					authorizeNavigate=false;
					//Destroy(flechesD[position-1]);
					flechesD[position-1].SetActive(false);
					controlAffiches[position-1].guiText.text="?";
					controlAffiches[position-1].guiText.color=Color.blue;
					waitingForKey = true;
					string action =ControllerAPI.GetActionName(menuCourant[position+1]);
					string name  = ControllerAPI.KeyIs(positionH, action);

					StartCoroutine(setKey(action, name));
				}
			}
		}
	}
	
	IEnumerator setKey(string action, string name)
	{
		for(int i=0;i<10;++i)
		{
			yield return new WaitForEndOfFrame ();
		}
		ControllerAPI.ListenForKey(action, name);
	}
	
	IEnumerator getKey()
	{
		for(int i=0;i<10;++i)
		{
			yield return new WaitForEndOfFrame ();
		}
		waitingForKey = false;
		flechesD[position-1].SetActive(true);
		authorizeNavigate=true;
		string action = ControllerAPI.GetActionName(menuCourant[position+1]);
		string name  = ControllerAPI.KeyIs(positionH, action);
		controlAffiches[position-1].guiText.text=name;
	}
	
	void CheckNewKey()
	{
		if (ControllerAPI.CheckForAxis() || ControllerAPI.CheckForKey())
			StartCoroutine(getKey());
	}
	
	IEnumerator RestrictMovement()
	{
		readyToMove = false;
		for(int i=0;i<10;i++)
			yield return new WaitForEndOfFrame ();
		readyToMove = true;
	}
	
	IEnumerator changeLevel(string level)
	{
		for(int i=0; i<20;i++)
			yield return new WaitForEndOfFrame ();
		if(level=="loaded")
			Application.LoadLevel (Application.loadedLevel);
		else
			Application.LoadLevel (level);
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
				StartCoroutine(changeLevel("loaded"));
				Restart();
				break;
			case "OPTIONS":
				displayMenu(menuOptions);
				break;
			case "CHANGER CONFIG":
				break;
			case "CHANGER NIVEAU":
				if (Application.loadedLevelName == "plage")
					//Application.LoadLevel("dinoRace");
					StartCoroutine(changeLevel("dinoRace"));
				else
					StartCoroutine(changeLevel("plage"));
				//Application.LoadLevel("plage");
				Restart();
				break;
			case "QUITTER":
				Application.Quit();
				break;
			default:
				break;
			}
		}
		if(menu[0]=="Fin")
		{
			switch (menu[p+1])
			{
			case "RECOMMENCER":
				StartCoroutine(changeLevel("loaded"));
				Restart();
				break;
			case "CHANGER NIVEAU":
				if (Application.loadedLevelName == "plage")
					StartCoroutine(changeLevel("dinoRace"));
				else
					StartCoroutine(changeLevel("plage"));
				Restart();
				break;
			case "QUITTER":
				Application.Quit();
				break;
			default:
				break;
			}
		}
		if(menu[0]=="Options")
		{
			switch (menu[p+1])
			{
			case "REGLAGES CONTROLES":
				displayMenu(menuReglages);
				break;
			case "RETOUR":
				displayMenu(menuPause);
				break;
			default:
				break;
			}
		}
		if(menu[0]=="Reglages Controles")
		{
			switch (menu[p+1])
			{
			case "RETOUR":
				displayMenu(menuOptions);
				break;
			default:
				break;
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
		authorizeNavigate = false;
		Destroy (titreAffiche);
		Destroy (triangleFond);
		Destroy (triangleVolume);
		Destroy (fleches);
		Destroy (textPlayer);
		foreach (GameObject g in textureAffichees) Destroy (g);
		foreach (GameObject g in textAffiches) Destroy (g);
		foreach (GameObject g in controlAffiches) Destroy (g);
		foreach (GameObject g in flechesD) Destroy (g);
		textureAffichees =  new List <GameObject>();
		textAffiches =  new List <GameObject>();
		controlAffiches =  new List <GameObject>();
		flechesD =  new List <GameObject>();
		cameraMenu.SetActive (false);
	}
	
}
