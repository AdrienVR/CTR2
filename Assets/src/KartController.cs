using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using System;


public class KartController : MonoBehaviour
{
	public static Dictionary <int, Dictionary<string, KeyCode>> playersMapping;
	public static Dictionary <int, Dictionary<string, string>> axisMapping;
	public static Dictionary <int, bool> controllersEnabled = new Dictionary <int, bool> {
		{1,true},{2,true},{3,true},{4,true},{5,true},{6,true}
	};
	public static int nControllers = 0;
	public static bool stop = true;
	private bool stopDie = false;
	private bool isGoingInAir = false;
	
	public float coeffVitesse=2f;
	public float coeffManiabilite=4f;
	public float coeffVInit;
	
	private bool hasAxis = true;
	private Vector3 postForce;
	private Vector3 lowForce;
	
	private bool pressX=false;
	private bool pressFleche=false;
	private bool pressR1=false;
	private bool pressL1=false;
	private bool pressXAndFleche = false;
	private bool pressXAndFlecheAndR1 = false;

	private WeaponBoxScript takenWeaponBox;
	private ExplosionScript shield;
	private ExplosionScript protection;
	
	private List<string> state = new List<string>();
	public List<string> weapons;
	public Dictionary <string, KeyCode> keyMap;
	
	private bool cameraReversed=false;
	private Kart kart;
	private bool dansLesAirs = true;
	private Dictionary <string, string> axisMap;
	private float ky;
	private bool baddie = false;
	
	private ExplosionScript arme;
	public bool explosiveWeapon;
	private float facteurSens = 1f;
	private static List<string> poseWeapons = new List<string>() {"nitro", "TNT", "greenBeaker", "redBeaker"};
	private static List<string> bombList = new List<string>() {"bomb", "superBomb"};
	
	// Use this for initialization
	void Start ()
	{
		coeffVInit = coeffVitesse;
		explosiveWeapon = false;
		weapons = new List<string>();
		if (playersMapping == null)
			InitMapping ();
		// clavier pour joueur n°3 si 2 manettes deja connectees.
		if (kart.numeroJoueur == nControllers + 1 || kart.numeroJoueur == nControllers + 2) {
			hasAxis = false;
			controllersEnabled[kart.numeroJoueur] = false;
		}
		InitSelfMapping ();
	}
	
	void FixedUpdate()
	{

		if (postForce.Equals(new Vector3())){
			rigidbody.rotation = transform.rotation;
			rigidbody.position = transform.position;
			rigidbody.velocity = new Vector3(rigidbody.velocity.x/1.10f, rigidbody.velocity.y, rigidbody.velocity.z/1.10f);
			rigidbody.velocity = new Vector3(lowForce.x, rigidbody.velocity.y, lowForce.z);
		}
		else
			rigidbody.velocity = new Vector3(postForce.x, rigidbody.velocity.y, postForce.z);

		if (dansLesAirs)
			rigidbody.velocity = new Vector3(rigidbody.velocity.x,-26f,rigidbody.velocity.z);
	}

	
	void Update()
	{
		lowForce = new Vector3 ();
		postForce = new Vector3 ();
		if (!stop && !stopDie)
			controlPosition ();
	}

	void OnCollisionStay(Collision collision)
	{
		if(collision.gameObject.name=="Ground")
			dansLesAirs = false;
		
		/*if(collision.gameObject.name=="accelerateur")
			rigidbody.velocity = new Vector3(rigidbody.velocity.x*3,rigidbody.velocity.y*3,rigidbody.velocity.z*3);*/
	}
	
	void OnCollisionExit(Collision collision)
	{
		if(collision.gameObject.name=="Ground") {
			if (!isGoingInAir)
				StartCoroutine(FreeAir());
		}
	}
	
	IEnumerator FreeAir()
	{
		isGoingInAir = true;
		yield return 0;
		dansLesAirs = true;
		isGoingInAir = false;
	}
	
	public bool IsArmed()
	{
		return state.IndexOf("armed")!=-1;
	}
	
