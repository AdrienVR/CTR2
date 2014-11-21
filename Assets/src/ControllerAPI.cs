using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ControllerAPI {
	
	private static Dictionary <int,ControllerAPI> allControllers = new Dictionary <int,ControllerAPI>();

	// AXIS
	private static Dictionary <string, float> axisValues = new Dictionary<string, float> {
		{"throw",-2f}, {"moveBack",2f},
		{"turnRight",2f}, {"turnLeft",-2f}
	};
	private static Dictionary <string, string> ps1_axis = new Dictionary<string, string> {
		{"throw","J1_StopAxis"}, {"moveBack","J1_StopAxis"},
		{"turnRight","J1_TurnAxis"}, {"turnLeft","J1_TurnAxis"}
	};
	private static Dictionary <string, string> ps2_axis = new Dictionary<string, string> {
		{"throw","J2_StopAxis"}, {"moveBack","J2_StopAxis"},
		{"turnRight","J2_TurnAxis"}, {"turnLeft","J2_TurnAxis"}
	};
	private static Dictionary <string, string> ps3_axis = new Dictionary<string, string> {
		{"throw","J3_StopAxis"}, {"moveBack","J3_StopAxis"},
		{"turnRight","J3_TurnAxis"}, {"turnLeft","J3_TurnAxis"}
	};
	private static Dictionary <string, string> ps4_axis = new Dictionary<string, string> {
		{"throw","J4_StopAxis"}, {"moveBack","J4_StopAxis"},
		{"turnRight","J4_TurnAxis"}, {"turnLeft","J4_TurnAxis"}
	};
	private static Dictionary <string, string> pc_axis = new Dictionary<string, string>();
	
	// BUTTONS
	public static Dictionary <string, KeyCode> pc1 = new Dictionary<string, KeyCode> {
		{"throw",KeyCode.E}, {"moveBack",KeyCode.S},
		{"moveForward",KeyCode.Z}, {"stop",KeyCode.R},
		{"turnRight",KeyCode.D}, {"turnLeft",KeyCode.Q},
		{"jump",KeyCode.Space}, {"jump2",KeyCode.F5}, 
		{"action",KeyCode.A}, {"start",KeyCode.Escape}, 
		{"viewChange",KeyCode.F1}, {"viewInverse",KeyCode.F2},
		{"bip",KeyCode.F3}, {"bip2",KeyCode.F4}
	};
	public static Dictionary <string, KeyCode> pc2 = new Dictionary<string, KeyCode> {
		{"throw",KeyCode.O}, {"moveBack",KeyCode.K},
		{"moveForward",KeyCode.I}, {"stop",KeyCode.P},
		{"turnRight",KeyCode.L}, {"turnLeft",KeyCode.J},
		{"jump",KeyCode.B}, {"jump2",KeyCode.F11}, 
		{"action",KeyCode.U}, {"start",KeyCode.F12}, 
		{"viewChange",KeyCode.F7}, {"viewInverse",KeyCode.F8},
		{"bip",KeyCode.F9}, {"bip2",KeyCode.F10}
	};
	public static Dictionary <string, KeyCode> ps1 = new Dictionary<string, KeyCode> {
		{"moveForward",KeyCode.Joystick1Button2}, {"stop",KeyCode.Joystick1Button3},
		{"jump",KeyCode.Joystick1Button7}, {"jump2",KeyCode.Joystick1Button6},
		{"action",KeyCode.Joystick1Button1},{"start",KeyCode.Joystick1Button9},
		{"viewChange",KeyCode.Joystick1Button4}, {"viewInverse",KeyCode.Joystick1Button5},
		{"bip",KeyCode.Joystick1Button10}, {"bip2",KeyCode.Joystick1Button11}
	};
	public static Dictionary <string, KeyCode> ps2 = new Dictionary<string, KeyCode> {
		{"moveForward",KeyCode.Joystick2Button2}, {"stop",KeyCode.Joystick2Button3},
		{"jump",KeyCode.Joystick2Button7}, {"jump2",KeyCode.Joystick2Button6},
		{"action",KeyCode.Joystick2Button1},{"start",KeyCode.Joystick2Button9},
		{"viewChange",KeyCode.Joystick2Button4}, {"viewInverse",KeyCode.Joystick2Button5},
		{"bip",KeyCode.Joystick2Button10}, {"bip2",KeyCode.Joystick2Button11}
	};
	public static Dictionary <string, KeyCode> ps3 = new Dictionary<string, KeyCode> {
		{"moveForward",KeyCode.Joystick3Button2}, {"stop",KeyCode.Joystick3Button3},
		{"jump",KeyCode.Joystick3Button7}, {"jump2",KeyCode.Joystick3Button6},
		{"action",KeyCode.Joystick3Button1},{"start",KeyCode.Joystick3Button9},
		{"viewChange",KeyCode.Joystick3Button4}, {"viewInverse",KeyCode.Joystick3Button5},
		{"bip",KeyCode.Joystick3Button10}, {"bip2",KeyCode.Joystick3Button11}
	};
	public static Dictionary <string, KeyCode> ps4 = new Dictionary<string, KeyCode> {
		{"moveForward",KeyCode.Joystick4Button2}, {"stop",KeyCode.Joystick4Button3},
		{"jump",KeyCode.Joystick4Button7}, {"jump2",KeyCode.Joystick4Button6},
		{"action",KeyCode.Joystick4Button1},{"start",KeyCode.Joystick4Button9},
		{"viewChange",KeyCode.Joystick4Button4}, {"viewInverse",KeyCode.Joystick4Button5},
		{"bip",KeyCode.Joystick4Button10}, {"bip2",KeyCode.Joystick4Button11}
	};
	public static Dictionary <string, string> actions = new Dictionary<string, string>
	{
		{"Avancer","moveForward"},
		{"Reculer","moveBack"},
		{"Sauter","jump"},
		{"rien1","jump2"},
		{"Actionner Arme","action"},
		{"Mettre en Pause","start"},
		{"Changer Vue","viewChange"},
		{"Inverser Camera","viewInverse"},
		{"Tourner Gauche","turnLeft"},
		{"Tourner Droite","turnRight"},
		{"rien2","bip"},
		{"rien3","bip2"}
	};
	public static int nControllers;	

	public static Dictionary <string, Dictionary <string, KeyCode>> buttonProfiles = 
	new Dictionary <string, Dictionary <string, KeyCode>>{{"keyboard1",pc1}, {"keyboard2",pc2},
		{"xbox1",ps1}, {"xbox2",ps2}, {"xbox3",ps3}, {"xbox4",ps4}};
	private static Dictionary <string, Dictionary <string, string>> axisProfiles = 
	new Dictionary <string, Dictionary <string, string>>{{"keyboard1",pc_axis}, {"keyboard2",pc_axis},
		{"xbox1",ps1_axis}, {"xbox2",ps2_axis}, {"xbox3",ps3_axis}, {"xbox4",ps4_axis}};
	
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

	// -------------------------------------- CONSTRUCTOR ------------------------------
	// Use this for initialization
	public ControllerAPI  (int i) {
		controllerNumber = i;
		if (nControllers != Input.GetJoystickNames ().Length)
			InitJoysticks ();
		AffectControl (Game.playersMapping[i]);
		allControllers [i] = this;

		if (i == 1){
			Debug.Log ("Test saving + loading binary dictionary");
			ControllerAPI.Test (1);//player 1 = keyboard 1 or xbox1
		}
	}

	public static void CheckJoysticks() {
		if (Input.GetJoystickNames ().Length != nControllers)
			InitJoysticks ();
	}
	
	public static void InitJoysticks()
	{
		nControllers = Input.GetJoystickNames ().Length;
		
		int n = nControllers;
		if (n > 4)
			n = 4;

		// BUTTONS -------------------------------------------------------
		Game.playersMapping = new Dictionary<int, string> {{1,"xbox1"},{2,"xbox2"},{3,"xbox3"},{4,"xbox4"}};
		Game.playersMapping[n + 1] = "keyboard1";
		Game.playersMapping[n + 2] = "keyboard2";

		if (!initialized) {
			foreach(Dictionary <string, string> axis_dict in axisProfiles.Values){
				foreach(string key in axis_dict.Keys){
					last_axis_up[key] = 0;
					last_axis_down[key] = 0;
				}
			}
			initialized = true;
		}
		else{
			Debug.Log ("Now "+nControllers+" controllers detected");
			foreach(ControllerAPI a in allControllers.Values)
				a.SetDefaultFor(0);
		}
	}

	public void SetDefaultFor(int n){
		if (n == 0)
			n = controllerNumber;
		controllerName = Game.playersMapping [n];
		Update ();
	}

	public void AffectControl(string type){
		controllerName = type;
		Update ();
	}

	public void Update(){
		axis = axisProfiles [controllerName];
		buttons = buttonProfiles[controllerName];
		buttonList = new List<string> (buttons.Keys);
	}

	public bool IsPressed(string action)
	{
		if (buttonList.IndexOf (action) != -1){
			return Input.GetKey (buttons [action]);}
		else{
			if (axisValues[action] < 0)
				return (Input.GetAxis (axis[action]) > axisValues[action] && Input.GetAxis (axis[action]) < Game.thresholdAxis);
			else
				return (Input.GetAxis (axis[action]) < axisValues[action] && Input.GetAxis (axis[action]) > Game.thresholdAxis);
		}
	}
	
	public bool GetKeyDown(string action)
	{
		//Debug.Log ("key : " + action);
		if (buttonList.IndexOf (action) != -1)
			return Input.GetKeyDown (buttons [action]);
		else if (!(Input.GetAxis (axis[action]) < last_axis_down[action]+Game.thresholdAxis && 
		           Input.GetAxis (axis[action]) > last_axis_down[action]-Game.thresholdAxis)){
			if (System.Math.Abs (Input.GetAxis (axis[action]))<Game.thresholdAxis){
				last_axis_down[action] = 0;
				return false;
			}
			last_axis_down[action] = Input.GetAxis (axis[action]);
			if (axisValues[action] < 0)
				return (Input.GetAxis (axis[action]) > axisValues[action] && Input.GetAxis (axis[action]) < Game.thresholdAxis);
			else
				return (Input.GetAxis (axis[action]) < axisValues[action] && Input.GetAxis (axis[action]) > Game.thresholdAxis);
		}
		return false;
	}
	
	public static void ListenForKey(string action, string key)
	{
		waitingKey = true;
		keyToChange = key;
		actionToChange = action;
	}

	public static bool CheckForAxis(){
		if (waitingKey){
			foreach(Dictionary <string, string> ax in axisProfiles.Values){
				foreach(string action in ax.Keys){
					if (CheckDownAxis(action, ax)){
						waitingKey = false;
						foreach(ControllerAPI c in allControllers.Values){
							List<string> axisList = new List<string> (c.axis.Keys);
							if (c.buttonList.IndexOf(actionToChange)!=-1 && c.buttons[actionToChange].ToString() == keyToChange)
							{
								c.buttons.Remove(actionToChange);
								c.axis[actionToChange] = ax[action];
								last_axis_down[actionToChange] = 0;
								last_axis_up[actionToChange] = 0;
								c.Update();
								break;
							}
							else if(axisList.IndexOf(actionToChange)!=-1 && c.axis[actionToChange] == keyToChange)
							{
								c.axis[actionToChange] = ax[action];
								last_axis_down[actionToChange] = 0;
								last_axis_up[actionToChange] = 0;
								c.Update();
								break;
							}
						}
						return true;
					}
				}
			}
		}
		return false;
	}
	
	public static bool CheckForKey(){
		if (waitingKey){
			foreach(KeyCode key in KeyCodes.codes){
				if (Input.GetKeyDown(key)){
					waitingKey = false;
					foreach(ControllerAPI c in allControllers.Values){
						List<string> axisList = new List<string> (c.axis.Keys);
						if (c.buttonList.IndexOf(actionToChange)!=-1 && c.buttons[actionToChange].ToString() == keyToChange)
						{
							c.buttons[actionToChange] = key;
							c.Update();
							break;
						}
						else if(axisList.IndexOf(actionToChange)!=-1 && c.axis[actionToChange] == keyToChange)
						{
							c.axis.Remove(actionToChange);
							c.buttons[actionToChange] = key;
							c.Update();
							break;
						}
					}
					return true;
				}
				}
		}
		return false;
	}
	
	public static bool StaticGetKeyDown(int i, string action)
	{
		return allControllers [i].GetKeyDown (action);
	}
	
	public bool GetKeyUp(string action)
	{
		if (buttonList.IndexOf (action) != -1)
			return Input.GetKeyUp (buttons [action]);
		else if (Input.GetAxis (axis[action]) != last_axis_up[action]){
			last_axis_up[action] = Input.GetAxis (axis[action]);
			if (axisValues[action] < 0)
				return (Input.GetAxis (axis[action]) > axisValues[action] && Input.GetAxis (axis[action]) < Game.thresholdAxis);
			else
				return (Input.GetAxis (axis[action]) < axisValues[action] && Input.GetAxis (axis[action]) > Game.thresholdAxis);
		}
		return false;
	}
	
	public static bool StaticIsPressed(int i, string action)
	{
		return (allControllers [i].IsPressed (action) && allControllers [i].KeyValue(action)>Game.thresholdAxis);
	}
	
	public float KeyValue(string action)
	{
		if (buttonList.IndexOf (action) != -1){
			if (Input.GetKey (buttons [action])){
				//Debug.Log("Emulate axis for "+key);
				return 1.0f;
			}
		}
		else
			return System.Math.Abs(Input.GetAxis (axis[action]));
		return 0;
	}

	// return the name of the button corresponding to the action
	public static string KeyIs(int controllerN, string action)
	{
		ControllerAPI c = allControllers[controllerN];
		if (c.buttonList.IndexOf(action) != -1)
			return c.buttons[action].ToString();
		else
			return c.axis[action];
	}

	// retourne le nom du boutton correspondant à l'action
	public static string GetActionName(string n)
	{
		return actions [n];
	}

	// TODO
	public static void Test(int i)
	{
		var dictionary = new Dictionary<string, string>();
		dictionary["perls"] = "dot";
		dictionary["net"] = "perls";
		dictionary["dot"] = "net";
		Write(dictionary, "C:\\dictionary"+allControllers[i].controllerName+".bin");

		dictionary = Read( "C:\\dictionary"+allControllers[i].controllerName+".bin");
		foreach (var pair in dictionary)
		{
			Debug.Log(pair);
		}
	}
			
	static void Write(Dictionary<string, string> dictionary, string file)
	{
				using (FileStream fs = File.OpenWrite(file))
					using (BinaryWriter writer = new BinaryWriter(fs))
				{
					// Put count.
					writer.Write(dictionary.Count);
					// Write pairs.
					foreach (var pair in dictionary)
					{
						writer.Write(pair.Key);
						writer.Write(pair.Value);
					}
				}
	}

	static Dictionary<string, string> Read(string file)
	{
		var result = new Dictionary<string, string>();
		using (FileStream fs = File.OpenRead(file))
			using (BinaryReader reader = new BinaryReader(fs))
		{
			// Get count.
			int count = reader.ReadInt32();
			// Read in all pairs.
			for (int i = 0; i < count; i++)
			{
				string key = reader.ReadString();
				string value = reader.ReadString();
				result[key] = value;
			}
		}
		return result;
	}

	private static bool CheckDownAxis(string action, Dictionary <string, string> axis_dict)
	{
		float axisValue = Input.GetAxis (axis_dict [action]);
		if (!(axisValue < last_axis_down[action]+Game.thresholdAxis && 
		      axisValue > last_axis_down[action]-Game.thresholdAxis))
		{
			if (System.Math.Abs (axisValue)<Game.thresholdAxis){
				last_axis_down[action] = 0;
				return false;
			}
			last_axis_down[action] = axisValue;
			if (axisValues[action] < 0)
				return (axisValue > axisValues[action] && axisValue < Game.thresholdAxis);
			else
				return (axisValue < axisValues[action] && axisValue > Game.thresholdAxis);
		}
		return false;
	}
}
