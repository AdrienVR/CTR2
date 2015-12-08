using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponBoxScript : MonoBehaviour {

	public AudioClip randomMusic;
	public AudioClip endMusic;
	private int nbImgArmes;
	private float timeLookingWeapon;

	void Start()
	{
		m_selfCollider = GetComponent<Collider> ();
        m_loopSource = GetComponent<AudioSource>();
        m_animation = GetComponent<Animation>();
    }

	void OnTriggerEnter(Collider other)
	{
        //animation
		m_selfCollider.enabled = false;
		AudioManager.Instance.Play("boxExplosion");
		StartCoroutine (TakeCoroutine());

        PlayerController player = null;

        if (other.tag != "Kart")
        {
            if (other.tag == "Weapon")
            {
                player = other.GetComponent<WeaponBehavior>().Owner;
            }
            else
            {
                return;
            }
        }
        else
        {
            player = other.GetComponent<PlayerController>();
        }

        player.UIPlayerManager.SetWeapon();
	}


	public bool selectRandomWeapon()
	{
		if(timeLookingWeapon>1f)
			nbImgArmes = 25;
		else 
			return false;
		return true;
	}
	
	private IEnumerator TakeCoroutine()
    {
        m_selfCollider.enabled = false;
        m_animation.Play("boxDisappear");
		yield return new WaitForSeconds (2f);
        m_animation.Play("boxGrow");
		yield return new WaitForSeconds (1.3f);
        m_selfCollider.enabled = true;
	}

	private Collider m_selfCollider;
    private Animation m_animation;

    private AudioSource m_loopSource;
}