	public void SetWeaponBox(WeaponBoxScript wp)
	{
		takenWeaponBox = wp;
	}
	
	public bool IsWaitingWeapon()
	{
		return state.IndexOf("waiting")!=-1;
	}
	public bool IsSuper()
	{
		return state.IndexOf("super")!=-1;
	}
	
	public void setWaitingWeapon(bool t)
	{
		if (t){
			if (state.IndexOf("waiting")==-1)
				state.Add("waiting");
		}
		else
			if (state.IndexOf("waiting")!=-1)
				state.Remove("waiting");
	}
	
	public void setEvoluteWeapon(bool t)
	{
		if (t){
			if (state.IndexOf("armedEvolute")==-1)
				state.Add("armedEvolute");
		}
		else
			if (state.IndexOf("armedEvolute")!=-1)
				state.Remove("armedEvolute");
	}
	
	public void SetWeapon(string w)
	{
		if (w == "Aku-Aku" )
			if (baddie)
				w = "Uka-Uka";
		else if (w == "superAku-Aku")
			if (baddie)
				w = "superUka-Uka";
		
		if (w.IndexOf ("triple") != -1){
			int j = weapons.Count;
			if (j == 0) j=3;
			weapons = new List<string> ();
			for(int i=0;i<j;i++)
				weapons.Add (w.Split('_')[w.Split('_').Length - 1]);}
		else{
			weapons = new List<string> ();
			weapons.Add (w);
		}
		if (state.IndexOf("armed")==-1)
			state.Add ("armed");
		takenWeaponBox = null;
	}
	
	public void UseWeapon()
	{
		// stop the random searching of weapon from WeaponBox
		if (takenWeaponBox != null){
			if (takenWeaponBox.selectRandomWeapon ())
				takenWeaponBox = null;
			return;
		}
		
		Vector3 forwardNormal = transform.forward;
		if (transform.forward.y>0)
			forwardNormal.y = 0;
		forwardNormal = normalizeVector (forwardNormal);
		Vector3 posToAdd ;
		//launch the shield
		if (shield != null) {
			shield.vitesseInitiale =  100f*forwardNormal;
			shield.name = "bomb";
			posToAdd = transform.position + 6f * (new Vector3 (forwardNormal.x, forwardNormal.y+0.6f, forwardNormal.z));
			shield.transform.position = posToAdd;
			shield.transform.rotation = new Quaternion();
			shield.activePhysics();
			shield.transform.localScale = new Vector3(0.66f,0.75f,0.66f);
			shield = null;
			return;
		}
		
		if (weapons.Count == 0)
			return;
		string w = weapons [0];
		
		float sens = -1f;
		if (hasAxis && Input.GetAxis (axisMap ["stop"]) < -0.1f)
			sens = 1f;

		// computing the distance to instantiate the weapon
		if (bombList.IndexOf(w)!=-1)
			posToAdd = 6f * (new Vector3 (facteurSens * forwardNormal.x, forwardNormal.y + 0.6f, facteurSens * forwardNormal.z));
		else if (poseWeapons.IndexOf(w) != -1){
			if (w == "greenBeaker" || w=="redBeaker")
				posToAdd = 4f * (new Vector3 (sens*forwardNormal.x, forwardNormal.y + 0.2f, sens*forwardNormal.z));
			else
				posToAdd = 4f * (new Vector3 (-forwardNormal.x, forwardNormal.y + 0.2f, -forwardNormal.z));
		}
		else
			posToAdd = 6f * (new Vector3 (forwardNormal.x, forwardNormal.y + 0.6f, forwardNormal.z));
		Quaternion q = new Quaternion (0,transform.rotation.y,0,transform.rotation.w);
		if (poseWeapons.IndexOf(w) != -1)
			q = transform.rotation;

		//instantiate the weapon
		GameObject arme1 = Instantiate(Resources.Load("weapons/"+w), transform.position + posToAdd, q) as GameObject;
		arme1.name = arme1.name.Split ('(') [0];

		//compute the velocity
		arme = (ExplosionScript) arme1.GetComponent ("ExplosionScript");
		if (arme!=null)	{
			arme.owner = gameObject;
			
			if (bombList.IndexOf(w)!=-1) {
				explosiveWeapon = true;
				arme.vitesseInitiale =  90f*new Vector3(facteurSens * forwardNormal.x, 0, facteurSens * forwardNormal.z);
			}
			else if (w == "missile")
				arme.vitesseInitiale =  120f*forwardNormal;
			else if (w == "greenShield"|| w == "blueShield"){
				shield = arme;
				shield.lifeTime = 14f;
			}
			else if (w == "Aku-Aku" || w == "superAku-Aku") {
				if (protection!=null){
					if (kart.nbApples == 10)
						protection.lifeTime = 10f;
					else
						protection.lifeTime = 7f;
					Destroy(arme.gameObject);
				}
				else
					protection = arme;
			}
			else if (w == "greenBeaker" || w=="redBeaker")
				if (sens == 1f)
					arme.rigidbody.AddForce(2000f*new Vector3(sens * forwardNormal.x, 0.2f, sens * forwardNormal.z));
					//arme.vitesseInstant =  90f*new Vector3(sens * forwardNormal.x, 0, sens * forwardNormal.z);
		}

		//use the weapon so remove
		weapons.RemoveAt (0);
		if (weapons.Count == 0) {
			state.Remove ("armed");
			GetKart().ws.guiTexture.texture = null;
		}
		if(state.IndexOf("armedEvolute")!=-1)
		{
			state.Remove ("armed");
			state.Remove ("armed");
			state.Remove ("armedEvolute");
		}
	}
	
