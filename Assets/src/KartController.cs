using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using System;


public class KartController : MonoBehaviour
{
	// static 

	public static bool IA_enabled = false;
	public static bool stop = true;

	// ---------- propriete du kart --------------

	private bool stopDie = false;
	private float speedCoeff;
	private float turnCoeff;
	private float coeffInitSpeed;
	private Dictionary <string, float> speedDuration = new Dictionary <string,  float>();
	private Dictionary <string, float> speedToAdd = new Dictionary <string,  float>();
	private Vector3 postForce;
	private Vector3 lowForce;

	public Vector3 forwardNormal;

	// ------------ pointeurs d'objets ------------

	private Kart kart;
	private ControllerAPI controller;
	private List<GameObject> smoke = new List<GameObject>();
	private WeaponBoxScript takenWeaponBox;
	private ExplosionScript arme;
	private Bomb bomb;

	public bool explosiveWeapon = false;
	public Dictionary <string, Transform> wheels = new Dictionary <string, Transform>();
	public ExplosionScript shield;
	public ExplosionScript protection;
	public GameObject tnt;
	public List<string> weapons;

	// ----------------- etat --------------------

	public List<string> state = new List<string>();
	private bool isInAir = true;
	private bool isOnWall = false;
	private bool baddie = false;
	private bool forward = false;
	private bool backward = false;
	
	private int numberOfJump = 0;

	private float yTurn;
	private float facteurSens = 1f;
	private float ellapsedTime = 0f;
	private float currentTime = 0f;
	private float accelerationTime = 0f;
	private float twTime = 0f;
	private float twLerp = 0f;
	private float twTimeWheels = 0f;
	private float twLerpWheels = 0f;
	
	// Use this for initialization
	void Start ()
	{
		coeffInitSpeed = speedCoeff;
		weapons = new List<string>();

		foreach (Transform child in transform){
			if (child.name == "kartSmoke"){
				smoke.Add(child.gameObject);
				continue;
			}
			if (child.name != "steering")continue;
			wheels["steering"] = child;
			foreach (Transform w in child.transform)
				wheels[w.name] = w;
		}
		controller = new ControllerAPI (kart.numeroJoueur);
	}
	
	void Update()
	{
		if (IA_enabled)
			return;
		yTurn = 0;
		if (Time.timeScale == 0)
			return;
		if (!stop && !stopDie){
			forwardNormal = wheels ["steering"].transform.forward;
			forwardNormal.y = 0;
			forwardNormal = normalizeVector (forwardNormal);
			
			if (IA_enabled){}
			//controlIA();
			else{
				controle ();
				/*
				if (hasAxis)
					controlPosition ();
				else
					controlKeyboard ();*/
			}
		}
		if (!IA_enabled)
			controlCamera ();
	}
	
