using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
	public static float thresholdAxis = 0.3f;
	// src : http://crashbandicoot.wikia.com/wiki/Crash_Team_Racing
	public static Dictionary <int, string> normalWeapons =  new Dictionary<int, string> {
		{1,"greenBeaker"},{2,"greenShield"},{3,"bomb"},{4,"triple_bomb"},{5,"missile"},{6,"triple_missile"},
		{7,"Aku-Aku"},{8,"TNT"},{9,"turbo"}	};
	public static Dictionary <int, string> superWeapons = new Dictionary<int, string> {
		{1,"redBeaker"},{2,"blueShield"},{3,"bomb"},{4,"triple_bomb"},{5,"missile"},{6,"triple_missile"},
		{7,"Aku-Aku"},{8,"nitro"},{9,"turbo"}	};
	public static List <string> gameWeapons =  new List <string> {
		"greenBeaker","greenShield","bomb","triple_bomb","triple_missile","Aku-Aku","TNT","turbo"};

	public static Dictionary <string, string> translateNameWeapons =  new Dictionary<string, string> {
		{"Potion","greenBeaker"},{"Bouclier","greenShield"},{"Bombe","bomb"},{"Bombe x 3","triple_bomb"},{"Missile","missile"},{"Missile x 3","triple_missile"},
		{"Aku-Aku","Aku-Aku"},{"TNT","TNT"},{"Turbo","turbo"}	};
	public static Dictionary <string, string> translateNameMaps =  new Dictionary<string, string> {
		{"Skull Rock","plage"},{"Parking","parking"}	};

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
	public static int nbPoints =8;

	public static List<string> listKarts = new List<string>{"Crash","Coco"};
	public static List<string> characters = new List<string>() {"kartCrash", "kartCoco"};
	public static List<string> listMap = new List<string>() {"plage", "parking"};
	public static List<string> listMapForMenu = new List<string>() {"Skull Rock","Parking"};

	public static int GetWeaponNumber(string w){
		for(int k=1;k<Game.normalWeapons.Count+1;k++)
			if (Game.normalWeapons[k] == w)
				return k;
		return -1;
	}
	
	public static string GetWeaponSuper(string w){
		return superWeapons[GetWeaponNumber(w)];
	}


}

