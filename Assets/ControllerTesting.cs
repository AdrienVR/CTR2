using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControllerTesting : MonoBehaviour {

	public List<GUIText> labels;

	// Use this for initialization
	void Start () {
		StartCoroutine(UpdateCaca());
	}

	private List<string> controllerNames = new List<string>();

	private IEnumerator UpdateCaca()
	{
		List<string> cacaTexts = new List<string>{"","","",""};
		string axisName;
		while(true)
		{
			for(int i = 1 ; i < 5 ; i++)
			{
				cacaTexts[i - 1] = "";
				foreach(string rawAxisName in caca)
				{
					axisName = rawAxisName.Replace("0",i.ToString());
					cacaTexts[i - 1] += axisName + " : " + Input.GetAxis(axisName) + "\n";
				}
			}

			for(int i = 0 ; i < Input.GetJoystickNames ().Length ; i++)
			{
				if (controllerNames.IndexOf(Input.GetJoystickNames ()[i]) == -1)
					controllerNames.Add(Input.GetJoystickNames ()[i]);
				labels[i].text = controllerNames[i] + 
					"\n\n" +
					cacaTexts[i];
			}
			
			for(int i = Input.GetJoystickNames ().Length ; i < 4 ; i++)
			{
				string name = "none";
				if (i < controllerNames.Count)
					name = controllerNames[i];
				labels[i].text = name + 
					"\n\n" +
						cacaTexts[i];
			}
			yield return new WaitForSeconds(0.1f);
		}
	}

	static List<string> caca = new List<string>
	{
		{"J0_X_Axis"},
		{"J0_Y_Axis"},
		{"J0_Axis_1"},
		{"J0_Axis_2"},
		{"J0_Axis_3"},
		{"J0_Axis_4"},
		{"J0_Axis_5"},
		{"J0_Axis_6"},
		
	};

}
