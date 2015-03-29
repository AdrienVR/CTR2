using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Kart 
{
	
	public CameraController cm1c;
	private KartController kc;
	public KartScript kart_script;

	private GUIText pointText;
	public GUIText guitextApples; 
	
	public GameObject guiArme;
	public GameObject camera;
	public GameObject c2d;
	public GameObject superLight;
	public GameObject superLightWeapon;
	public GameObject guiPoints;
	
	public int lastWeaponTextureNb = -1;
	public int numeroJoueur;
	public int nbPoints = 0;
	public int nbApples = 0;
	public int nbApplesFinal = 0;
	public bool isWinner=false;

	public static int nPlayer=0;
	public static int totalPlayers=1;
	private static float speedCoeff;
	private static float turnCoeff; 

	public Kart(Vector3 pos, Quaternion q, string kart)
	{
		nbPoints = 0;
		numeroJoueur = ++nPlayer;
		InitObjet (pos, q, kart);
		InitCamera ();
		InitGui();
	}

	public static void setCoefficients(float speed, float turn){
		speedCoeff = speed;
		turnCoeff = turn;
	}

	public void InitObjet(Vector3 pos, Quaternion q, string kart_name)
	{
		GameObject kart = GameObject.Instantiate (Resources.Load("kart"+kart_name), pos, q) as GameObject;
		//GameObject kart_angles = GameObject.Instantiate (Resources.Load("GameplayObject"), pos, q) as GameObject;
		kart.name = kart.name.Split ('(') [0];
		kc = kart.GetComponent<KartController> ();
		kart_script= kart.GetComponent<KartScript> ();
		
		kc.SetKart(this);
		kart_script.SetKart(this);

		kc.setCoefficients (speedCoeff, turnCoeff);
		//kart_angles.GetComponent<Gameplay> ().SetKart (kart.transform);
	}

	public void InitCamera()
	{
		camera = GameObject.Instantiate (Resources.Load("cameraKart")) as GameObject;
		camera.camera.rect = Game.cameraMap[totalPlayers][numeroJoueur-1];
		camera.camera.cullingMask |= (1 << LayerMask.NameToLayer("layer_j"+numeroJoueur));
		cm1c = (CameraController) camera.GetComponent ("CameraController");
		cm1c.SetKartController(kc);
		
		c2d = GameObject.Instantiate (Resources.Load("cameraGui")) as GameObject;
		c2d.transform.position += new Vector3 (0, - numeroJoueur * 500);
		c2d.camera.rect = Game.cameraMap[totalPlayers][numeroJoueur-1];
		c2d.camera.cullingMask |= (1 << LayerMask.NameToLayer("layer2d_j"+numeroJoueur));
	}
	
	public void InitGui()
	{
		guiPoints = GameObject.Instantiate (Resources.Load ("guiPoints")) as GameObject;
		//guiPoints.transform.position = new Vector3 (guiPoints.transform.position.x, guiPoints.transform.position.y - numeroJoueur * 500, pointGui.transform.position.z);
		guiPoints.layer = LayerMask.NameToLayer ("layer2d_j" + numeroJoueur);
		pointText = (GUIText)guiPoints.GetComponent ("GUIText");
		pointText.text="0";
		if (totalPlayers > 2)
			pointText.transform.position = new Vector3(0.8f,pointText.transform.position.y,pointText.transform.position.z) ;

		GameObject guiApple = GameObject.Instantiate (Resources.Load("guiApple")) as GameObject;
		Resizer rs = (Resizer)guiApple.GetComponent ("Resizer");
		rs.rectCam = Game.cameraMap [totalPlayers] [numeroJoueur - 1];
		guiApple.transform.position += new Vector3 (0, - numeroJoueur * 500);
		guiApple.layer = LayerMask.NameToLayer ("layer2d_j" + numeroJoueur);
		foreach (Transform child in guiApple.transform)
		{
			child.gameObject.layer = LayerMask.NameToLayer ("layer2d_j" + numeroJoueur);
			if (child.gameObject.name == "superLight") superLight = child.gameObject;
			if (child.gameObject.name == "superLightWeapon") superLightWeapon = child.gameObject;
		}
		
		GameObject nbAppleGui = GameObject.Instantiate (Resources.Load ("guitextApples")) as GameObject;
		nbAppleGui.layer= LayerMask.NameToLayer ("layer2d_j" + numeroJoueur);
		guitextApples = (GUIText)nbAppleGui.GetComponent ("GUIText");
		guitextApples.text = "x 0";

		guiArme = GameObject.Instantiate (Resources.Load ("guiArme")) as GameObject;
		guiArme.layer = LayerMask.NameToLayer ("layer2d_j" + numeroJoueur);
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
	
	public void SetIllumination(bool a)
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

	public void drawWeaponGui(){
		if (lastWeaponTextureNb == -1)
			return;
		string textureName;
		if (IsSuper())
			textureName = Game.superWeapons[lastWeaponTextureNb];
		else
			textureName = Game.normalWeapons[lastWeaponTextureNb];
		int count = 0;
		if (textureName == "triple_bomb"){
			foreach (string element in kart_script.weapons)
				if (element=="bomb")
					count++;
			if (count == 0)
				count = 3;
			textureName = "bomb"+count;
		}
		else if (textureName == "triple_missile"){
			foreach (string element in kart_script.weapons)
				if (element=="missile")
					count++;
			if (count == 0)
				count = 3;
			textureName = "missile"+count;
		}
		if(guiArme)
			guiArme.guiTexture.texture = GameObject.Instantiate (Resources.Load ("Pictures/"+textureName)) as Texture;
	}
	
	public void undrawWeaponGui(){
		lastWeaponTextureNb = -1;
		if(guiArme)
			guiArme.guiTexture.texture = null;
	}

	public void AddPoint(int n)
	{
		// n = 1 or n = -1
		if (KartController.IA_enabled)
			return;
		nbPoints+=n;
		if(pointText)
			pointText.text = nbPoints.ToString();
		if (nbPoints == Main.nbPtsPartie){
			isWinner=true;
			kc.gameObject.AddComponent<Party>();
			KartController.IA_enabled = true;
		}
	}

	public void addApples()
	{
		int n = Random.Range (4, 8);
		nbApplesFinal = System.Math.Min (10, nbApplesFinal+n);
		kart_script.animApples();
	}
	
	public void rmApples(int n)
	{
		nbApplesFinal -= n;
		nbApples -= n;
		nbApplesFinal = System.Math.Max (0, nbApplesFinal);
		nbApples = System.Math.Max (0, nbApples);
		if (nbApples != 10) SetIllumination(false);
		if(guitextApples)
			guitextApples.text = "x "+nbApples.ToString();
		drawWeaponGui();
	}
	
	public bool IsSuper()
	{
		return nbApples == 10;
	}

}










