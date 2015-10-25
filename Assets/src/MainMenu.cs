using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public Texture normal;
    public Texture hover;

    // Use this for initialization
    void Awake()
    {
        InitController();

        InitMenus();

#if UNITY_EDITOR
        Application.runInBackground = true;
#endif
    }

    void InitController()
    {
        KartController.stop = true;
    }

    void InitMenus()
    {
        Instantiate(Resources.Load("videoFond"));
        Menus menu = (Menus)gameObject.AddComponent<Menus>();
        menu.displayMenu(Menus.menuToGo);
        menu.normal = normal;
        menu.hover = hover;

    }
}
