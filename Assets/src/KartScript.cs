using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// this class manages the actions

public class KartScript : MonoBehaviour 
{
	
	public static bool stop = true;
	public bool stopDie = false;
	
	public Dictionary <string, Transform> wheels = new Dictionary <string, Transform>();

	private KartController kc;
	private Kart kart;
	private KartState kart_state;
	
	private WeaponBehavior bomb;
	private WeaponBoxScript takenWeaponBox;
	public WeaponBehavior shield;
	public WeaponBehavior protection;
	public GameObject tnt;
	private List<GameObject> smoke = new List<GameObject>();

	public List<string> weapons = new List<string>();
	
	private bool baddie = false;
	private ControllerBase controller;
	private float facteurSens = 1f;

	// Use this for initialization
	void Start () {
		kc = GetComponent<KartController>();
		kart_state = GetComponent<KartState>();
		controller = ControllerManager.Instance.GetController(kart.numeroJoueur - 1);

		
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
	void Update () 
	{
		if (controller.GetKeyDown("action")) 
		{
			if (!kart_state.AbleToShoot())
				return;
			facteurSens = 1;
			if (controller.GetKey("down"))
				facteurSens = -1;
			
			if (bomb == null)
				UseWeapon ();
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
		
		
		if (weapons.Count == 0)
			return;
		// apply
		string w = weapons [0];

		// superisation
		if (IsSuper())
		{
		}

		// computing the side
	
		
		//use the weapon so remove
		weapons.RemoveAt (0);
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
			GetComponent<AudioSource>().Play();
			kart.guitextApples.text = "x "+kart.nbApples.ToString();
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
        Animator anim = transform.GetChild(0).GetComponent<Animator>();
        anim.enabled = true;
        anim.Play("Dead");
		// si on est pas invincible : on meurt
		if (!kart_state.IsInvincible())
		{
			StartCoroutine (Transparence ());
			kart_state.SetUnabilityToMove(1f);
			// mise en etat empechant de tirer : 
			kart_state.SetUnabilityToShoot(2.5f);
			if (killer==gameObject)
			{
				kart.AddPoint(-1);
				Main.statistics.getStatPerso(kart.numeroJoueur).nbSuicides++;
			}
			else
			{
				killer.GetComponent<KartScript>().kart.AddPoint(1);
				Main.statistics.getStatPerso(killer.GetComponent<KartScript>().kart.numeroJoueur).PtsMarques.Add(kart.numeroJoueur);
				Main.statistics.getStatPerso(kart.numeroJoueur).PtsDonnes.Add(killer.GetComponent<KartScript>().kart.numeroJoueur);
			}
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
			wheels [w].GetComponent<Renderer>().enabled = false;
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
					wheels [w].GetComponent<Renderer>().enabled = !wheels [w].GetComponent<Renderer>().enabled;
				}
				/*foreach(GameObject w in smoke){
					w.SetActive(!w.activeSelf);
				}*/
			}
		}
		foreach(string w in wheels.Keys)
		{
			wheels [w].GetComponent<Renderer>().enabled = true;
		}
		foreach(GameObject w in smoke){
			w.SetActive(true);
		}
	}
	



}
