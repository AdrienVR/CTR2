using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControllerTesting : MonoBehaviour {

	public List<GUIText> labels;

	// Use this for initialization
	void Start () {
		StartCoroutine(UpdateCaca());

		for(int i = 1 ; i < 7 ; i++)
		{
			caca.Add("J0_Axis_1".Replace("1", i.ToString()));
		}
	}

	private IEnumerator UpdateCaca()
	{
		string text;
		List<string> cacaTexts = new List<string>{"","","",""};
		string axisName;
		while(true)
		{
			text = "";
			for(int i = 1 ; i < 2 ; i++)
			{
				cacaTexts[i - 1] = "";
				foreach(string rawAxisName in caca)
				{
					axisName = rawAxisName.Replace("0",i.ToString());
					cacaTexts[i - 1] += axisName + " : " + Input.GetAxis(axisName) + "\n";
				}
				text += cacaTexts[i - 1];
			}
			guiText.text = text;

			for(int i = 0 ; i < Input.GetJoystickNames ().Length ; i++)
			{
				labels[i].text = Input.GetJoystickNames ()[i] + 
					"\n\n" +
					cacaTexts[i];
			}
			yield return new WaitForSeconds(0.33f);
		}
	}

	static List<string> caca = new List<string>{
		"J0_X_Axis",
		"J0_Y_Axis"
	};
}
