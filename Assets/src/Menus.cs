using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	private bool authorizeNavigate = false;
	private bool inPause = false;
	private static GameObject titreAffiche;
	private static GameObject triangleFond;
	private static GameObject triangleVolume;
	private static GameObject fleches;
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
		{8,"Deraper"},
		{9,"Inverser Camera"},
		{10,"Mettre en Pause"},
		{11,"RETOUR"}
	};
	// Use this for initialization
	void Start ()
	{
		normalTime = Time.timeScale;
		cameraMenu = (GameObject)GameObject.Instantiate (Resources.Load("cameraMenu"));
		//AudioListener.volume = 0.5f;
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
			if(!specialButton(menu,i))
			{
				Vector3 pos =new Vector3(0.5f,0.5f+(((float)heightLabel/2)/(float)Screen.height)*(menu.Count/2-i),-1);
				textureAffichees.Add((GameObject)Instantiate (Resources.Load ("button"),pos,Quaternion.identity));
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
			textureAffichees.Add((GameObject)Instantiate (Resources.Load ("button"),pos,Quaternion.identity));
			triangleFond = (GameObject)Instantiate (Resources.Load ("triangleFond"),new Vector3(pos.x,pos.y,2),Quaternion.identity);
			triangleVolume = (GameObject)Instantiate (Resources.Load ("triangleVolume"),new Vector3(pos.x,pos.y,3),Quaternion.identity);
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
				textureAffichees.Add((GameObject)Instantiate (Resources.Load ("button"),pos,Quaternion.identity));
				fleches = (GameObject)Instantiate (Resources.Load ("fleches"),new Vector3(pos.x+(float)((float)400/(float)((float)Screen.width*(float)5)),pos.y,5),Quaternion.identity);
				GameObject textbutton =(GameObject)Instantiate (Resources.Load ("textButton"),new Vector3(pos.x-(float)((float)400/(float)((float)Screen.width*(float)4)),pos.y,0),Quaternion.identity);
				textbutton.guiText.text=menu[i];
				textAffiches.Add(textbutton);
			}
			else
			{
				Vector3 pos =new Vector3(0.5f,0.5f+(((float)heightLabel/2)/(float)Screen.height)*(menu.Count/2-i),-1);
				textureAffichees.Add((GameObject)Instantiate (Resources.Load ("button"),pos,Quaternion.identity));
				GameObject textbutton =(GameObject)Instantiate (Resources.Load ("textButton"),new Vector3(pos.x-(float)((float)400/(float)((float)Screen.width*(float)2.2f)),pos.y,0),Quaternion.identity);
				textbutton.guiText.text=menu[i];
				textbutton.guiText.anchor=TextAnchor.MiddleLeft;
				textAffiches.Add(textbutton);
			}
			return true;
		}
		else return false;
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
			else if(down && !(position<menuCourant.Count-2)) position = 0;
			bool up = false;
			for (int i = 1; i<5; i++)
			{
				up |= (Input.GetAxis (KartController.axisMapping[i]["stop"]) == -1 && KartController.controllersEnabled[i]);
				up |= (Input.GetKeyDown (KartController.playersMapping [i] ["moveForward"]) && !KartController.controllersEnabled[i]);
			}
			if (up && position>0) position--;
			else if(up && !(position>0)) position = menuCourant.Count-2;
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
			bool right = Input.GetKey(KeyCode.D);
			if(right && menuCourant[position+1]=="VOLUME :" && AudioListener.volume<=0.692) AudioListener.volume+=0.008f;
			bool left = Input.GetKey(KeyCode.Q);
			if(left && menuCourant[position+1]=="VOLUME :" && AudioListener.volume>=0.008f) AudioListener.volume-=0.008f;
			if(menuCourant[position+1]=="VOLUME :")
			{
				Destroy(triangleVolume);
				Vector3 pos =new Vector3(0.5f,0.5f+(((float)heightLabel/2)/(float)Screen.height)*(menuCourant.Count/2-1),-1);
				triangleVolume = (GameObject)Instantiate (Resources.Load ("triangleVolume"),new Vector3(pos.x,pos.y,3),Quaternion.identity);
				Rect t = new Rect(triangleVolume.guiTexture.pixelInset.x,triangleVolume.guiTexture.pixelInset.y,250*AudioListener.volume*1.42f,25*AudioListener.volume*1.42f);
				triangleVolume.guiTexture.pixelInset=t;
				if(AudioListener.volume<0.4f) triangleVolume.guiTexture.texture=triVolume1;
				else if (AudioListener.volume>=0.4f && AudioListener.volume<0.6f) triangleVolume.guiTexture.texture=triVolume2;
				else triangleVolume.guiTexture.texture=triVolume3;
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
				viderMenu ();
				Destroy (greyT);
				Time.timeScale = normalTime;
				Main.Restart();
				break;
			case "OPTIONS":
				displayMenu(menuOptions);
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

	void viderMenu()
	{
		authorizeNavigate = false;
		Destroy (titreAffiche);
		Destroy (triangleFond);
		Destroy (triangleVolume);
		Destroy (fleches);
		foreach (GameObject g in textureAffichees) Destroy (g);
		foreach (GameObject g in textAffiches) Destroy (g);
		textureAffichees =  new List <GameObject>();
		textAffiches =  new List <GameObject>();
		cameraMenu.SetActive (false);
	}

}
