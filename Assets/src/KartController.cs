using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class KartController : MonoBehaviour
{
	public static Dictionary <int, Dictionary<string, KeyCode>> playersMapping;
	public static int nControllers = 0;

	public float coeffVitesse=2f;
	public float coeffManiabilite=4f;

	private bool hasAxis = true;

	private bool pressX=false;
	private bool pressFleche=false;
	private bool pressR1=false;
	private bool pressL1=false;
	private bool pressXAndFleche = false;
	private bool pressXAndFlecheAndR1 = false;

	private Vector3 velocityToApplyByJonathan;
	private WeaponBoxScript takenWeaponBox;
	private ExplosionScript shield;
	private ExplosionScript protection;

	public List<string> state;
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
	public float facteurSens ;
	private static List<string> poseWeapons = new List<string>() {"nitro", "TNT", "greenBeaker", "redBeaker"};
	
	// Use this for initialization
	void Start ()
	{
		explosiveWeapon = false;
		weapons = new List<string>();
		if (playersMapping == null)
			InitMapping ();
		if (kart.numeroJoueur == nControllers + 1 || kart.numeroJoueur == nControllers + 2)
			hasAxis = false;
		InitSelfMapping ();
	}
	
	void Update()
	{
		transform.rotation = new Quaternion (transform.rotation.x, transform.rotation.y,
		                                    transform.rotation.z, transform.rotation.w);

		rigidbody.velocity = new Vector3(rigidbody.velocity.x,-9.81f,rigidbody.velocity.z);

		// INDISPENSABLE : annule la possibilité de CONTROLER la rotation z
		rigidbody.angularVelocity = Vector3.zero;

	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (keyMap == null)
			return;
		if (keyMap ["moveForward"] == KeyCode.Joystick3Button2 && !hasAxis)
			return;
		if (keyMap ["moveForward"] == KeyCode.Joystick4Button2 && !hasAxis)
			return;

		controlPosition ();
	}
	
	void OnCollisionStay(Collision collision)
	{
		if(collision.gameObject.name=="Ground")
			dansLesAirs = false;

		if(collision.gameObject.name=="accelerateur")
			rigidbody.velocity = new Vector3(rigidbody.velocity.x*3,rigidbody.velocity.y*3,rigidbody.velocity.z*3);
	}
	
	void OnCollisionExit(Collision collision)
	{
		if(collision.gameObject.name=="Ground")
		{
			dansLesAirs = true;
		}
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

	public void setWaitingWeapon(bool t)
	{
		if (t)
			state.Add("waiting");
		else
			state.Remove("waiting");
	}

	public void SetWeapon(string w)
	{
		if (w == "Aku-Aku" )
			if (baddie)
				w = "Uka-Uka";
		else if (w == "superAku-Aku")
			if (baddie)
				w = "superUka-Uka";

		if (w.IndexOf("triple") != -1 )
			for(int i=0;i<3;i++)
				weapons.Add (w.Split('_')[w.Split('_').Length - 1]);
		else
			weapons.Add (w);
		state.Add ("armed");
		takenWeaponBox = null;
	}

	public void UseWeapon()
	{
		// stop the random searching of weapon from WeaponBox
		if (takenWeaponBox != null){
			takenWeaponBox.selectRandomWeapon ();
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
			posToAdd = transform.position - 6f * (new Vector3 (-facteurSens * forwardNormal.x, forwardNormal.y - 0.6f, -facteurSens * forwardNormal.z));
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
		if (w == "bomb")
			posToAdd = 6f * (new Vector3 (-facteurSens * forwardNormal.x, forwardNormal.y - 0.6f, -facteurSens * forwardNormal.z));
		else if (poseWeapons.IndexOf(w) != -1)
			posToAdd = 4f * (new Vector3 (forwardNormal.x, forwardNormal.y - 0.6f, forwardNormal.z));
		else
			posToAdd = 6f * (new Vector3 (-1 * forwardNormal.x, forwardNormal.y - 0.6f, -1* forwardNormal.z));

		Quaternion q = new Quaternion (0,transform.rotation.y,0,transform.rotation.w);
		if (poseWeapons.IndexOf(w) != -1)
			q = transform.rotation;

		GameObject arme1 = Instantiate(Resources.Load("weapons/"+w), transform.position-posToAdd, q) as GameObject;
		arme = (ExplosionScript) arme1.GetComponent ("ExplosionScript");
		if (arme!=null)	{
			arme.owner = gameObject;

			if (w == "bomb") {
				explosiveWeapon = true;
				arme.vitesseInitiale =  60f*new Vector3(facteurSens * forwardNormal.x, 0, facteurSens * forwardNormal.z);
			}
			else if (w == "missile")
				arme.vitesseInitiale =  100f*forwardNormal;
			else if (w == "greenShield")
				shield = arme;
			else if (w == "Aku-Aku") {
				if (protection!=null){
					arme.lifeTime = 12f;
					Destroy(protection.gameObject);
				}
				protection = arme;
			}
		}

		weapons.RemoveAt (0);
		if (weapons.Count == 0) {
			state.Remove ("armed");
			GetKart().ws.guiTexture.texture = null;
		}
	}

	public void Die(GameObject killer, string weapon)
	{
		if (shield != null)
		{
				Destroy (shield.gameObject);
				return;
		}
		if (protection != null)
				return;
		// si on est pas invincible : on meurt
		if (state.IndexOf ("invincible") == -1)
		{
				StartCoroutine (Transparence ());
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
	
	IEnumerator UnableToShoot()
	{
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
		int nbApplesFinal = kart.nbApples;
		int n = Random.Range (4, 8);
		int nb = kart.nbApples + n;
		if( nb == 10 ) nbApplesFinal=10;
		else if( nb > 10 ) nbApplesFinal=10;
		else nbApplesFinal+=n;
		StartCoroutine(animAddApples(nbApplesFinal));
	}

	public void rmApples(int n)
	{
		int nbApplesFinal = kart.nbApples;
		if (n > kart.nbApples) nbApplesFinal = 0;
		else nbApplesFinal -= n;
		StartCoroutine(animRmApples(nbApplesFinal));
	}

	IEnumerator animAddApples(int nbToGet)
	{
		while(kart.nbApples!=nbToGet)
		{
			kart.nbApples+=1;
			kart.pommeText.text ="x " + kart.nbApples.ToString();
			GameObject soundGetApple = GameObject.Instantiate (Resources.Load ("getApple")) as GameObject;
			yield return new WaitForSeconds (0.27f);
			Destroy(soundGetApple);
		}
	}

	IEnumerator animRmApples(int nbToGet)
	{
		while(kart.nbApples!=nbToGet)
		{
			kart.nbApples-=1;
			kart.pommeText.text ="x " + kart.nbApples.ToString();
			yield return new WaitForSeconds (0.27f);
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
			coeffManiabilite=2;
			coeffVitesse=3;
		}
		if(Input.GetKeyDown(keyMap["jump"]))
		{
			pressR1=true;
		}
		if(Input.GetKeyUp(keyMap["jump"]))
		{
			pressR1=false;
			pressXAndFlecheAndR1=false;
			coeffManiabilite=2;
			coeffVitesse=3;
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
			coeffManiabilite=4;
			coeffVitesse=3;
		}
		
		if(!pressXAndFlecheAndR1 || !pressL1)
		{
		}
	}
	
	public void controlPosition()
	{
		Vector3 postForce = new Vector3 ();
		rigidbody.position = transform.position;
		Vector3 forwardNormal = transform.forward;
		forwardNormal.y = 0;
		forwardNormal = normalizeVector (forwardNormal);

		if(Input.GetKey(keyMap["moveBack"])) {
			if (hasAxis)
				transform.Rotate(0,Input.GetAxis(axisMap["turn"])*coeffManiabilite,0);
				//postForce = new Vector3 ();
			else {
				postForce-=forwardNormal*coeffVitesse;
				if(Input.GetKey(keyMap["turnLeft"]))
					transform.Rotate(0,-0.5f*coeffManiabilite,0);
				if(Input.GetKey(keyMap["turnRight"]))
					transform.Rotate(0,0.5f*coeffManiabilite,0);
			}
		}

		if(Input.GetKey(keyMap["moveForward"]))
		{
			postForce+=forwardNormal*coeffVitesse;
			if (hasAxis)
				transform.Rotate(0,Input.GetAxis(axisMap["turn"])*coeffManiabilite,0);
			else
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
				rigidbody.position += new Vector3(0,2f,0);
			}
		}
		if (hasAxis && Input.GetAxis (axisMap ["stop"]) > 0) {
			postForce -= Input.GetAxis (axisMap ["stop"]) * forwardNormal * coeffVitesse;
			transform.Rotate (0, -Input.GetAxis (axisMap ["turn"]) * coeffManiabilite, 0);
		}
		
		if (Input.GetKeyDown (keyMap ["action"])) {
			if (state.IndexOf ("UnableToShoot") != -1)
			    return;
			if (hasAxis && Input.GetAxis (axisMap ["stop"]) < 0)
				facteurSens = 1f;
			else if (Input.GetKey(keyMap["moveBack"]))
				facteurSens = -1f;
			else if (!hasAxis)
				facteurSens = 1f;
			else
				facteurSens = -1f;

			if (!explosiveWeapon)
					UseWeapon ();
			else {
					explosiveWeapon = false;
					arme.ActionExplosion ();
			}
		}

		rigidbody.position += postForce;

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
			{"action",KeyCode.U}, {"start",KeyCode.Escape}, 
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