	public void Die(GameObject killer, string weapon)
	{
		if (shield != null)
		{
			StartCoroutine( TempUndead());
			Destroy (shield.gameObject);
			return;
		}
		if (protection != null)
			return;
		// si on est pas invincible : on meurt
		if (state.IndexOf ("invincible") == -1)
		{
			StartCoroutine (Transparence ());
			StartCoroutine (UnableToMove ());
			// mise en etat empechant de tirer : 
			if (state.IndexOf ("UnableToShoot") == -1)
				StartCoroutine (UnableToShoot ());
			if (killer==gameObject)
				kart.AddPoint(-1);
			else
				((KartController)killer.GetComponent ("KartController")).kart.AddPoint(1);
			if (weapon=="greenBeaker" || weapon== "redBeaker") // pour retirer des pommes
			{
				rmApples(1);
			}
			else rmApples(3);
		}
	}

	IEnumerator TempUndead()
	{
		//clignotment, invincibilité temporaire
		if (state.IndexOf("invincible")==-1)
			state.Add ("invincible");
		float time = 0f;
		while (time < 0.35f) {
			yield return new WaitForSeconds (0.05f);
			time += 0.05f;
		}
		state.Remove ("invincible");
	}

	IEnumerator UnableToMove()
	{
		stopDie = true;
		float time = 0f;
		while (time < 1f) {
			yield return new WaitForSeconds (0.1f);
			time += 0.1f;
		}
		stopDie = false;
	}
	
	IEnumerator UnableToShoot()
	{
		if (state.IndexOf("UnableToShoot")==-1)
			state.Add ("UnableToShoot");
		float time = 0f;
		while (time < 2.5f) {
			yield return new WaitForSeconds (0.1f);
			time += 0.1f;
		}
		state.Remove ("UnableToShoot");
	}
	
	IEnumerator Transparence()
	{
		//clignotment, invincibilité temporaire
		if (state.IndexOf("invincible")==-1)
			state.Add ("invincible");
		renderer.enabled = false;
		float time = 0f;
		float last_time = 0f;
		float clignotement = 0.3f;
		while (time < 4f) {
			yield return new WaitForSeconds (0.1f);
			time += 0.1f;
			if ((time - last_time) > clignotement)
			{
				last_time = time;
				clignotement /= 2;
				renderer.enabled = !renderer.enabled;
			}
		}
		renderer.enabled = true;
		state.Remove ("invincible");
	}
	
