using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dictionnaries : MonoBehaviour
{
	// src : http://crashbandicoot.wikia.com/wiki/Crash_Team_Racing
	public static Dictionary <int, bool> controllersEnabled = new Dictionary <int, bool> {
		{1,false},{2,false},{3,false},{4,false}};
	public static Dictionary <int, string> normalWeapons =  new Dictionary<int, string> {
		{1,"greenBeaker"},{2,"greenShield"},{3,"bomb"},{4,"triple_bomb"},{5,"triple_missile"},
		{6,"Aku-Aku"},{7,"TNT"},{8,"turbo"}	};
	public static Dictionary <int, string> superWeapons = new Dictionary<int, string> {
		{1,"redBeaker"},{2,"blueShield"},{3,"bomb"},{4,"triple_bomb"},{5,"triple_missile"},
		{6,"superAku-Aku"},{7,"nitro"},{8,"superTurbo"}	};
	public static Dictionary <int, List<Rect>> cameraMap = new Dictionary <int, List<Rect>>{
		{1, new List<Rect>(){new Rect(0, 0, 1, 1)}},
		{2, new List<Rect>(){new Rect(0, 0.51f, 1, 0.49f), new Rect(0, 0, 1, 0.49f)}},
		{3, new List<Rect>(){new Rect(0, 0.51f, 1, 0.49f), new Rect(0, 0, 0.495f, 0.49f), 
				new Rect(0.505f, 0, 0.495f, 0.49f)}},
		{4, new List<Rect>(){new Rect(0, 0.51f, 0.49f, 0.49f), new Rect(0, 0, 0.49f, 0.49f), 
				new Rect(0.51f, 0.51f, 0.49f, 0.49f), new Rect(0.51f, 0, 0.49f, 0.49f)}}
	};

	public static List<string> poseWeapons = new List<string>() {"nitro", "TNT", "greenBeaker", "redBeaker"};
	public static List<string> launchWeapons = new List<string>() {"missile", "bomb"};
	public static List<string> boxes = new List<string>() {"weaponBox","appleBox"};
	public static List<string> unkillable = new List<string>() {"weaponBox","appleBox", "Ground", "PALMIER3","totem"};
	public static List<string> protectWeapons = new List<string>() {"Aku-Aku", "greenShield", "blueShield","superAku-Aku"};
	public static List<string> protectors = new List<string>() {"Aku-Aku","superAku-Aku", "Uka-Uka","superUka-Uka"};
	public static List<string> shields = new List<string>() {"greenShield", "blueShield"};
	
	public static Dictionary <int, Dictionary<string, KeyCode>> playersMapping;
	public static Dictionary <int, Dictionary<string, string>> axisMapping;
	public static List<string> listKarts = new List<string>{"Crash","Crash","Crash","Crash"};
	public static List<string> characters = new List<string>() {"kartCrash"};
	public static List<string> listMap = new List<string>() {"dinoRace", "plage"};

	// AXIS
	private static Dictionary <string, string> ps1_axis = new Dictionary<string, string> {
		{"turn","J1_TurnAxis"}, {"stop","J1_StopAxis"}		};
	private static Dictionary <string, string> ps2_axis = new Dictionary<string, string> {
		{"turn","J2_TurnAxis"}, {"stop","J2_StopAxis"}		};
	private static Dictionary <string, string> ps3_axis = new Dictionary<string, string> {
		{"turn","J3_TurnAxis"}, {"stop","J3_StopAxis"}		};
	private static Dictionary <string, string> ps4_axis = new Dictionary<string, string> {
		{"turn","J4_TurnAxis"}, {"stop","J4_StopAxis"}		};

	// BUTTONS
	public static Dictionary <string, KeyCode> pc1 = new Dictionary<string, KeyCode> {
		{"moveForward",KeyCode.Z}, {"moveBack",KeyCode.S},
		{"turnRight",KeyCode.Q}, {"turnLeft",KeyCode.D},
		{"jump",KeyCode.Space}, {"jump2",KeyCode.F5}, 
		{"action",KeyCode.A}, {"start",KeyCode.Escape}, 
		{"viewChange",KeyCode.F1}, {"viewInverse",KeyCode.F2},
		{"bip",KeyCode.F3}, {"bip2",KeyCode.F4}
	};
	public static Dictionary <string, KeyCode> pc2 = new Dictionary<string, KeyCode> {
		{"moveForward",KeyCode.I}, {"moveBack",KeyCode.K},
		{"turnRight",KeyCode.J}, {"turnLeft",KeyCode.L},
		{"jump",KeyCode.B}, {"jump2",KeyCode.F11}, 
		{"action",KeyCode.U}, {"start",KeyCode.F12}, 
		{"viewChange",KeyCode.F7}, {"viewInverse",KeyCode.F8},
		{"bip",KeyCode.F9}, {"bip2",KeyCode.F10}
	};
	public static Dictionary <string, KeyCode> ps1 = new Dictionary<string, KeyCode> {
		{"moveForward",KeyCode.Joystick1Button2}, {"moveBack",KeyCode.Joystick1Button3},
		{"jump",KeyCode.Joystick1Button7}, {"jump2",KeyCode.Joystick1Button6},
		{"action",KeyCode.Joystick1Button1},{"start",KeyCode.Joystick1Button9},
		{"viewChange",KeyCode.Joystick1Button4}, {"viewInverse",KeyCode.Joystick1Button5},
		{"bip",KeyCode.Joystick1Button10}, {"bip2",KeyCode.Joystick1Button11}
	};
	public static Dictionary <string, KeyCode> ps2 = new Dictionary<string, KeyCode> {
		{"moveForward",KeyCode.Joystick2Button2}, {"moveBack",KeyCode.Joystick2Button3},
		{"jump",KeyCode.Joystick2Button7}, {"jump2",KeyCode.Joystick2Button6},
		{"action",KeyCode.Joystick2Button1},{"start",KeyCode.Joystick2Button9},
		{"viewChange",KeyCode.Joystick2Button4}, {"viewInverse",KeyCode.Joystick2Button5},
		{"bip",KeyCode.Joystick2Button10}, {"bip2",KeyCode.Joystick2Button11}
	};
	public static Dictionary <string, KeyCode> ps3 = new Dictionary<string, KeyCode> {
		{"moveForward",KeyCode.Joystick3Button2}, {"moveBack",KeyCode.Joystick3Button3},
		{"jump",KeyCode.Joystick3Button7}, {"jump2",KeyCode.Joystick3Button6},
		{"action",KeyCode.Joystick3Button1},{"start",KeyCode.Joystick3Button9},
		{"viewChange",KeyCode.Joystick3Button4}, {"viewInverse",KeyCode.Joystick3Button5},
		{"bip",KeyCode.Joystick3Button10}, {"bip2",KeyCode.Joystick3Button11}
	};
	public static Dictionary <string, KeyCode> ps4 = new Dictionary<string, KeyCode> {
		{"moveForward",KeyCode.Joystick4Button2}, {"moveBack",KeyCode.Joystick4Button3},
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

	private static int nControllers;

	// Use this for initialization
	void Start ()
	{
		InitJoysticks ();
	}

	void FixedUpdate() {
		if (Input.GetJoystickNames ().Length != nControllers)
			InitJoysticks ();
	}

	public static void InitJoysticks()
	{
		nControllers = Input.GetJoystickNames ().Length;

		int n = nControllers;
		if (n > 4)
			n = 4;
		for(int i = 1; i < n+1 ; i++)
			Dictionnaries.controllersEnabled[i] = true;

		// AXIS ---------------------------------------------------------
		axisMapping = new Dictionary<int, Dictionary<string, string>> {{1,ps1_axis},{2,ps2_axis},{3,ps3_axis},{4,ps4_axis}};

		// BUTTONS -------------------------------------------------------
		playersMapping = new Dictionary<int, Dictionary<string, KeyCode>> {{1,ps1},{2,ps2},{3,ps3},{4,ps4}};
		playersMapping[n + 1] = pc1;
		playersMapping[n + 2] = pc2;
	}
}

