using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using MiniJSON;
using System.Linq;

public class ControllerResources
{

	// JSON files path : Assets/Config (for Unity Editor) and Build/X_Data/Config (for Built Player)
	const string relativePath = "Config";

	public static int controllers = 0;

	private static IDictionary controllersDictionary;
	private static IDictionary actionsDictionary;

	public static List<string> ActionNames;

	private static void LoadConfigFiles()
	{

		string path = Path.Combine(Application.dataPath, Path.Combine(relativePath, "ControllersConfig.JSON"));
		string controllersConfigFile = File.ReadAllText(path);
		
		path = Path.Combine(Application.dataPath, Path.Combine(relativePath, "ActionsConfig.JSON"));
		string actionsConfigFile = File.ReadAllText(path); 

		controllersDictionary = (IDictionary) Json.Deserialize(controllersConfigFile);
		actionsDictionary = (IDictionary) Json.Deserialize(actionsConfigFile);

		ActionNames = new List<string>();
		foreach(string action in actionsDictionary.Values)
		{
			ActionNames.Add(action);
		}
	}

	public static Dictionary <string, List <VirtualKey> > GetButtons(string type)
	{
		if (controllersDictionary == null || actionsDictionary == null)
		{
			LoadConfigFiles();
		}

		if (controllersDictionary.Contains(type) == false)
		{
			Debug.LogError("The controller : '"+type + "' has no default config !");
			if (type == "Keyboard1")
			{
				Debug.LogError("Please check config files !!!");
				return null;
			}
			Debug.LogWarning("Trying with Keyboard1, default config...");
			return GetButtons("Keyboard1");
		}
		
		IDictionary controller = (IDictionary) controllersDictionary[type];

		Dictionary <string, List <VirtualKey> > buttons = new Dictionary <string, List<VirtualKey>>();

		bool succeed = false;
		//bool usingAxis = false;
		bool isControllerDependant = false;
		foreach (string action in controller.Keys) 
		{
			string realActionName = actionsDictionary[action] as string;
			List<VirtualKey> virtualKeys = new List <VirtualKey> ();
			IList keys = (IList) controller[action];
			foreach(string key in keys)
			{
				string contractedAxisName = key.Substring(0,key.Length - 1);
				int offset = 0;
				if (KeyCodes.nControllerDependant.IndexOf(key) != -1 || KeyCodes.nControllerDependant.IndexOf(contractedAxisName) != -1)
				{
					isControllerDependant = true;
				}

				if (KeyCodes.axisNames.IndexOf(contractedAxisName) != -1)
				{
					if (isControllerDependant)
						offset = controllers * 8;
					string axisName = KeyCodes.axis[KeyCodes.axisNames.IndexOf(contractedAxisName) + offset];
					string sign = key.Substring(key.Length - 1, 1);
					if (sign == "+")
						virtualKeys.Add( new Axis(realActionName, axisName, 0, 1) );
					else // sign == "-"
						virtualKeys.Add( new Axis(realActionName, axisName, 0, -1) );
					//usingAxis = true;
				}
				else if (KeyCodes.codeNames.IndexOf(key) != -1)
				{
					if (isControllerDependant)
						offset = controllers * 20;
					KeyCode code = KeyCodes.codes[KeyCodes.codeNames.IndexOf(key) + offset];
					virtualKeys.Add(new Key(code,realActionName));
				}
				else
				{
					Debug.LogError("The key : '"+key + "' has not been recognized !");
				}
			}
			buttons[realActionName] = virtualKeys;
			succeed = true;
			//Debug.Log ("Loading keys for '"+realActionName+"' action succeeded !");
		}

		// some checks...
		if (succeed == true)
		{
			Debug.Log("The controller : '"+type + "' has been nicely loaded !");
		}

		return buttons;
	}
}