	void FixedUpdate()
	{
		if(IA_enabled)
		{
			//rigidbody.velocity = new Vector3(postForce.x, rigidbody.velocity.y, postForce.z);
			if (isInAir)
				rigidbody.velocity = new Vector3(rigidbody.velocity.x,-26f,rigidbody.velocity.z);
		}
		else{
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
			}

			if (!forward){
				if (System.Math.Abs(accelerationTime)<0.01f)
					accelerationTime = 0f;
				if (accelerationTime>0)
					accelerationTime -= ellapsedTime;
				else if (backward && accelerationTime>-1){
					backward = false;
					accelerationTime -= ellapsedTime;
				}
				else if (accelerationTime<0)
					accelerationTime += ellapsedTime;
			}
			else{
				forward = false;
				if (accelerationTime<1)
					accelerationTime += ellapsedTime;
				//rigidbody.velocity = new Vector3(postForce.x, 
				//                                 rigidbody.velocity.y, postForce.z);

			}
			if (accelerationTime>0)
				rigidbody.velocity = Vector3.Slerp(new Vector3(),postForce,accelerationTime);
			else
				rigidbody.velocity = Vector3.Slerp(new Vector3(),lowForce,-accelerationTime);

			if (isInAir)
				rigidbody.velocity = new Vector3(rigidbody.velocity.x,-26f,rigidbody.velocity.z);
			transform.Rotate (0, yTurn, 0);

			controlWheels ();
		}
	}

	float Slerp(float a, float b, float f)
	{
		return a + f*f * (b - a);
	}
	
	float Lerp(float a, float b, float f)
	{
		return a + f * (b - a);
	}

	void controlWheels(){
		float yTurnWheel = 0f;
		if(controller.IsPressed("turnLeft"))
			yTurnWheel = -controller.KeyValue("turnLeft");
		else if(controller.IsPressed("turnRight"))
			yTurnWheel = controller.KeyValue("turnRight");
		if (System.Math.Abs (yTurnWheel) < Game.thresholdAxis)
			yTurnWheel = 0;

		// WHEELS
		if (yTurnWheel==0){
			if (System.Math.Abs (twTimeWheels) < 0.1f)
				twTimeWheels = 0;
			if (twTimeWheels>0)
				twTimeWheels -= ellapsedTime;
			else if (twTimeWheels<0)
				twTimeWheels += ellapsedTime;
		}
		else{
			twTimeWheels += yTurnWheel*ellapsedTime;
		}
		
		twTimeWheels = System.Math.Max (twTimeWheels,-0.25f);
		twTimeWheels = System.Math.Min (twTimeWheels,0.25f);

		// STEERING WHEEL
		if (yTurnWheel==0 || System.Math.Abs(rigidbody.velocity.magnitude) < 1f){
			if (System.Math.Abs (twTime) < 0.01f)
				twTime = 0;
			if (twTime>0)
				twTime -= ellapsedTime;
			else if (twTime<0)
				twTime += ellapsedTime;
		}
		else{
			twTime += yTurnWheel*ellapsedTime;
		}

		twTime = System.Math.Max (twTime,-0.25f);
		twTime = System.Math.Min (twTime,0.25f);


		twLerpWheels = Lerp (0, 160f, twTimeWheels);
		twLerp = Lerp (0, 45f, twTime);

		wheels ["steering"].localRotation = Quaternion.Euler (new Vector3 (0, twLerp));
		wheels ["wheelAL"].localRotation = Quaternion.Euler (new Vector3 (0, 90f + twLerpWheels));
		wheels ["wheelAR"].localRotation = Quaternion.Euler (new Vector3 (0, 90f + twLerpWheels));
	}

	void OnCollisionStay(Collision collision)
	{
		if(collision.gameObject.name=="Ground")
			isInAir = false;
	}
	
	void OnCollisionExit(Collision other)
	{
		if(other.gameObject.name=="Ground")
			isInAir = true;
	}
	
	void OnTriggerStay(Collider other)
	{
		if(other.name=="Ground_trigger")
			isOnWall = true;
	}

	void OnTriggerExit(Collider other)
	{
		if(other.name=="Ground_trigger")
			isOnWall = false;
	}

	public void setCoefficients(float speed, float turn){
		speedCoeff = speed;
		turnCoeff = turn;
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

		if (tnt)
			return;

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
		if (controller.IsPressed("throw"))
			sens = controller.KeyValue("throw");
		if (System.Math.Abs(sens)<Game.thresholdAxis)
			sens = -1f;
		// computing the distance to instantiate the weapon
		if (w == "bomb")
			posToAdd = 6f * (new Vector3 (facteurSens * forwardNormal.x, forwardNormal.y + 0.2f, facteurSens * forwardNormal.z));
		else if (Game.poseWeapons.IndexOf(w) != -1){
			if (w == "greenBeaker" || w=="redBeaker")
				posToAdd = 4f * (new Vector3 (sens*forwardNormal.x, forwardNormal.y + 0.2f, sens*forwardNormal.z));
			else
				posToAdd = 4f * (new Vector3 (-forwardNormal.x, forwardNormal.y + 0.2f, -forwardNormal.z));
		}
		else
			posToAdd = 6f * (new Vector3 (forwardNormal.x, forwardNormal.y + 0.2f, forwardNormal.z));
		Quaternion q = Quaternion.Euler (new Vector3(0,transform.rotation.eulerAngles.y,0));
		if (Game.poseWeapons.IndexOf(w) != -1)
			q = transform.rotation;

		GameObject arme1;
		//instantiate the weapon
		if (Game.instatiableWeapons.IndexOf(w)!=-1){
			arme1 = Instantiate(Resources.Load("Weapons/"+w), transform.position + posToAdd, q) as GameObject;
			arme1.name = arme1.name.Split ('(') [0];

			//compute the velocity
			if (w!="bomb")
				arme = arme1.GetComponent <ExplosionScript>();
			else
				bomb = arme1.GetComponent <Bomb>();
		}
		if ((arme!=null||bomb!=null) && Game.instatiableWeapons.IndexOf(w)!=-1)	{
			
			if (w == "bomb") {
				explosiveWeapon = true;
				bomb.owner = gameObject;
				bomb.vitesseInitiale =  3*speedCoeff*new Vector3(facteurSens * forwardNormal.x, 0, facteurSens * forwardNormal.z);
			}
			else if (w == "missile"){
				arme.owner = gameObject;
				if (IsSuper())
					arme.vitesseInitiale =  4*speedCoeff*forwardNormal;
				else
					arme.vitesseInitiale =  2.75f*speedCoeff*forwardNormal;
			}
			else if (w == "greenShield"|| w == "blueShield"){
				arme.owner = gameObject;
				shield = arme;
				shield.lifeTime = 14f;
			}
			else if (w == "Aku-Aku" || w == "Uka-Uka") {
				arme.owner = gameObject;
				/*Main.sourceMusic.clip=(AudioClip)Instantiate(Resources.Load("Audio/akuaku"));
				Main.sourceMusic.Play();*/
				if (protection!=null)
					Destroy(arme.gameObject);
				else{
					protection = arme;
				}
				if (IsSuper())
					protection.lifeTime = 10f;
				else
					protection.lifeTime = 7f;
				AddSpeed(protection.lifeTime+2, 1.5f, "aku");
				Main.ManageSound ();
			}
			else if (w == "greenBeaker" || w=="redBeaker"){
				arme.owner = gameObject;
				if (sens == 1f)
					arme.rigidbody.AddForce(2000f*new Vector3(sens * forwardNormal.x, 0.2f, sens * forwardNormal.z));
					//arme.vitesseInstant =  90f*new Vector3(sens * forwardNormal.x, 0, sens * forwardNormal.z);
			}
			else
				arme.owner = gameObject;
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
		if(tnt && weapon != "tntExploded")
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
			StartCoroutine (JumpDie ());
			StartCoroutine (TurnDie ());
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
		bool fire = false;

		foreach(string key in copy)
		{
			if (speedDuration [key] > 0f){
				if (key == "turbo")
					fire = true;
				speedDuration [key] -= ellapsedTime;
				speedCoeff *= speedToAdd[key];
			}
		}
		if (fire){
			foreach(GameObject w in smoke)
				w.GetComponent<ParticleRenderer>().material.SetColor ("_TintColor", Color.red);
		}
		else{
			float rgb = 0.1f;
			if (rigidbody.velocity.magnitude>1)
				rgb = 0.05f;
			else if (controller.IsPressed("moveForward"))
				rgb = 0.5f;
			Color smokeColor = new Color(rgb,rgb,rgb);
			foreach(GameObject w in smoke)
				w.GetComponent<ParticleRenderer>().material.SetColor ("_TintColor", smokeColor);
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
	
	IEnumerator JumpDie()
	{
		float time = 0;
		float high = 5f;
		while (time < 2.5f) {
			rigidbody.MovePosition(rigidbody.position + new Vector3(0,high));
			yield return new WaitForSeconds (0.25f);
			time += 0.25f;
			high -= 0.25f;
		}
	}
	
	IEnumerator TurnDie()
	{
		float time = 0;
		float x = 0, y = 0, z = 0;
		while (time < 2f) {
			yield return new WaitForSeconds (0.01f);
			wheels["steering"].transform.rotation = Quaternion.Euler(new Vector3(x,y,z));
			time += 0.01f;
			x = 360 * time ;
			y = x;
			//z = x;
		}
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
		foreach(GameObject w in smoke){
			w.SetActive(false);
		}
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
				/*foreach(GameObject w in smoke){
					w.SetActive(!w.activeSelf);
				}*/
			}
		}
		foreach(string w in wheels.Keys)
		{
			wheels [w].renderer.enabled = true;
		}
		foreach(GameObject w in smoke){
			w.SetActive(true);
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

	
	public void controle()
	{	
		if(controller.IsPressed("moveBack") && !controller.IsPressed("moveForward")){
			lowForce = -controller.KeyValue("moveBack") * forwardNormal * speedCoeff;
			backward = true;
		}
		
		if(controller.IsPressed("moveForward") && !controller.IsPressed("moveBack"))
		{
			postForce = forwardNormal*speedCoeff;
			forward = true;
		}
		
		if(controller.GetKeyDown("jump"))
		{
			if(!isInAir && !isOnWall)
			{
				if (rigidbody.velocity.magnitude < 5){
					rigidbody.MovePosition(rigidbody.position + new Vector3(0,1.75f,0));
				}
				else {
					float high = transform.localRotation.eulerAngles.x;
					if (high > 180)
						high = System.Math.Abs(high-360);
					else 
						high = 0;
					//Debug.Log("Angle : "+high);
					high = System.Math.Min(high, 9f);
					high = System.Math.Max(high, 1.75f);
					rigidbody.MovePosition(rigidbody.position + new Vector3(0,high,0));
				}
				
				if (tnt)
					numberOfJump++;
			}
		}
		if(controller.IsPressed("jump") )//&& System.Math.Abs(Vector3.Angle(rigidbody.velocity,forwardNormal))>45)
		{
			//Debug.Log("Angle : "+transform.localRotation.eulerAngles.x+","+transform.localRotation.eulerAngles.z);
			//rigidbody.velocity = new Vector3(0,-26f,0);
		}

		if (!controller.IsPressed("moveBack") && (controller.IsPressed("stop") || controller.IsPressed("moveForward"))){
			if(controller.IsPressed("turnRight"))
				yTurn = 0.5f*controller.KeyValue("turnRight") * turnCoeff;
			else if(controller.IsPressed("turnLeft"))
				yTurn = -0.5f*controller.KeyValue("turnLeft") * turnCoeff;
		}
		else if (controller.IsPressed("moveBack")){
			if(controller.IsPressed("turnRight"))
				yTurn = -0.5f*controller.KeyValue("turnRight") * turnCoeff;
			else if(controller.IsPressed("turnLeft"))
				yTurn = 0.5f*controller.KeyValue("turnLeft") * turnCoeff;
		}
		
		if (controller.GetKeyDown("action")) {
			if (state.IndexOf ("UnableToShoot") != -1)
				return;
			facteurSens = 1f;
			if (controller.IsPressed("moveBack"))
				facteurSens = -controller.KeyValue("throw");
			if (System.Math.Abs(facteurSens)<Game.thresholdAxis)
				facteurSens = 1f;
			
			if (!explosiveWeapon){
				UseWeapon ();
			}
			else {
				explosiveWeapon = false;
				if (kart.nbApples == 10)
					bomb.explosionRadius = 8f;
				bomb.ActionExplosion ();
			}
		}
	}
	
	public void controlCamera()
	{
		if (controller.GetKeyDown ("viewInverse")) {
			kart.cm1c.reversed = -1f ;
		}
		if (controller.GetKeyUp ("viewInverse")) {
			kart.cm1c.reversed = 1f ;
		}
		if (controller.GetKeyDown ("viewChange")) {
			if (kart.cm1c.positionForward == 1f)
				kart.cm1c.positionForward = 0.85f ;
			else
				kart.cm1c.positionForward = 1f ;
		}
	}
}
