using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
	public static float thresholdAxis = 0.3f;
	// src : http://crashbandicoot.wikia.com/wiki/Crash_Team_Racing
	public static Dictionary <int, string> normalWeapons =  new Dictionary<int, string> {
		{1,"greenBeaker"},{2,"greenShield"},{3,"bomb"},{4,"triple_bomb"},{5,"triple_missile"},
		{6,"Aku-Aku"},{7,"TNT"},{8,"turbo"}	};
	public static Dictionary <int, string> superWeapons = new Dictionary<int, string> {
		{1,"redBeaker"},{2,"blueShield"},{3,"bomb"},{4,"triple_bomb"},{5,"triple_missile"},
		{6,"Aku-Aku"},{7,"nitro"},{8,"turbo"}	};
	public static Dictionary <int, List<Rect>> cameraMap = new Dictionary <int, List<Rect>>{
		{1, new List<Rect>(){new Rect(0, 0, 1, 1)}},
		{2, new List<Rect>(){new Rect(0, 0.51f, 1, 0.49f), new Rect(0, 0, 1, 0.49f)}},
		{3, new List<Rect>(){new Rect(0, 0.51f, 1, 0.49f), new Rect(0, 0, 0.495f, 0.49f), 
				new Rect(0.505f, 0, 0.495f, 0.49f)}},
		{4, new List<Rect>(){new Rect(0, 0.51f, 0.49f, 0.49f), new Rect(0, 0, 0.49f, 0.49f), 
				new Rect(0.51f, 0.51f, 0.49f, 0.49f), new Rect(0.51f, 0, 0.49f, 0.49f)}}
	};
	
	public static List<string> instatiableWeapons = new List<string>() {"nitro", "TNT", "greenBeaker", "redBeaker",
		"missile", "bomb", "Aku-Aku", "greenShield", "blueShield", "Uka-Uka"};
	public static List<string> poseWeapons = new List<string>() {"nitro", "TNT", "greenBeaker", "redBeaker"};
	public static List<string> launchWeapons = new List<string>() {"missile", "bomb"};
	public static List<string> boxes = new List<string>() {"weaponBox","appleBox"};
	public static List<string> unkillable = new List<string>() {"weaponBox","appleBox", "Ground", "PALMIER3","totem"};
	public static List<string> protectWeapons = new List<string>() {"Aku-Aku", "greenShield", "blueShield", "Uka-Uka"};
	public static List<string> protectors = new List<string>() {"Aku-Aku", "Uka-Uka"};
	public static List<string> shields = new List<string>() {"greenShield", "blueShield"};
	
	public static Dictionary<int, string> playersMapping;// {1:xbox1, etc}

	public static List<string> listKarts = new List<string>{"Crash","Coca","Crash","Crash"};
	public static List<string> characters = new List<string>() {"kartCrash", "kartCoco", "kartCoca"};
	public static List<string> listMap = new List<string>() {"parking", "plage"};
	public static List<string> listMapForMenu = new List<string>() {"Skull Rock","Parking"};


}

