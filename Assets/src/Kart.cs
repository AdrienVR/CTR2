using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Kart 
{
	
	public CameraController cm1c;
	private KartController kc;
	private GUIText pointText;
	public GUIText guitextApples; 
	
	public GameObject guiArme;
	public GameObject camera;
	public GameObject c2d;
	public GameObject superLight;
	public GameObject superLightWeapon;
	
	public int lastWeaponTextureNb = -1;
	public int numeroJoueur;
	private int nbPoints = 0;
	public int nbApples = 0;
	public int nbApplesFinal = 0;

	public static int nPlayer=0;
	public static int nbPlayers=0;
	private static float speedCoeff;
	private static float turnCoeff;

	public Kart(Vector3 pos, Quaternion q, string kart)
	{
		nbPoints = 0;
		nPlayer++;
		numeroJoueur = nPlayer;
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
		kart.name = kart.name.Split ('(') [0];
		kc = (KartController)kart.GetComponent ("KartController");
		foreach (Transform child in kart.transform)
			kc.wheels[child.name] = child;
		kc.SetKart(this);
		kc.setCoefficients (speedCoeff, turnCoeff);
	}

	public void InitCamera()
	{
		camera = GameObject.Instantiate (Resources.Load("cameraKart")) as GameObject;
		camera.camera.rect = Dictionnaries.cameraMap[nbPlayers][numeroJoueur-1];
		camera.camera.cullingMask |= (1 << LayerMask.NameToLayer("layer_j"+numeroJoueur));
		cm1c = (CameraController) camera.GetComponent ("CameraController");
		cm1c.SetKartController(kc);
		
		c2d = GameObject.Instantiate (Resources.Load("cameraGui")) as GameObject;
		c2d.transform.position = new Vector3 (c2d.transform.position.x, c2d.transform.position.y - numeroJoueur * 500, c2d.transform.position.z);
		c2d.camera.rect = Dictionnaries.cameraMap[nbPlayers][numeroJoueur-1];
		c2d.camera.cullingMask |= (1 << LayerMask.NameToLayer("layer2d_j"+numeroJoueur));
	}
	
	public void InitGui()
	{
		GameObject guiPoints = GameObject.Instantiate (Resources.Load ("guiPoints")) as GameObject;
		//guiPoints.transform.position = new Vector3 (guiPoints.transform.position.x, guiPoints.transform.position.y - numeroJoueur * 500, pointGui.transform.position.z);
		guiPoints.layer = LayerMask.NameToLayer ("layer_j" + numeroJoueur);
		pointText = (GUIText)guiPoints.GetComponent ("GUIText");
		pointText.text="0";
		if (nbPlayers > 2)
			pointText.transform.position = new Vector3(0.8f,pointText.transform.position.y,pointText.transform.position.z) ;

		GameObject guiApple = GameObject.Instantiate (Resources.Load("guiApple")) as GameObject;
		Resizer rs = (Resizer)guiApple.GetComponent ("Resizer");
		rs.rectCam = Dictionnaries.cameraMap [nbPlayers] [numeroJoueur - 1];
		guiApple.transform.position = new Vector3 (guiApple.transform.position.x, guiApple.transform.position.y - numeroJoueur * 500, guiApple.transform.position.z);
		guiApple.layer = LayerMask.NameToLayer ("layer2d_j" + numeroJoueur);
		foreach (Transform child in guiApple.transform)
		{
			child.gameObject.layer = LayerMask.NameToLayer ("layer2d_j" + numeroJoueur);
			if (child.gameObject.name == "superLight") superLight = child.gameObject;
			if (child.gameObject.name == "superLightWeapon") superLightWeapon = child.gameObject;
		}
		
		GameObject nbAppleGui = GameObject.Instantiate (Resources.Load ("guitextApples")) as GameObject;
		nbAppleGui.layer= LayerMask.NameToLayer ("layer_j" + numeroJoueur);
		guitextApples = (GUIText)nbAppleGui.GetComponent ("GUIText");
		guitextApples.text = "x 0";

		guiArme = GameObject.Instantiate (Resources.Load ("guiArme")) as GameObject;
		guiArme.layer = LayerMask.NameToLayer ("layer_j" + numeroJoueur);
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
		if (!IsSuper())
			guiArme.guiTexture.texture = GameObject.Instantiate (Resources.Load ("Pictures/"+Dictionnaries.normalWeapons[lastWeaponTextureNb])) as Texture;
		else
			guiArme.guiTexture.texture = GameObject.Instantiate (Resources.Load ("Pictures/"+Dictionnaries.superWeapons[lastWeaponTextureNb])) as Texture;
	}
	
	public void undrawWeaponGui(){
		lastWeaponTextureNb = -1;
		guiArme.guiTexture.texture = null;
	}

	public void AddPoint(int n)
	{
		// n = 1 or n = -1
		nbPoints+=n;
		pointText.text = nbPoints.ToString();
		if (nbPoints == Main.nbPtsPartie){
			Main.Restart ();
			Application.LoadLevel (Application.loadedLevel);
		}
	}

	public void addApples()
	{
		int n = Random.Range (4, 8);
		nbApplesFinal = System.Math.Min (10, nbApplesFinal+n);
		kc.animApples();
	}
	
	public void rmApples(int n)
	{
		nbApplesFinal -= n;
		nbApples -= n;
		nbApplesFinal = System.Math.Max (0, nbApplesFinal);
		nbApples = System.Math.Max (0, nbApples);
		if (nbApples != 10) SetIllumination(false);
		guitextApples.text = "x "+nbApples.ToString();
		drawWeaponGui();
	}
	
	public bool IsSuper()
	{
		return nbApples == 10;
	}

}










