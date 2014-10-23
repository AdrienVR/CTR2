using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Kart 
{
	public static int nPlayer=0;
	public CameraController cm1c;
	public int numeroJoueur;
	public int lastWeaponTextureNb;
	public GameObject armeGui;
	public WeaponScript ws;
	private GUIText pointText;
	public GUIText pommeText;

	public GameObject camera;
	public GameObject c2d;
	public GameObject superLight;
	public GameObject superLightWeapon;

	private static int nbPlayers=0;
	private int nbPoints = 0;
	public int nbApples = 0;
	public int nbApplesFinal = 0;
	private KartController kc;
	private static Dictionary <int, List<Rect>> cameraMap = new Dictionary <int, List<Rect>>{
		{1, new List<Rect>(){new Rect(0, 0, 1, 1)}},
		{2, new List<Rect>(){new Rect(0, 0.51f, 1, 0.49f), new Rect(0, 0, 1, 0.49f)}},
		{3, new List<Rect>(){new Rect(0, 0.51f, 1, 0.49f), new Rect(0, 0, 0.495f, 0.49f), 
				new Rect(0.505f, 0, 0.495f, 0.49f)}},
		{4, new List<Rect>(){new Rect(0, 0.51f, 0.49f, 0.49f), new Rect(0, 0, 0.49f, 0.49f), 
				new Rect(0.51f, 0.51f, 0.49f, 0.49f), new Rect(0.51f, 0, 0.49f, 0.49f)}}
	};

	public Kart(Vector3 pos, Quaternion q)
	{
		nbPoints = 0;
		nPlayer++;
		numeroJoueur = nPlayer;
		InitObjet (pos, q);
		InitCamera ();
		InitGuiLayer ();
	}

	public static void SetNbPlayers(int n)
	{
		nbPlayers = n;
	}

	public void blackScreen()
	{
		camera.camera.enabled = false;
		c2d.camera.enabled = false;
	}

	public void normalScreen()
	{
		camera.camera.enabled = true;
		c2d.camera.enabled = true;
	}

	public void InitObjet(Vector3 pos, Quaternion q)
	{
		int j = numeroJoueur;
		Dictionary <int, string> prefabMap = new Dictionary <int, string>{{1,"crash_prefab"},{2,"crash_prefab"},{3,"crash_prefab"},{4,"crash_prefab"}};

		GameObject kart= Resources.Load(prefabMap[j]) as GameObject;
		kart = GameObject.Instantiate (kart, pos, q) as GameObject;
		kart.name = kart.name.Split ('(') [0];
		kc = (KartController)kart.GetComponent ("KartController");
		kc.SetKart(this);
	}

	public void InitCamera()
	{
		camera = GameObject.Instantiate (Resources.Load("Camera_prefab")) as GameObject;
		camera.camera.rect = cameraMap[nbPlayers][numeroJoueur-1];
		camera.camera.cullingMask |= (1 << LayerMask.NameToLayer("layer_j"+numeroJoueur));
		cm1c = (CameraController) camera.GetComponent ("CameraController");
		cm1c.SetKartController(kc);
		
		c2d = GameObject.Instantiate (Resources.Load("Camera2D_prefab")) as GameObject;
		c2d.transform.position = new Vector3 (c2d.transform.position.x, c2d.transform.position.y - numeroJoueur * 500, c2d.transform.position.z);
		c2d.camera.rect = cameraMap[nbPlayers][numeroJoueur-1];
		c2d.camera.cullingMask |= (1 << LayerMask.NameToLayer("layer2d_j"+numeroJoueur));
	}
	
	public void InitGuiLayer()
	{
		setWeaponGUI ("arme");
		GameObject pointGui = GameObject.Instantiate (Resources.Load ("pointTexture")) as GameObject;
		//pointGui.transform.position = new Vector3 (pointGui.transform.position.x, pointGui.transform.position.y - numeroJoueur * 500, pointGui.transform.position.z);
		pointGui.layer= LayerMask.NameToLayer ("layer_j" + numeroJoueur);
		pointText = (GUIText)pointGui.GetComponent ("GUIText");
		pointText.text="0";
		if (nbPlayers > 2)
			pointText.transform.position = new Vector3(0.8f,pointText.transform.position.y,pointText.transform.position.z) ;

		GameObject pomme = GameObject.Instantiate (Resources.Load("apple_prefab")) as GameObject;
		Resizer rs = (Resizer)pomme.GetComponent ("Resizer");
		rs.rectCam = cameraMap [nbPlayers] [numeroJoueur - 1];
		pomme.transform.position = new Vector3 (pomme.transform.position.x, pomme.transform.position.y - numeroJoueur * 500, pomme.transform.position.z);
		pomme.layer = LayerMask.NameToLayer ("layer2d_j" + numeroJoueur);
		foreach (Transform child in pomme.transform)
		{
			child.gameObject.layer = LayerMask.NameToLayer ("layer2d_j" + numeroJoueur);
			if (child.gameObject.name == "superLight") superLight = child.gameObject;
			if (child.gameObject.name == "superLightWeapon") superLightWeapon = child.gameObject;
		}
		
		GameObject nbAppleGui = GameObject.Instantiate (Resources.Load ("pommeText")) as GameObject;
		nbAppleGui.layer= LayerMask.NameToLayer ("layer_j" + numeroJoueur);
		pommeText = (GUIText)nbAppleGui.GetComponent ("GUIText");
		pommeText.text = "x 0";
	}

	public void setWeaponGUI(string armeImage)
	{
		armeGui = GameObject.Instantiate (Resources.Load (armeImage)) as GameObject;
		armeGui.layer = LayerMask.NameToLayer ("layer_j" + numeroJoueur);
		ws = (WeaponScript)armeGui.GetComponent ("WeaponScript");
		if(kc.IsArmed())
		{
			ws.SetTextureN(lastWeaponTextureNb);
			kc.setEvoluteWeapon(true);
			if(armeImage=="arme") kc.SetWeapon(WeaponBoxScript.normalWeapons[lastWeaponTextureNb]);
			else if(armeImage=="superArme") kc.SetWeapon(WeaponBoxScript.superWeapons[lastWeaponTextureNb]);
		}
	}

	public void SetIlluminated(bool a)
	{
		if (a){
			superLight.light.color = new Color(114,113,0);
			superLightWeapon.light.color = new Color(114,113,0);
		}
		else{
			superLight.light.color = new Color();
			superLightWeapon.light.color = new Color();
		}
	}

	public void AddPoint(int n)
	{
		nbPoints+=n;
		//Debug.Log (numeroJoueur + " a " + nbPoints + " points !");
		pointText.text = nbPoints.ToString();
		if (nbPoints == Main.nbPtsPartie)
			Main.Restart ();
	}
}