	public void addApples()
	{
		int n = Random.Range (4, 8);
		kart.nbApplesFinal = System.Math.Min (10, kart.nbApplesFinal+n);
		StartCoroutine(animApplesNb());
	}
	
	public void rmApples(int n)
	{
		kart.nbApplesFinal -= n;
		kart.nbApples -= n;
		kart.nbApplesFinal = System.Math.Max (0, kart.nbApplesFinal);
		kart.nbApples = System.Math.Max (0, kart.nbApples);
		if (kart.nbApples != 10) kart.SetIlluminated(false);
		kart.pommeText.text = "x "+kart.nbApples.ToString();
		state.Remove("super");
		Destroy(kart.armeGui);
		kart.setWeaponGUI ("arme");
	}
	
	IEnumerator animApplesNb()
	{
		while(kart.nbApplesFinal != kart.nbApples)
		{
			kart.nbApples ++;
			kart.SetIlluminated((kart.nbApples == 10));
			audio.Play();
			kart.pommeText.text = "x "+kart.nbApples.ToString();
			yield return new WaitForSeconds (0.27f);
		}
		if(kart.nbApplesFinal == 10 && !IsSuper())
		{
			if(IsArmed())
			{
				Destroy(kart.armeGui);
			}
			if (state.IndexOf("super")==-1)
				state.Add ("super");
			kart.setWeaponGUI ("superArme");
		}
	}
	
	public void SetKart (Kart k)
	{
		kart = k;
	}
	
	
	public Kart GetKart ()
	{
		return kart;
	}
	
	public Vector3 normalizeVector(Vector3 a)
	{
		float div = Mathf.Sqrt (a.x * a.x + a.y * a.y + a.z * a.z);
		a.x /= div;
		a.y /= div;
		a.z /= div;
		return a;
	}
	
	public void controlDerapage()
	{
		
		if(Input.GetKeyDown(keyMap["moveForward"]))
		{
			pressX=true;
		}
		if(Input.GetKeyUp(keyMap["moveForward"]))
		{
			pressX=false;
			pressXAndFleche=false;
			pressXAndFlecheAndR1=false;
			//coeffManiabilite=2;
			//coeffVitesse=3;
		}
		if(Input.GetKeyDown(keyMap["jump"]))
		{
			pressR1=true;
		}
		if(Input.GetKeyUp(keyMap["jump"]))
		{
			pressR1=false;
			pressXAndFlecheAndR1=false;
			//coeffManiabilite=2;
			//coeffVitesse=3;
		}
		if(Input.GetKeyDown(keyMap["jump2"]))
		{
			pressL1=true;
		}
		if(Input.GetKeyUp(keyMap["jump2"]))
		{
			pressL1=false;
		}
		if(pressX && pressFleche)
		{
			pressXAndFleche=true;
		}
		if(pressXAndFleche && pressR1)
		{
			pressXAndFlecheAndR1=true;
		}
		if(pressXAndFlecheAndR1)
		{
			Debug.Log("JE DERAPE");
			//coeffManiabilite=4;
			//coeffVitesse=3;
		}
		
		if(!pressXAndFlecheAndR1 || !pressL1)
		{
		}
	}
	
