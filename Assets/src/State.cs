using UnityEngine;
using System.Collections;

public class State : MonoBehaviour {

	public Kart kart;
	public KartController kc;

	public float stopDie;
	public float unableToShoot;
	public float invincible;
	public bool armed;
	public bool armedEvolute;

	// Use this for initialization
	void Start () {
		kc = gameObject.GetComponent<KartController>();
	}

	void FixedUpdate () {
		if (stopDie > 0)
			stopDie -= Time.deltaTime;
		if (unableToShoot > 0)
			unableToShoot -= Time.deltaTime;
		if (invincible > 0)
			invincible -= Time.deltaTime;
	}

	public bool CanMoveAfterShot(){
		if (stopDie > 0)
			return false;
		return true;
	}
	
	public bool UnableToShoot(){
		if (unableToShoot > 0)
			return true;
		return false;
	}
	
	public bool IsInvincible(){
		if (invincible > 0)
			return true;
		return false;
	}
}
