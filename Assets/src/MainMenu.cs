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
		Application.LoadLevelAdditive("commonScene");
		InitMenus ();
	}

	void InitController()
	{
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
}
