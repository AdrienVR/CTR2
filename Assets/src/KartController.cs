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
	public Vector3 forwardNormal;
	
	private bool pressX=false;
	private bool pressFleche=false;
	private bool pressR1=false;
	private bool pressL1=false;
	private bool pressXAndFleche = false;
	private bool pressXAndFlecheAndR1 = false;

	private WeaponBoxScript takenWeaponBox;
	public ExplosionScript shield;
	public ExplosionScript protection;
	public GameObject tnt;

	public List<string> state = new List<string>();
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
	private float ellapsedTime = 0f;
	private float currentTime = 0f;

	private float yTurn;

	public int weaponSize = 1;
	private static int nControllers;

	private float accelerationTime = 0f;
	private float maxTime = 2.5f;
	private float lerpedSpeed = 0f;
	private float slerpedCoeffSpeed = 0f;
	
	private float twTime = 0f;
	private float twMaxTime = 10.5f;
	private float twLerp = 0f;

	private float twTimeWheels = 0f;
	private float twLerpWheels = 0f;

	private int numberOfJump = 0;
	
	// Use this for initialization
	void Start ()
	{
		coeffInitSpeed = speedCoeff;
		explosiveWeapon = false;
		weapons = new List<string>();
		InitSelfMapping ();

		foreach (Transform child in transform){
			wheels["steering"] = child;
			foreach (Transform w in child.transform)
				wheels[w.name] = w;
		}
	}
	
	void FixedUpdate()
	{
		if (Input.GetJoystickNames ().Length != nControllers)
			InitSelfMapping ();

		ellapsedTime = Time.time - currentTime;
		currentTime = Time.time;
		CheckSpeed();

		if (tnt && numberOfJump > 8) {
			tnt.transform.position = tnt.transform.position + new Vector3 (0, 5f);
			ExplosionScript e = tnt.GetComponent <ExplosionScript>();
			e.animation.Stop();
			e.disamorced = true;
			e.SetName("tntDropped");
			e.transform.parent = null;
			e.rigidbody.velocity = new Vector3();
			tnt = null;
			numberOfJump = 0;
			state.Remove("isGettingFuckedByATNT");
		}

		if (postForce.Equals(new Vector3())){
			accelerationTime -= ellapsedTime;
			checkAcc();
			//rigidbody.rotation = transform.rotation;
			//rigidbody.position = transform.position;
			slerpedCoeffSpeed = Slerp(slerpedCoeffSpeed, 0.9f, 1.0f);//lerpedSpeed);
			rigidbody.velocity = new Vector3(rigidbody.velocity.x*slerpedCoeffSpeed, 
			                                 rigidbody.velocity.y, rigidbody.velocity.z*slerpedCoeffSpeed);
			rigidbody.velocity = new Vector3(lowForce.x, rigidbody.velocity.y, lowForce.z);
		}
		else{
			accelerationTime += ellapsedTime;
			checkAcc();
			slerpedCoeffSpeed = Slerp(slerpedCoeffSpeed, 1.0f, lerpedSpeed);
			rigidbody.velocity = new Vector3(postForce.x*slerpedCoeffSpeed, rigidbody.velocity.y, postForce.z*slerpedCoeffSpeed);
		}

		if (dansLesAirs)
			rigidbody.velocity = new Vector3(rigidbody.velocity.x,-26f,rigidbody.velocity.z);

		transform.Rotate (0, yTurn, 0);

		controlWheels ();
	}

	float Slerp(float a, float b, float f)
	{
		return a + f*f * (b - a);
	}
	
	float Lerp(float a, float b, float f)
	{
		return a + f * (b - a);
	}

	void checkAcc(){
		accelerationTime = System.Math.Min (accelerationTime, maxTime);
		accelerationTime = System.Math.Max (accelerationTime, 0f);
		lerpedSpeed = accelerationTime/maxTime;
		lerpedSpeed = System.Math.Min (lerpedSpeed, 1f);
		lerpedSpeed = System.Math.Max (lerpedSpeed, 0f);
	}

	void controlWheels(){
		float yTurnWheel = 0f;
		if (hasAxis){
			yTurnWheel = Input.GetAxis (axisMap ["turn"]);
			if (yTurnWheel <0.1f && yTurnWheel>-0.1f)
				yTurnWheel = 0f;
		}
		else{
			if(Input.GetKey(keyMap["turnLeft"]))
				yTurnWheel = 1f;
			else if(Input.GetKey(keyMap["turnRight"]))
				yTurnWheel = -1f;
		}

		if (yTurnWheel != 0){
			twTimeWheels += ellapsedTime;
			if (!postForce.Equals(new Vector3()))
				twTime += ellapsedTime;
			else
				twTime -= ellapsedTime;
		}
		else{
			twTimeWheels -= ellapsedTime;
			twTime -= ellapsedTime;
		}

		twTimeWheels = System.Math.Min (twTimeWheels, 1.5f);
		twTimeWheels = System.Math.Max (twTimeWheels, 0f);
		twLerpWheels = Lerp (twLerpWheels, yTurnWheel, twTimeWheels/twMaxTime);

		twTime = System.Math.Min (twTime, 1.5f);
		twTime = System.Math.Max (twTime, 0f);
		twLerp = Lerp (twLerp, yTurn*System.Math.Min (rigidbody.velocity.magnitude, 27.5f)*0.25f, twTime/twMaxTime);

		wheels ["wheelAL"].rotation = Quaternion.Euler (transform.eulerAngles + new Vector3 (0, 90f + twLerpWheels * 40f));
		wheels ["wheelAR"].rotation = Quaternion.Euler (transform.eulerAngles + new Vector3 (0, 90f + twLerpWheels * 40f));
		wheels ["steering"].rotation = Quaternion.Euler (transform.eulerAngles + new Vector3 (0, twLerp));
	}

	
	void Update()
	{
		yTurn = 0;
		if (Time.timeScale == 0)
			return;
		lowForce = new Vector3 ();
		postForce = new Vector3 ();
		if (!stop && !stopDie){
			forwardNormal = wheels ["steering"].transform.forward;
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
		
		Vector3 forwardNormal = wheels ["steering"].transform.forward;
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
			shield.EnablePhysics();
			shield.transform.localScale = new Vector3(0.66f,0.75f,0.66f);
			shield = null;
			return;
		}
		
		if (weapons.Count == 0)
			return;
		string w = weapons [0];
		if (IsSuper() && !Game.superWeapons.ContainsValue(w) && w!="missile")
		{
			int n = 0;
			for(int k=1;k<Game.normalWeapons.Count+1;k++)
				if (Game.normalWeapons[k] == w)
					n = k;
			w = Game.superWeapons[n];
		}
		float sens = -1f;
		if (hasAxis && Input.GetAxis (axisMap ["stop"]) < -0.1f)
			sens = 1f;
		else if (!hasAxis && Input.GetKey(keyMap["moveForward"]))
			sens = 1f;

		// computing the distance to instantiate the weapon
		if (w == "bomb")
			posToAdd = 6f * (new Vector3 (facteurSens * forwardNormal.x, forwardNormal.y + 0.6f, facteurSens * forwardNormal.z));
		else if (Game.poseWeapons.IndexOf(w) != -1){
			if (w == "greenBeaker" || w=="redBeaker")
				posToAdd = 4f * (new Vector3 (sens*forwardNormal.x, forwardNormal.y + 0.2f, sens*forwardNormal.z));
			else
				posToAdd = 4f * (new Vector3 (-forwardNormal.x, forwardNormal.y + 0.2f, -forwardNormal.z));
		}
		else
			posToAdd = 6f * (new Vector3 (forwardNormal.x, forwardNormal.y + 0.2f, forwardNormal.z));
		Quaternion q = new Quaternion (0,transform.rotation.y,0,transform.rotation.w);
		if (Game.poseWeapons.IndexOf(w) != -1)
			q = transform.rotation;

		if (Game.instatiableWeapons.IndexOf(w)!=-1){
			//instantiate the weapon
			GameObject arme1 = Instantiate(Resources.Load("Weapons/"+w), transform.position + posToAdd, q) as GameObject;
			arme1.name = arme1.name.Split ('(') [0];

			//compute the velocity
			arme = (ExplosionScript) arme1.GetComponent ("ExplosionScript");
		}
		if (arme!=null && Game.instatiableWeapons.IndexOf(w)!=-1)	{
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
			else if (w == "Aku-Aku" || w == "Uka-Uka") {
				if (protection!=null)
					Destroy(arme.gameObject);
				else{
					protection = arme;
					if (IsSuper())
						protection.lifeTime = 10f;
					else
						protection.lifeTime = 7f;
				}
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
			kart.undrawWeaponGui();
		}
		else
			kart.drawWeaponGui ();
		if(state.IndexOf("armedEvolute")!=-1)
		{
			state.Remove ("armed");
			state.Remove ("armed");
			state.Remove ("armedEvolute");
		}
	}
	
	public void Die(GameObject killer, string weapon)
	{
		if(tnt)
			Destroy(tnt);
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
		while (time < 0.75f) {
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
		numberOfJump = 0;
		//clignotment, invincibilité temporaire
		if (state.IndexOf("invincible")==-1)
			state.Add ("invincible");
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
				foreach(string w in wheels.Keys)
				{
					wheels [w].renderer.enabled = !wheels [w].renderer.enabled;
				}
			}
		}
		foreach(string w in wheels.Keys)
		{
			wheels [w].renderer.enabled = true;
		}
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
				
				if (tnt)
					numberOfJump++;
			}
		}

		if(!Input.GetKey(keyMap["moveBack"]) && System.Math.Abs(Input.GetAxis (axisMap ["turn"]))>0.1f){
			if(Input.GetAxis (axisMap ["stop"]) > 0.1f){
				yTurn = -0.5f*Input.GetAxis (axisMap ["turn"]) * turnCoeff;
			}
			else if(Input.GetKey(keyMap["moveForward"]) || Input.GetKeyDown(keyMap["jump"]))
				yTurn = 0.5f*Input.GetAxis (axisMap ["turn"]) * turnCoeff;
		}
		if(Input.GetKey(keyMap["moveBack"])){
			yTurn = 0.5f*Input.GetAxis (axisMap ["turn"]) * turnCoeff;
		}

		if (Input.GetKeyDown (keyMap ["action"])) {
			if (state.IndexOf ("UnableToShoot") != -1)
				return;
			facteurSens = 1f;
			if (Input.GetAxis (axisMap ["stop"]) > 0.1f)
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
				
				if (tnt)
					numberOfJump++;
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
		hasAxis = Game.controllersEnabled[kart.numeroJoueur];
		if (hasAxis)
			axisMap = Game.axisMapping [kart.numeroJoueur];
		
		keyMap = Game.playersMapping [kart.numeroJoueur];
	}
	
}
