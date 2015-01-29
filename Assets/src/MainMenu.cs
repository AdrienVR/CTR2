using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
	public Texture normal;
	public Texture hover;

	// Use this for initialization
	void Start ()
	{
		InitController ();
		InitMenus ();
	}

	void InitController()
	{
		Instantiate (Resources.Load ("videoFond"));
		ControllerAPI.InitJoysticks ();
		ControllerAPI controller = new ControllerAPI (1);
		KartController.stop = true;
	}

	void InitMenus()
	{
		Menus m =(Menus)gameObject.AddComponent ("Menus");
		m.startMainMenu ();
		m.normal = normal;
		m.hover = hover;

	}
	// Update is called once per frame
	void Update ()
	{
		ControllerAPI.CheckJoysticks ();	
	}
}
