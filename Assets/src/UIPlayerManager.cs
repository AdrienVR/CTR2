using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIPlayerManager : MonoBehaviour
{
    [HideInInspector]
    public PlayerController Player;

	public GameObject WeaponBackground, AppleBackground;
    public Image WeaponImage, PointImage;
    public Text WeaponText;
	public Text PointText, AppleText, AppleText2;
	public GameObject ArrivingApple, PlusOneImg, LessOneImg;
	public bool IsSuper;
    public float MinTimeSelection = 1.5f;

    public float Period = 0.1f;

    void Update()
    {
        if (m_waitingForWeapon == false)
            return;

        m_chooseTimer += Time.deltaTime;
        if (Player.Controller.GetKeyDown("action"))
        {
            if (m_chooseTimer > MinTimeSelection)
            {
                m_weaponChoosed = true;
            }
        }
    }
	
    public void SetAppleText(string appleNb)
	{
		AppleText.text = "x " + appleNb;
		AppleText2.text = AppleText.text;
	}

	public void SetPointText(string pointNb)
	{
		PointText.text = pointNb;
	}
	public void AnimApple()
	{
		ArrivingApple.SetActive (true);
		StartCoroutine (ArriveApple());
	}
	public void AnimPts(string pointNb, bool suicide)
	{
		GameObject ArrivingPts;
		if (suicide == false)
			ArrivingPts = PlusOneImg;
		else
			ArrivingPts = LessOneImg;
		ArrivingPts.SetActive (true);
		StartCoroutine (ArrivePts(pointNb, ArrivingPts));
	}
	IEnumerator ArrivePts(string pointNb, GameObject ArrivingPts)
	{
		for (float i=0f; i<1; i+=0.04f)
		{
			ArrivingPts.transform.position=Vector3.Lerp(new Vector3 (0, 0, 1),PointText.transform.position,i);
			yield return new WaitForSeconds (0.005f);
		}
		//AudioManager.Instance.Play("miniBip");
		ArrivingPts.SetActive (false);
		SetPointText(pointNb);
		PointText.color = Color.red;
		PointImage.color = Color.red;
		yield return new WaitForSeconds (0.3f);
		PointText.color = Color.white;
		PointImage.color = Color.white;
	}

	IEnumerator ArriveApple()
	{
		for (float i=0f; i<1; i+=0.1f)
		{
			ArrivingApple.transform.position=Vector3.Lerp(new Vector3 (0, 0, 1),AppleText.transform.position,i);
			yield return new WaitForSeconds (0.01f);
		}
		AudioManager.Instance.Play("miniBip");
		ArrivingApple.SetActive (false);
	}

    // WEAPONS

    public void SetWeapon()
    {
        if (Player.KartState.IsArmed)
            return;

        Player.KartState.IsArmed = true;

        Main.statistics.nbWeaponBox++;

        StartCoroutine(RandomWeaponSelection(Player));
    }

    private IEnumerator RandomWeaponSelection(PlayerController player)
    {
        m_waitingForWeapon = true;
        m_weaponChoosed = false;
        m_chooseTimer = 0;

        AudioManager.Instance.PlayLoopingUI("randomBox");

        for (int i = 0; i < 24 && m_weaponChoosed == false; i++)
        {
            yield return new WaitForSeconds(Period);
            UpdateWeapon(WeaponManager.Instance.GetRandomBattleWeapon());
        }

        AudioManager.Instance.StopLoopingUI();

        AssignWeapon();
        m_waitingForWeapon = false;

        AudioManager.Instance.Play("boxRing");
    }
    public void SetSuperWeapons()
    {
        m_superWeapons = true;
		if(WeaponImage.IsActive())
		{
			UpdateWeapon(m_currentWeapon);
			WeaponBackground.SetActive(true);
		}
		AppleBackground.SetActive(true);
		IsSuper = true;
    }

    public void UnsetSuperWeapons()
    {
        m_superWeapons = false;
        WeaponBackground.SetActive(false);
		AppleBackground.SetActive(false);
		IsSuper = false;
    }

    public void UpdateWeapon(ActivableWeapon weapon)
    {
		m_currentWeapon = weapon;
		WeaponImage.gameObject.SetActive(true);
		WeaponBackground.SetActive(IsSuper);
        if (m_superWeapons == true)
        {
            if (weapon.SuperSprite == null)
            {
                // Aku-aku or anything
                if (Player.CharacterSide == CharacterSide.NormalSide || weapon.DarkSprite == null)
                {
                    WeaponImage.sprite = weapon.Sprite;
                    m_currentWeaponPrefab = weapon.Weapon;
                }
                else // Uka-Uka
                {
                    WeaponImage.sprite = weapon.DarkSprite;
                    m_currentWeaponPrefab = weapon.DarkWeapon;
                }
            }
            else // Nitro
            {
                WeaponImage.sprite = weapon.SuperSprite;
                m_currentWeaponPrefab = weapon.SuperWeapon;
            }
        }
        else
        {
            if (Player.CharacterSide == CharacterSide.NormalSide || weapon.DarkSprite == null)
            {
                WeaponImage.sprite = weapon.Sprite;
                m_currentWeaponPrefab = weapon.Weapon;
            }
            else
            {
                WeaponImage.sprite = weapon.DarkSprite;
                m_currentWeaponPrefab = weapon.DarkWeapon;
            }
        }

        m_currentMultiplicity = weapon.Multiplicity;

        if (weapon.Multiplicity > 1)
        {
            WeaponText.text = weapon.Multiplicity.ToString();
            WeaponText.gameObject.SetActive(true);
        }
        else
        {
            WeaponText.gameObject.SetActive(false);
        }
    }

    private void AssignWeapon()
    {
        for (int i = 0; i < m_currentMultiplicity; i++)
        {
            Player.WeaponPrefab.Add(m_currentWeaponPrefab);
        }
    }

    public void DecrementWeaponText()
    {
        --m_currentMultiplicity;
        WeaponText.text = m_currentMultiplicity.ToString();
    }

    public void HideWeapon()
    {
        if (m_waitingForWeapon == true)
            return;

		StartCoroutine (BlinkWeapon ());
        WeaponBackground.SetActive(false);

        WeaponImage.gameObject.SetActive(false);
    }

	IEnumerator BlinkWeapon()
	{
		for(int i=0;i<5;i++)
		{
			WeaponBackground.SetActive(IsSuper);
			WeaponImage.gameObject.SetActive(true);
			yield return new WaitForSeconds(0.1f);
			WeaponBackground.SetActive(false);
			WeaponImage.gameObject.SetActive(false);
			yield return new WaitForSeconds(0.1f);
		}
	}

    private bool m_waitingForWeapon;
    private bool m_weaponChoosed;
    private float m_chooseTimer;
	private ActivableWeapon m_currentWeapon;
    private bool m_superWeapons;

    private int m_currentMultiplicity;
    private GameObject m_currentWeaponPrefab;
}
