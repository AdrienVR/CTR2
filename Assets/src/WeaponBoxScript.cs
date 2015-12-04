using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponBoxScript : MonoBehaviour {

	public AudioClip randomMusic;
	public AudioClip endMusic;
	private int nbImgArmes;
	private float timeLookingWeapon;
    
    public Collider SelfCollider;
    public AudioSource LoopSource;
    public AudioSource Source;

	void Start()
	{
		m_selfCollider = GetComponent<Collider> ();
	}

	void OnTriggerEnter(Collider other)
	{
        //animation
		m_selfCollider.enabled = false;
        //Source.Play();
		StartCoroutine (TakeCoroutine());

        KartScript player = null;

        if (other.tag != "kart")
        {
            if (other.tag == "weapon")
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
            player = other.GetComponent<KartScript>();
        }

        Main.statistics.nbWeaponBox++;

        if (player.IsArmed() || player.IsWaitingWeapon())
			return;
        player.setWaitingWeapon (true);
        player.SetWeaponBox(this);

		//animation of giving weapon
		StartCoroutine(RandomWeaponSelection(player));
	}

    private IEnumerator RandomWeaponSelection(KartScript player)
    {
        GetComponent<AudioSource>().PlayOneShot(randomMusic);
        while (nbImgArmes<25 && nbImgArmes>0) {
			yield return new WaitForSeconds (randomMusic.length*3/4);
            ActivableWeapon weapon = WeaponManager.Instance.GetRandomBattleWeapon();
        }
		GetComponent<AudioSource>().PlayOneShot(endMusic);
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
        GetComponent<Collider>().enabled = false;
        GetComponent<Animation>().Play ("boxDisappear");
		yield return new WaitForSeconds (2f);
		GetComponent<Animation>().Play ("boxGrow");
		yield return new WaitForSeconds (1.3f);
		GetComponent<Collider>().enabled = true;
	}

	private Collider m_selfCollider;
}