	public void controlPosition()
	{
		//rigidbody.position = transform.position;
		Vector3 forwardNormal = transform.forward;
		forwardNormal.y = 0;
		forwardNormal = normalizeVector (forwardNormal);
		
		if(Input.GetKey(keyMap["moveBack"])) {
			if (!hasAxis){
				lowForce = -forwardNormal*coeffVitesse;
				if(Input.GetKey(keyMap["turnLeft"]))
					transform.Rotate(0,-0.5f*coeffManiabilite,0);
				if(Input.GetKey(keyMap["turnRight"]))
					transform.Rotate(0,0.5f*coeffManiabilite,0);
			}
		}
		else if (hasAxis)
			if(Input.GetAxis (axisMap ["stop"]) > 0.1f)
				lowForce = -Input.GetAxis (axisMap ["stop"]) * forwardNormal * coeffVitesse;
		
		if(Input.GetKey(keyMap["moveForward"]))
		{
			postForce = forwardNormal*coeffVitesse;
			if (!hasAxis)
			{
				if(Input.GetKey(keyMap["turnLeft"]))
					transform.Rotate(0,0.5f*coeffManiabilite,0);
				if(Input.GetKey(keyMap["turnRight"]))
					transform.Rotate(0,-0.5f*coeffManiabilite,0);
			}
		}
		if (Input.GetKeyDown (keyMap ["viewInverse"])) {
			cameraReversed = true;
		}
		if (Input.GetKeyUp (keyMap ["viewInverse"])) {
			cameraReversed = false;
			kart.cm1c.positionForward = 1f ;
		}
		if (Input.GetKey (keyMap ["viewInverse"])) {
			if (cameraReversed)
				kart.cm1c.positionForward = -1f ;
		}
		if(Input.GetKeyDown(keyMap["jump"]))
		{
			if(dansLesAirs==false)
			{
				rigidbody.position += new Vector3(0,3f,0);
			}
		}
		if (hasAxis){
			if(!Input.GetKey(keyMap["moveBack"]) && System.Math.Abs(Input.GetAxis (axisMap ["turn"]))>0.1f){
				if(Input.GetAxis (axisMap ["stop"]) > 0.1f){
					transform.Rotate (0, -Input.GetAxis (axisMap ["turn"]) * coeffManiabilite, 0);
				}
				else if(Input.GetKey(keyMap["moveForward"]) || Input.GetKeyDown(keyMap["jump"]))
					transform.Rotate (0, Input.GetAxis (axisMap ["turn"]) * coeffManiabilite, 0);
			}
			if(Input.GetKey(keyMap["moveBack"])){
				transform.Rotate (0, Input.GetAxis (axisMap ["turn"]) * coeffManiabilite, 0);
			}
		}
		
		if (Input.GetKeyDown (keyMap ["action"])) {
			if (state.IndexOf ("UnableToShoot") != -1)
				return;
			facteurSens = 1f;
			if (hasAxis && Input.GetAxis (axisMap ["stop"]) > 0.1f)
				facteurSens = -1f;
			else if (!hasAxis && Input.GetKey(keyMap["moveBack"]))
				facteurSens = -1f;
			
			if (!explosiveWeapon)
				UseWeapon ();
			else {
				explosiveWeapon = false;
				arme.ActionExplosion ();
			}
		}
	}
	
	void InitSelfMapping()
	{
		Dictionary <string, string> ps1_axis = new Dictionary<string, string> {
			{"turn","J1_TurnAxis"}, {"stop","J1_StopAxis"}		};
		Dictionary <string, string> ps2_axis = new Dictionary<string, string> {
			{"turn","J2_TurnAxis"}, {"stop","J2_StopAxis"}		};
		Dictionary <string, string> ps3_axis = new Dictionary<string, string> {
			{"turn","J3_TurnAxis"}, {"stop","J3_StopAxis"}		};
		Dictionary <string, string> ps4_axis = new Dictionary<string, string> {
			{"turn","J4_TurnAxis"}, {"stop","J4_StopAxis"}		};
		List<Dictionary <string, string> > l_axis = new List<Dictionary<string, string>> {
			ps1_axis,ps2_axis,ps3_axis,ps4_axis	};
		
		
		if (hasAxis)
			axisMap = l_axis[kart.numeroJoueur-1];
		
		keyMap = playersMapping [kart.numeroJoueur];
		axisMapping = new Dictionary<int, Dictionary<string, string>> {{1,ps1_axis},{2,ps2_axis},{3,ps3_axis},{4,ps4_axis}};
	}
	
