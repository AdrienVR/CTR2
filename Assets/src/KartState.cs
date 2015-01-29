using UnityEngine;
using System.Collections;

public class KartState : MonoBehaviour {

	public float unableToShoot;
	public float unableToMove;
	public float invincible;
	public bool armed;
	public bool waiting;
	public bool armedEvolute;

	// Use this for initialization
	void Start () {
	}

	void FixedUpdate () {
		if (unableToShoot > 0)
			unableToShoot -= Time.deltaTime;
		if (unableToMove > 0)
			unableToMove -= Time.deltaTime;
		if (invincible > 0)
			invincible -= Time.deltaTime;
	}
	
	public bool AbleToShoot(){
		if (unableToShoot > 0)
			return false;
		return true;
	}
	
	public bool AbleToMove(){
		if (unableToMove > 0)
			return false;
		return true;
	}
	
	public bool IsInvincible(){
		if (invincible > 0)
			return true;
		return false;
	}
	
	public void SetUnabilityToShoot(float a){
		unableToShoot = a;
	}
	
	public void SetUnabilityToMove(float a){
		unableToMove = a;
	}
	
	public void SetInvincibility(float a){
		invincible = a;
	}

}
