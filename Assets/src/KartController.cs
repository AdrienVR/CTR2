using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using System;


public class KartController : MonoBehaviour
{
	public static bool stop = true;
	private bool stopDie = false;
	private bool isGoingInAir = false;
	
	private float speedCoeff;
	private float turnCoeff;
	public float coeffInitSpeed;
	public Dictionary <string, float> speedDuration = new Dictionary <string,  float>();
	public Dictionary <string, float> speedToAdd = new Dictionary <string,  float>();
	
	private bool hasAxis = false;
	private Vector3 postForce;
	private Vector3 lowForce;
	private Vector3 forwardNormal;
	
	private bool pressX=false;
	private bool pressFleche=false;
	private bool pressR1=false;
	private bool pressL1=false;
	private bool pressXAndFleche = false;
	private bool pressXAndFlecheAndR1 = false;

	private WeaponBoxScript takenWeaponBox;
	private ExplosionScript shield;
	public ExplosionScript protection;
	
	private List<string> state = new List<string>();
	public List<string> weapons;
	public Dictionary <string, KeyCode> keyMap;

	private Kart kart;
	private bool dansLesAirs = true;
	private Dictionary <string, string> axisMap;
	public Dictionary <string, Transform> wheels = new Dictionary <string, Transform>();
	private float ky;
	private bool baddie = false;
	
	private ExplosionScript arme;
	public bool explosiveWeapon;
	private float facteurSens = 1f;
	private static float ellapsedTime = 0f;
	private static float currentTime = 0f;

	private float yTurn;
	private float yTurnWheel;

	public int weaponSize = 1;
	private static int nControllers;
	
	// Use this for initialization
	void Start ()
	{
		coeffInitSpeed = speedCoeff;
		explosiveWeapon = false;
		weapons = new List<string>();
		InitSelfMapping ();
	}
	
