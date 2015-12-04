using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIPlayerManager : MonoBehaviour
{

	public Text AppleText;
	public Text PointText;

	public void setAppleText(string appleNb)
	{
		AppleText.text = "x " + appleNb;
	}
}
