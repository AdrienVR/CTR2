using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KartScript : MonoBehaviour {
	
	public static bool stop = true;
	public bool stopDie = false;
	
	public Dictionary <string, Transform> wheels = new Dictionary <string, Transform>();

	private KartController kc;
	private Kart kart;
	private KartState kart_state;
	
	private ExplosionScript bomb;
	private WeaponBoxScript takenWeaponBox;
	public ExplosionScript shield;
	public ExplosionScript protection;
	public GameObject tnt;
	private List<GameObject> smoke = new List<GameObject>();

	public List<string> weapons = new List<string>();
	
	private bool baddie = false;
	private ControllerAPI controller;
	private float facteurSens = 1f;

	// Use this for initialization
	void Start () {
		kc = GetComponent<KartController>();
		kart_state = GetComponent<KartState>();
		controller = ControllerAPI.GetController(kart.numeroJoueur);

		
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

	}
	
	// Update is called once per frame
	void Update () {
		if (controller.GetKeyDown("action")) {
			if (!kart_state.AbleToShoot())
				return;
			facteurSens = 1f;
			if (controller.IsPressed("moveBack"))
				facteurSens = -controller.KeyValue("throw");
			if (System.Math.Abs(facteurSens)<Game.thresholdAxis)
				facteurSens = 1f;
			
			if (bomb == null)
				UseWeapon ();
			else {
				bomb.BombActionExplosion ();
				bomb = null;
      		}
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

	public void UseWeapon()
	{
		// stop the random searching of weapon from WeaponBox
		if (takenWeaponBox != null){
			if (takenWeaponBox.selectRandomWeapon ())
				takenWeaponBox = null;
			return;
		}

		// while wearing a tnt you can't use a weapon
		if (tnt)
			return;

		Vector3 posToAdd = Vector3.zero;
		//launch the shield
		if (shield != null) {
			shield.vitesseInitiale =  100f*kc.forwardNormal;
			shield.name = "bomb";
			posToAdd = transform.position + 6f * (new Vector3 (kc.forwardNormal.x, kc.forwardNormal.y+0.6f, kc.forwardNormal.z));
			shield.transform.position = posToAdd;
			shield.transform.rotation = new Quaternion();
			shield.EnablePhysics();
			shield.transform.localScale = new Vector3(0.66f,0.75f,0.66f);
			shield = null;
			return;
		}
		
		if (weapons.Count == 0)
			return;
		// apply
		string w = weapons [0];

		// superisation
		if (IsSuper())
		{
			w = Game.GetWeaponSuper(w);
		}

		// computing the side
		float sens = -1f;
		if (controller.IsPressed("throw"))
			sens = controller.KeyValue("throw");
		if (System.Math.Abs(sens)<Game.thresholdAxis)
			sens = -1f;

		// computing the distance to instantiate the weapon
		if (w == "bomb")
			posToAdd = 6f * (new Vector3 (facteurSens * kc.forwardNormal.x, kc.forwardNormal.y + 0.2f, facteurSens * kc.forwardNormal.z));
		else if (Game.poseWeapons.IndexOf(w) != -1){
			if (w == "greenBeaker" || w=="redBeaker")
				posToAdd = 4f * (new Vector3 (sens*kc.forwardNormal.x, kc.forwardNormal.y + 0.2f, sens*kc.forwardNormal.z));
			else
				posToAdd = 4f * (new Vector3 (-kc.forwardNormal.x, kc.forwardNormal.y + 0.2f, -kc.forwardNormal.z));
		}
		else
			posToAdd = 6f * (new Vector3 (kc.forwardNormal.x, kc.forwardNormal.y + 0.2f, kc.forwardNormal.z));

		// computing the angle
		Quaternion q = Quaternion.Euler (new Vector3(0,transform.rotation.eulerAngles.y,0));
		if (Game.poseWeapons.IndexOf(w) != -1)
			q = transform.rotation;
		
		//instantiate the weapon
		if (Game.instatiableWeapons.IndexOf(w)!=-1){
			GameObject arme1 = Instantiate(Resources.Load("Weapons/"+w), transform.position + posToAdd, q) as GameObject;
			arme1.name = arme1.name.Split ('(') [0];

			ExplosionScript arme = arme1.GetComponent <ExplosionScript>();
			if (arme!=null){
				arme.owner = gameObject;
				
				if (w == "bomb") {
					bomb = arme;
					arme.vitesseInitiale =  3*kc.speedCoeff*new Vector3(facteurSens * kc.forwardNormal.x, 0, facteurSens * kc.forwardNormal.z);
					if (kart.nbApples == 10)
						arme.explosionRadius *= 2.5f;
				}
				else if (w == "missile"){
					if (IsSuper())
						arme.vitesseInitiale =  4*kc.speedCoeff*kc.forwardNormal;
					else
						arme.vitesseInitiale =  2.75f*kc.speedCoeff*kc.forwardNormal;
				}
				else if (w == "greenShield"|| w == "blueShield"){
					shield = arme;
					shield.lifeTime = 14f;
				}
				else if (w == "Aku-Aku" || w == "Uka-Uka") {
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
					kc.AddSpeed(protection.lifeTime+2, 1.5f, "aku");
					Main.ManageSound ();
				}
				else if (w == "greenBeaker" || w=="redBeaker")
					if (sens == 1f)
						arme.rigidbody.AddForce(2000f*new Vector3(sens * kc.forwardNormal.x, 0.2f, sens * kc.forwardNormal.z));
				//arme.vitesseInstant =  90f*new Vector3(sens * forwardNormal.x, 0, sens * forwardNormal.z);
			}
			else
				Debug.Log("pb : +"+w);
		}
		else if (w=="turbo"){
			if (IsSuper())
				kc.AddSpeed(3.0f, 1.5f, w);
			else
				kc.AddSpeed(2.0f, 1.5f, w);
		}
		
		//use the weapon so remove
		weapons.RemoveAt (0);
		if (weapons.Count == 0) {
			kart_state.armed = false;
			kart.undrawWeaponGui();
		}
		else
			kart.drawWeaponGui ();
	}

	public bool IsArmed()
	{
		return kart_state.armed;
	}
	
	public void SetWeaponBox(WeaponBoxScript wp)
	{
		takenWeaponBox = wp;
	}
	
	public bool IsWaitingWeapon()
	{
		return kart_state.waiting;
	}
	
	public bool AbleToMove()
	{
		return kart_state.AbleToMove();
	}
	
	public bool IsInvincible()
	{
		return kart_state.IsInvincible();
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
		kart_state.waiting = t;
	}
	
	public void setEvoluteWeapon(bool t)
	{
		kart_state.armedEvolute = t;
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
			for(int i=0;i<j;i++){
				//[w.Split('_').Length - 1] = [1]
				weapons.Add (w.Split('_')[1]);//w.Split('_').Length - 1]);
			}
		}
		else{
			weapons = new List<string> ();
			weapons.Add (w);
		}
		if (!kart_state.armed)
			kart_state.armed= true;
		takenWeaponBox = null;
	}
	
	public void Die(GameObject killer, string weapon)
	{
		//Debug.Log ("mort");
		if(tnt && weapon != "tntExploded")
			Destroy(tnt);
		if (shield != null)
		{
			StartCoroutine(shield.ShieldExplosion());
			if (!kart_state.IsInvincible())
				kart_state.SetInvincibility(1);
			return;
		}
		if (protection != null)
			return;
		// si on est pas invincible : on meurt
		if (!kart_state.IsInvincible())
		{
			StartCoroutine (Transparence ());
			kart_state.SetUnabilityToMove(1f);
			// mise en etat empechant de tirer : 
			kart_state.SetUnabilityToShoot(2.5f);
			if (killer==gameObject)
				kart.AddPoint(-1);
			else
				killer.GetComponent<KartScript>().kart.AddPoint(1);
			if (weapon=="greenBeaker" || weapon== "redBeaker") // pour retirer des pommes
			{
				kart.rmApples(1);
			}
			else kart.rmApples(3);
		}
	}

	public Quaternion GetRotation(){
		return kc.wheels["steering"].transform.rotation;
	}
	
	public Transform GetTransform(){
		return kc.wheels["steering"].transform;
  	}
	
	public Vector3 GetForward(){
		return kc.forwardNormal;
	}
	
	IEnumerator Transparence()
	{
		kc.numberOfJump = 0;
		//clignotment, invincibilité temporaire
		if (!kart_state.IsInvincible())
			kart_state.SetInvincibility(4);
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
	}
	



}