	void FixedUpdate()
	{
		if (Input.GetJoystickNames ().Length != nControllers)
			InitSelfMapping ();

		ellapsedTime = Time.time - currentTime;
		currentTime = Time.time;
		CheckSpeed ();

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

		transform.Rotate (0, yTurn, 0);
		wheels ["wheelAL"].rotation = Quaternion.Euler (transform.eulerAngles + new Vector3 (0, 90f + yTurnWheel * 13f));
		wheels ["wheelAR"].rotation = Quaternion.Euler (transform.eulerAngles + new Vector3 (0, 90f + yTurnWheel * 13f));
		foreach(string w in wheels.Keys)
		{
			wheels [w].rotation = Quaternion.Euler (wheels [w].eulerAngles + new Vector3 (0, 0, 0));
		}
	}

	
	void Update()
	{
		yTurn = 0;
		yTurnWheel = 0;
		if (Time.timeScale == 0)
			return;
		lowForce = new Vector3 ();
		postForce = new Vector3 ();
		if (!stop && !stopDie){
			forwardNormal = transform.forward;
			forwardNormal.y = 0;
			forwardNormal = normalizeVector (forwardNormal);
			if (hasAxis)
				controlPosition ();
			else
				controlKeyboard ();
		}
		controlCamera ();
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

	public void setCoefficients(float speed, float turn){
		speedCoeff = speed;
		turnCoeff = turn;
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
		return kart.nbApples == 10;
	}
	
	public void addApples()
	{
		kart.addApples ();
	}

	public void animApples()
	{
		StartCoroutine (animApplesNb());
	}
	
	IEnumerator animApplesNb()
	{
		while(kart.nbApplesFinal != kart.nbApples)
		{
			kart.nbApples ++;
			kart.SetIllumination((kart.nbApples == 10));
			audio.Play();
			kart.guitextApples.text = "x "+kart.nbApples.ToString();
			kart.drawWeaponGui();
			yield return new WaitForSeconds (0.27f);
		}
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
		Vector3 posToAdd = new Vector3();
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
		if (IsSuper() && !Dictionnaries.superWeapons.ContainsValue(w) && w!="missile")
		{
			int n = 0;
			for(int k=1;k<Dictionnaries.normalWeapons.Count+1;k++)
				if (Dictionnaries.normalWeapons[k] == w)
					n = k;
			w = Dictionnaries.superWeapons[n];
			Debug.Log("ddd "+w);
		}
		float sens = -1f;
		if (hasAxis && Input.GetAxis (axisMap ["stop"]) < -0.1f)
			sens = 1f;

		// computing the distance to instantiate the weapon
		if (w == "bomb")
			posToAdd = 6f * (new Vector3 (facteurSens * forwardNormal.x, forwardNormal.y + 0.6f, facteurSens * forwardNormal.z));
		else if (Dictionnaries.poseWeapons.IndexOf(w) != -1){
			if (w == "greenBeaker" || w=="redBeaker")
				posToAdd = 4f * (new Vector3 (sens*forwardNormal.x, forwardNormal.y + 0.2f, sens*forwardNormal.z));
			else
				posToAdd = 4f * (new Vector3 (-forwardNormal.x, forwardNormal.y + 0.2f, -forwardNormal.z));
		}
		else
			posToAdd = 6f * (new Vector3 (forwardNormal.x, forwardNormal.y + 0.2f, forwardNormal.z));
		Quaternion q = new Quaternion (0,transform.rotation.y,0,transform.rotation.w);
		if (Dictionnaries.poseWeapons.IndexOf(w) != -1)
			q = transform.rotation;

		if (Dictionnaries.instatiableWeapons.IndexOf(w)!=-1){
			//instantiate the weapon
			GameObject arme1 = Instantiate(Resources.Load("Weapons/"+w), transform.position + posToAdd, q) as GameObject;
			arme1.name = arme1.name.Split ('(') [0];

			//compute the velocity
			arme = (ExplosionScript) arme1.GetComponent ("ExplosionScript");
		}
		if (arme!=null)	{
			arme.owner = gameObject;
			
			if (w == "bomb") {
				explosiveWeapon = true;
				arme.vitesseInitiale =  3*speedCoeff*new Vector3(facteurSens * forwardNormal.x, 0, facteurSens * forwardNormal.z);
				if (kart.nbApples == 10)
					arme.explosionRadius *= 2.5f;
			}
			else if (w == "missile"){
				if (IsSuper())
					arme.vitesseInitiale =  4*speedCoeff*forwardNormal;
				else
					arme.vitesseInitiale =  2.75f*speedCoeff*forwardNormal;
			}
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
				AddSpeed(protection.lifeTime, 1.5f, "aku");
			}
			else if (w == "greenBeaker" || w=="redBeaker")
				if (sens == 1f)
					arme.rigidbody.AddForce(2000f*new Vector3(sens * forwardNormal.x, 0.2f, sens * forwardNormal.z));
					//arme.vitesseInstant =  90f*new Vector3(sens * forwardNormal.x, 0, sens * forwardNormal.z);
		}
		else if (w=="turbo"){
			if (IsSuper())
				AddSpeed(3.0f, 1.5f, w);
			else
				AddSpeed(2.0f, 1.5f, w);
		}

		//use the weapon so remove
		weapons.RemoveAt (0);
		if (weapons.Count == 0) {
			state.Remove ("armed");
			GetKart().undrawWeaponGui();
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
				kart.rmApples(1);
			}
			else kart.rmApples(3);
		}
	}

	
	void AddSpeed(float duration, float addSpeed, string weapon)
	{
		if (speedDuration.ContainsKey(weapon) == false)
			speedDuration [weapon] = 0f;
		speedDuration [weapon] += duration;
		speedToAdd [weapon] = addSpeed;
	}

	void CheckSpeed()
	{
		speedCoeff = coeffInitSpeed;

		string [] copy = new string[speedDuration.Keys.Count];
		speedDuration.Keys.CopyTo(copy, 0);

		foreach(string key in copy)
		{
			if (speedDuration [key] > 0f){
				speedDuration [key] -= ellapsedTime;
				speedCoeff *= speedToAdd[key];
			}
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
		foreach(string w in wheels.Keys)
		{
			wheels [w].renderer.enabled = false;
		}
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
				
				foreach(string w in wheels.Keys)
				{
					wheels [w].renderer.enabled = !wheels [w].renderer.enabled;
				}
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
			//turnCoeff=2;
			//speedCoeff=3;
		}
		if(Input.GetKeyDown(keyMap["jump"]))
		{
			pressR1=true;
		}
		if(Input.GetKeyUp(keyMap["jump"]))
		{
			pressR1=false;
			pressXAndFlecheAndR1=false;
			//turnCoeff=2;
			//speedCoeff=3;
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
			//turnCoeff=4;
			//speedCoeff=3;
		}
		
		if(!pressXAndFlecheAndR1 || !pressL1)
		{
		}
	}
	
	public void controlPosition()
	{	
		yTurnWheel = Input.GetAxis (axisMap ["turn"]) * turnCoeff;

		if(Input.GetAxis (axisMap ["stop"]) > 0.1f)
				lowForce = -Input.GetAxis (axisMap ["stop"]) * forwardNormal * speedCoeff;
		
		if(Input.GetKey(keyMap["moveForward"]))
		{
			postForce = forwardNormal*speedCoeff;
		}

		if(Input.GetKeyDown(keyMap["jump"]))
		{
			if(dansLesAirs==false)
			{
				rigidbody.position += new Vector3(0,3f,0);
			}
		}

		if(!Input.GetKey(keyMap["moveBack"]) && System.Math.Abs(Input.GetAxis (axisMap ["turn"]))>0.1f){
			if(Input.GetAxis (axisMap ["stop"]) > 0.1f){
				yTurn = -Input.GetAxis (axisMap ["turn"]) * turnCoeff;
			}
			else if(Input.GetKey(keyMap["moveForward"]) || Input.GetKeyDown(keyMap["jump"]))
				yTurn = Input.GetAxis (axisMap ["turn"]) * turnCoeff;
		}
		if(Input.GetKey(keyMap["moveBack"])){
				yTurn = Input.GetAxis (axisMap ["turn"]) * turnCoeff;
		}

		if (Input.GetKeyDown (keyMap ["action"])) {
			if (state.IndexOf ("UnableToShoot") != -1)
				return;
			facteurSens = 1f;
			if (hasAxis && Input.GetAxis (axisMap ["stop"]) > 0.1f)
				facteurSens = -1f;

			if (!explosiveWeapon)
				UseWeapon ();
			else {
				explosiveWeapon = false;
				arme.ActionExplosion ();
			}
		}
	}

	public void controlKeyboard(){
		
		if(Input.GetKey(keyMap["turnLeft"]))
			yTurnWheel = 0.5f*turnCoeff;
		else if(Input.GetKey(keyMap["turnRight"]))
			yTurnWheel = -0.5f*turnCoeff;

		if(Input.GetKey(keyMap["moveBack"])) {
				lowForce = -forwardNormal*speedCoeff;
				if(Input.GetKey(keyMap["turnLeft"]))
					yTurn = -0.5f*turnCoeff;
				else if(Input.GetKey(keyMap["turnRight"]))
					yTurn = 0.5f*turnCoeff;
		}
		
		if(Input.GetKey(keyMap["moveForward"]))
		{
			postForce = forwardNormal*speedCoeff;
			if(Input.GetKey(keyMap["turnLeft"]))
				yTurn = 0.5f*turnCoeff;
			if(Input.GetKey(keyMap["turnRight"]))
				yTurn = -0.5f*turnCoeff;
		}
		
		if(Input.GetKeyDown(keyMap["jump"]))
		{
			if(dansLesAirs==false)
			{
				rigidbody.position += new Vector3(0,3f,0);
			}
		}
		
		if (Input.GetKeyDown (keyMap ["action"])) {
			if (state.IndexOf ("UnableToShoot") != -1)
				return;
			facteurSens = 1f;
			if (Input.GetKey(keyMap["moveBack"]))
				facteurSens = -1f;
			
			if (!explosiveWeapon)
				UseWeapon ();
			else {
				explosiveWeapon = false;
				arme.ActionExplosion ();
			}
		}

	}
	
	public void controlCamera()
	{
		if (Input.GetKeyDown (keyMap ["viewInverse"])) {
			kart.cm1c.reversed = -1f ;
		}
		if (Input.GetKeyUp (keyMap ["viewInverse"])) {
			kart.cm1c.reversed = 1f ;
		}
		if (Input.GetKeyDown (keyMap ["viewChange"])) {
			if (kart.cm1c.positionForward == 1f)
				kart.cm1c.positionForward = 0.85f ;
			else
				kart.cm1c.positionForward = 1f ;
		}
	}
	
	void InitSelfMapping()
	{
		nControllers = Input.GetJoystickNames ().Length;
		hasAxis = Dictionnaries.controllersEnabled[kart.numeroJoueur];
		if (hasAxis)
			axisMap = Dictionnaries.axisMapping [kart.numeroJoueur];
		
		keyMap = Dictionnaries.playersMapping [kart.numeroJoueur];
	}
	
}