	void InitMapping()
	{
		// constructs the static playersMapping => all 4 saved
		Dictionary <string, KeyCode> pc1 = new Dictionary<string, KeyCode> {
			{"moveForward",KeyCode.Z}, {"moveBack",KeyCode.S},
			{"turnRight",KeyCode.Q}, {"turnLeft",KeyCode.D},
			{"jump",KeyCode.Space}, {"jump2",KeyCode.F5}, 
			{"action",KeyCode.A}, {"start",KeyCode.Escape}, 
			{"viewChange",KeyCode.F1}, {"viewInverse",KeyCode.F2},
			{"bip",KeyCode.F3}, {"bip2",KeyCode.F4}
		};
		Dictionary <string, KeyCode> pc2 = new Dictionary<string, KeyCode> {
			{"moveForward",KeyCode.I}, {"moveBack",KeyCode.K},
			{"turnRight",KeyCode.J}, {"turnLeft",KeyCode.L},
			{"jump",KeyCode.B}, {"jump2",KeyCode.F11}, 
			{"action",KeyCode.U}, {"start",KeyCode.F12}, 
			{"viewChange",KeyCode.F7}, {"viewInverse",KeyCode.F8},
			{"bip",KeyCode.F9}, {"bip2",KeyCode.F10}
		};
		Dictionary <string, KeyCode> ps1 = new Dictionary<string, KeyCode> {
			{"moveForward",KeyCode.Joystick1Button2}, {"moveBack",KeyCode.Joystick1Button3},
			{"jump",KeyCode.Joystick1Button7}, {"jump2",KeyCode.Joystick1Button6},
			{"action",KeyCode.Joystick1Button1},{"start",KeyCode.Joystick1Button9},
			{"viewChange",KeyCode.Joystick1Button4}, {"viewInverse",KeyCode.Joystick1Button5},
			{"bip",KeyCode.Joystick1Button10}, {"bip2",KeyCode.Joystick1Button11}
		};
		Dictionary <string, KeyCode> ps2 = new Dictionary<string, KeyCode> {
			{"moveForward",KeyCode.Joystick2Button2}, {"moveBack",KeyCode.Joystick2Button3},
			{"jump",KeyCode.Joystick2Button7}, {"jump2",KeyCode.Joystick2Button6},
			{"action",KeyCode.Joystick2Button1},{"start",KeyCode.Joystick2Button9},
			{"viewChange",KeyCode.Joystick2Button4}, {"viewInverse",KeyCode.Joystick2Button5},
			{"bip",KeyCode.Joystick2Button10}, {"bip2",KeyCode.Joystick2Button11}
		};
		Dictionary <string, KeyCode> ps3 = new Dictionary<string, KeyCode> {
			{"moveForward",KeyCode.Joystick3Button2}, {"moveBack",KeyCode.Joystick3Button3},
			{"jump",KeyCode.Joystick3Button7}, {"jump2",KeyCode.Joystick3Button6},
			{"action",KeyCode.Joystick3Button1},{"start",KeyCode.Joystick3Button9},
			{"viewChange",KeyCode.Joystick3Button4}, {"viewInverse",KeyCode.Joystick3Button5},
			{"bip",KeyCode.Joystick3Button10}, {"bip2",KeyCode.Joystick3Button11}
		};
		Dictionary <string, KeyCode> ps4 = new Dictionary<string, KeyCode> {
			{"moveForward",KeyCode.Joystick4Button2}, {"moveBack",KeyCode.Joystick4Button3},
			{"jump",KeyCode.Joystick4Button7}, {"jump2",KeyCode.Joystick4Button6},
			{"action",KeyCode.Joystick4Button1},{"start",KeyCode.Joystick4Button9},
			{"viewChange",KeyCode.Joystick4Button4}, {"viewInverse",KeyCode.Joystick4Button5},
			{"bip",KeyCode.Joystick4Button10}, {"bip2",KeyCode.Joystick4Button11}
		};
		
		nControllers = Input.GetJoystickNames ().Length;
		if (nControllers > 4)
			nControllers = 4;
		
		playersMapping = new Dictionary<int, Dictionary<string, KeyCode>> {{1,ps1},{2,ps2},{3,ps3},{4,ps4}};
		
		playersMapping[nControllers + 1] = pc1;
		playersMapping[nControllers + 2] = pc2;
		
	}
	
}
