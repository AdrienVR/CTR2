using UnityEditor;

public class EditorMenuCTR2
{

    // Add a menu item named "Do Something" to MyMenu in the menu bar.
    [MenuItem("CTR2/Open the Main Menu")]
    static void OpenMainMenu()
    {
        EditorApplication.OpenScene("Assets/maps/mainmenu.unity");
    }

    [MenuItem("CTR2/Open the beach scene")]
    static void OpenBeach()
    {
        EditorApplication.OpenScene("Assets/maps/plage.unity");
    }
}