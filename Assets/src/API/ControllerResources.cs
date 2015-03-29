using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ControllerResources
{

	private static Dictionary <string, float> axisValues = new Dictionary<string, float> ();
	private static Dictionary <string, float> newAxisValues = new Dictionary<string, float> ();

	private static Dictionary <string, string> ps1_axis = new Dictionary<string, string> {
		{"throw","J1_StopAxis"}, {"moveBack","J1_StopAxis"},
		{"turnRight","J1_TurnAxis"}, {"turnLeft","J1_TurnAxis"},
		{"jump","J1_JumpAxis"}, {"jump2","J1_JumpAxis"}
	};
	private static Dictionary <string, string> ps2_axis = new Dictionary<string, string> {
		{"throw","J2_StopAxis"}, {"moveBack","J2_StopAxis"},
		{"turnRight","J2_TurnAxis"}, {"turnLeft","J2_TurnAxis"},
		{"jump","J2_JumpAxis"}, {"jump2","J2_JumpAxis"}
	};
	private static Dictionary <string, string> ps3_axis = new Dictionary<string, string> {
		{"throw","J3_StopAxis"}, {"moveBack","J3_StopAxis"},
		{"turnRight","J3_TurnAxis"}, {"turnLeft","J3_TurnAxis"},
		{"jump","J3_JumpAxis"}, {"jump2","J3_JumpAxis"}
	};
	private static Dictionary <string, string> ps4_axis = new Dictionary<string, string> {
		{"throw","J4_StopAxis"}, {"moveBack","J4_StopAxis"},
		{"turnRight","J4_TurnAxis"}, {"turnLeft","J4_TurnAxis"},
		{"jump","J4_JumpAxis"}, {"jump2","J4_JumpAxis"}
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
	public static Dictionary <string, KeyCode> pc3 = new Dictionary<string, KeyCode> {
		{"throw",KeyCode.None}, {"moveBack",KeyCode.DownArrow},
		{"moveForward",KeyCode.UpArrow}, {"stop",KeyCode.None},
		{"turnRight",KeyCode.RightArrow}, {"turnLeft",KeyCode.LeftArrow},
		{"jump",KeyCode.RightControl}, {"jump2",KeyCode.None}, 
		{"action",KeyCode.Return}, {"start",KeyCode.None}, 
		{"viewChange",KeyCode.None}, {"viewInverse",KeyCode.None},
		{"bip",KeyCode.None}, {"bip2",KeyCode.None}
	};
	public static Dictionary <string, KeyCode> xbox1 = new Dictionary<string, KeyCode> {
		{"moveForward",KeyCode.Joystick1Button0}, {"stop",KeyCode.Joystick1Button2},
		{"action",KeyCode.Joystick1Button1},{"start",KeyCode.Joystick1Button7},
		{"viewChange",KeyCode.Joystick1Button4}, {"viewInverse",KeyCode.Joystick1Button5},
		{"bip",KeyCode.Joystick1Button8}, {"bip2",KeyCode.Joystick1Button9}
	};
	public static Dictionary <string, KeyCode> xbox2 = new Dictionary<string, KeyCode> {
		{"moveForward",KeyCode.Joystick2Button0}, {"stop",KeyCode.Joystick2Button2},
		{"action",KeyCode.Joystick2Button1},{"start",KeyCode.Joystick2Button7},
		{"viewChange",KeyCode.Joystick2Button4}, {"viewInverse",KeyCode.Joystick2Button5},
		{"bip",KeyCode.Joystick2Button8}, {"bip2",KeyCode.Joystick2Button9}
	};
	public static Dictionary <string, KeyCode> xbox3 = new Dictionary<string, KeyCode> {
		{"moveForward",KeyCode.Joystick3Button0}, {"stop",KeyCode.Joystick3Button2},
		{"action",KeyCode.Joystick3Button1},{"start",KeyCode.Joystick3Button7},
		{"viewChange",KeyCode.Joystick3Button4}, {"viewInverse",KeyCode.Joystick3Button5},
		{"bip",KeyCode.Joystick3Button8}, {"bip2",KeyCode.Joystick3Button9}
	};
	public static Dictionary <string, KeyCode> xbox4 = new Dictionary<string, KeyCode> {
		{"moveForward",KeyCode.Joystick4Button0}, {"stop",KeyCode.Joystick4Button2},
		{"action",KeyCode.Joystick4Button1},{"start",KeyCode.Joystick4Button7},
		{"viewChange",KeyCode.Joystick4Button4}, {"viewInverse",KeyCode.Joystick4Button5},
		{"bip",KeyCode.Joystick4Button8}, {"bip2",KeyCode.Joystick4Button9}
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
	new Dictionary <string, Dictionary <string, KeyCode>>{{"keyboard1",pc1}, {"keyboard2",pc2}, {"keyboard3",pc3},
		{"xbox1",xbox1}, {"xbox2",xbox2}, {"xbox3",xbox3}, {"xbox4",xbox4}};

	private static Dictionary <string, Dictionary <string, string>> axisProfiles = 
	new Dictionary <string, Dictionary <string, string>>{{"keyboard1",pc_axis}, {"keyboard2",pc_axis}, {"keyboard3",pc_axis},
		{"xbox1",ps1_axis}, {"xbox2",ps2_axis}, {"xbox3",ps3_axis}, {"xbox4",ps4_axis}};

	private static Dictionary<int, string> playersMapping;// {1:xbox1, etc}
	
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
}
