using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class EditorMenuCTR2
{
    // Add a menu item named "Do Something" to MyMenu in the menu bar.
    [MenuItem("CTR2/Open the Main Menu")]
    static void OpenMainMenu()
    {
        OpenScene("Assets/Maps/mainmenu.unity");
    }

    [MenuItem("CTR2/Open the beach scene")]
    static void OpenBeach()
    {
        OpenScene("Assets/Maps/plage.unity");
    }

    [MenuItem("CTR2/Open the parking scene")]
    static void OpenParking()
    {
        OpenScene("Assets/Maps/parking.unity");
    }

    [MenuItem("CTR2/Open prefabs scene")]
    static void OpenPrefabs()
    {
        OpenScene("Assets/Maps/prefabs.unity");
    }

    static void OpenScene(string _scene)
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene(_scene);
    }

    [MenuItem("Documentation/CTR2 on Github")]
    public static void LinkToWeb()
    {
        Application.OpenURL("https://github.com/AdrienVR/CTR2");
    }
}