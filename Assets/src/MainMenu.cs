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
		ControllerAPI.InitJoysticks ();
		KartController.stop = true;
	}

	void InitMenus()
	{
		Instantiate (Resources.Load ("videoFond"));
		Menus m =(Menus)gameObject.AddComponent ("Menus");
		m.displayMenu(Menus.menuToGo);
		m.normal = normal;
		m.hover = hover;

	}
	// Update is called once per frame
	void FixedUpdate ()
	{
		ControllerAPI.CheckJoysticks ();	
	}
}
