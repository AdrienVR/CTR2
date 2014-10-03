using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class KartController : MonoBehaviour
{
	public static Dictionary <int, Dictionary<string, KeyCode>> playersMapping;
	public static List<bool> controllerEnabled;

	public bool j1enabled=false;
	public bool j2enabled=false;
	public float coeffVitesse=2f;
	public float coeffManiabilite=4f;

	private bool pressX=false;
	private bool pressFleche=false;
	private bool pressR1=false;
	private bool pressL1=false;
	private bool pressXAndFleche = false;
	private bool pressXAndFlecheAndR1 = false;

	private Vector3 velocityToApplyByJonathan;

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
		if (controllerEnabled == null)
		{
			controllerEnabled = new List<bool>();
			controllerEnabled.Add(j1enabled);
			controllerEnabled.Add(j2enabled);
			controllerEnabled.Add(true);
			controllerEnabled.Add(true);
		}
		if (playersMapping == null)
			InitMapping ();
		InitSelfMapping ();
	}
	
	void Update()
	{
		//limiter les rotations a 60 degrés
		float limitz1 = 0f;
		float limitz2 = -limitz1;
		/*if(transform.localEulerAngles.x > limit)
			transform.localEulerAngles = new Vector3(limit, transform.localEulerAngles.y, transform.localEulerAngles.z);
		else if(transform.localEulerAngles.x < -limit)
			transform.localEulerAngles = new Vector3(-limit, transform.localEulerAngles.y, transform.localEulerAngles.z);*/
		if(rigidbody.transform.localEulerAngles.z > limitz1)
			rigidbody.transform.localEulerAngles = new Vector3(rigidbody.transform.localEulerAngles.x, rigidbody.transform.localEulerAngles.y, limitz1);
		else if(rigidbody.transform.localEulerAngles.z < limitz2)
			rigidbody.transform.localEulerAngles = new Vector3(rigidbody.transform.localEulerAngles.x, rigidbody.transform.localEulerAngles.y, limitz2);


		if (dansLesAirs)
			rigidbody.velocity = new Vector3(rigidbody.velocity.x,-9.81f,rigidbody.velocity.z);
		/*
		else
			rigidbody.velocity = new Vector3(rigidbody.velocity.x,rigidbody.velocity.y/2,rigidbody.velocity.z);
			*/

		// INDISPENSABLE : annule la possibilité de CONTROLER la rotation z
		rigidbody.angularVelocity = Vector3.zero;

	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (keyMap == null)
			return;
		if (keyMap ["moveForward"] == KeyCode.Joystick3Button2 && !controllerEnabled [2])
			return;
		if (keyMap ["moveForward"] == KeyCode.Joystick4Button2 && !controllerEnabled [3])
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
	}

	public void UseWeapon()
	{
		if (weapons.Count == 0)
			return;
		string w = weapons [0];
		Vector3 posToAdd ;
		if (w == "bomb")
			posToAdd = 6f * (new Vector3 (facteurSens * transform.forward.x, transform.forward.y - 0.6f, facteurSens * transform.forward.z));
		else if (poseWeapons.IndexOf(w) != -1)
			posToAdd = 3f * (new Vector3 (-1 * transform.forward.x, transform.forward.y - 0.6f, -1 * transform.forward.z));
		else
			posToAdd = 6f * (new Vector3 (transform.forward.x, transform.forward.y - 0.6f, transform.forward.z));
		
		GameObject arme1 = Instantiate(Resources.Load("weapons/"+w), transform.position-posToAdd, transform.rotation) as GameObject;
		arme = (ExplosionScript) arme1.GetComponent ("ExplosionScript");
		if (arme!=null)	{
			arme.owner = rigidbody.gameObject;

			if (w == "bomb") {
				explosiveWeapon = true;
				arme.vitesseInitiale =  2f*rigidbody.velocity;
			}
			else if (w == "missile")
				arme.vitesseInitiale =  3.5f*rigidbody.velocity;
		}

		weapons.RemoveAt (0);
		if (weapons.Count == 0) {
			state.Remove ("armed");
			GetKart().ws.guiTexture.texture = null;
		}
	}

	public void Die()
	{

		if (state.IndexOf ("invincible") == -1) {
						state.Add ("invincible");
						StartCoroutine (Transparence ());
		}
		if (state.IndexOf ("DieAnimation") == -1) {
			state.Add ("DieAnimation");
			StartCoroutine (DieAnimation ());
		}
	}
	
	IEnumerator DieAnimation()
	{
		renderer.enabled = false;
		float time = 0f;
		while (time < 2.5f) {
			yield return new WaitForSeconds (0.1f);
			time += 0.1f;
		}
		state.Remove ("DieAnimation");
		
	}
	
	IEnumerator Transparence()
	{
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
	
	public void controlPosition()
	{
		Vector3 postForce = new Vector3 ();
		Vector3 forwardNormal = rigidbody.transform.forward;
		forwardNormal.y = 0;
		forwardNormal = normalizeVector (forwardNormal);
		if(Input.GetKey(keyMap["moveBack"]))
		{
			if (!controllerEnabled[kart.numeroJoueur-1])
			{
				postForce+=forwardNormal/8*coeffVitesse;
				//rigidbody.position+=forwardNormal/200*coeffVitesse;
			}
			if (controllerEnabled[kart.numeroJoueur-1])
				transform.Rotate(0,Input.GetAxis(axisMap["turn"])*coeffManiabilite,0);
			else
			{
				if(Input.GetKey(keyMap["turnLeft"]))
					transform.Rotate(0,-0.5f*coeffManiabilite,0);
				if(Input.GetKey(keyMap["turnRight"]))
					transform.Rotate(0,0.5f*coeffManiabilite,0);
			}
		}
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

		if(Input.GetKey(keyMap["moveForward"]))
		{
			postForce-=forwardNormal*coeffVitesse;
			rigidbody.position-=forwardNormal/4*coeffVitesse;
			if (controllerEnabled[kart.numeroJoueur-1])
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
				rigidbody.AddForce(0,600000,0);
			}
		}
		if (controllerEnabled [kart.numeroJoueur - 1] && Input.GetAxis (axisMap ["stop"]) > 0) {
			postForce += Input.GetAxis (axisMap ["stop"]) * forwardNormal / 4 * coeffVitesse;
			rigidbody.position+=Input.GetAxis (axisMap ["stop"]) * forwardNormal/4*coeffVitesse;
			transform.Rotate (0, -Input.GetAxis (axisMap ["turn"]) * coeffManiabilite, 0);
		}
		
		if (Input.GetKeyDown (keyMap ["action"])) {
			if (state.IndexOf ("DieAnimation") != -1)
			    return;
			if (controllerEnabled [kart.numeroJoueur - 1] && Input.GetAxis (axisMap ["stop"]) < 0)
				facteurSens = 1f;
			else if (Input.GetKey(keyMap["moveBack"]))
				facteurSens = -1f;
			else if (!controllerEnabled [kart.numeroJoueur - 1])
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

		
		if (controllerEnabled [kart.numeroJoueur - 1]) {
						if (Input.GetAxis (axisMap ["turn"]) == -1 || Input.GetAxis (axisMap ["turn"]) == 1) {
								pressFleche = true;
						}
						if (Input.GetAxis (axisMap ["turn"]) != -1 && Input.GetAxis (axisMap ["turn"]) != 1) {
								pressFleche = false;
			
								coeffManiabilite = 2;
								coeffVitesse = 3;
						}
				}

		float max = 3f;
		Vector3 veloce = rigidbody.velocity;
		veloce += postForce;
		if (veloce.x > max)
			veloce.x = max;
		else if (veloce.x < -max)
			veloce.x = -max;
		if (veloce.z > max)
			veloce.z = max;
		else if (veloce.z < -max)
			veloce.z = -max;
		rigidbody.velocity = veloce;

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
		

		if (controllerEnabled [kart.numeroJoueur-1])
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
		
		if (controllerEnabled [0])
			pc1 = ps1;
		if (controllerEnabled [1])
			pc2 = ps2;

		playersMapping = new Dictionary<int, Dictionary<string, KeyCode>> {{1,pc1},{2,pc2},{3,ps3},{4,ps4}};
	}
	
}
