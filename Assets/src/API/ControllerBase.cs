using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ControllerBase {

	public static int nControllers;	
	
	private static Dictionary <string, float> last_axis_up = new Dictionary <string, float>();
	private static Dictionary <string, float> last_axis_down = new Dictionary <string, float>();

	private static bool initialized = false;
	public static bool waitingKey = false;
	public static string keyToChange;
	public static string actionToChange;
	
	private string controllerName;
	private int controllerNumber;
	private List<string> buttonList;
	private Dictionary <string, string> axis;
	private Dictionary <string, KeyCode> buttons;

	public ControllerBase  (int i) 
	{
		Debug.Log("Initializing controller "+i);
		controllerNumber = i;
	}
	
	public static void ListenForKey(string action, string key)
	{
		waitingKey = true;
		keyToChange = key;
		actionToChange = action;
	}

}
