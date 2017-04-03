using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class EditorMenuCTR2
{
    // Add a menu item named "Do Something" to MyMenu in the menu bar.
    [MenuItem("CTR2/Open the Main Menu")]
    static void OpenMainMenu()
    {
        EditorSceneManager.OpenScene("Assets/Maps/mainmenu.unity");
    }

    [MenuItem("CTR2/Open the beach scene")]
    static void OpenBeach()
    {
        EditorSceneManager.OpenScene("Assets/Maps/plage.unity");
    }

    [MenuItem("CTR2/Open the parking scene")]
    static void OpenParking()
    {
        EditorSceneManager.OpenScene("Assets/Maps/parking.unity");
    }

    [MenuItem("CTR2/Open prefabs scene")]
    static void OpenPrefabs()
    {
        EditorSceneManager.OpenScene("Assets/Maps/prefabs.unity");
    }

    [MenuItem("Documentation/CTR2 on Github")]
    public static void LinkToWeb()
    {
        Application.OpenURL("https://github.com/AdrienVR/CTR2");
    }
}