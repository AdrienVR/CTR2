using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIPlayerManager : MonoBehaviour
{

	public Text PointText, AppleText, AppleText2;
	public GameObject ArrivingApple;

	public void SetAppleText(string appleNb)
	{
		AppleText.text = "x " + appleNb;
		AppleText2.text = AppleText.text;
	}

	public void AnimApple()
	{
		ArrivingApple.SetActive (true);
		StartCoroutine (ArriveApple());
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

}
