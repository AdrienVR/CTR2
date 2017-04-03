
using UnityEditor;
using UnityEngine;

public class Hotkeys
{
    [MenuItem("CTR2/Tools/Set scene cam as game cam #F1")]
    static void OpenMainMenu()
    {
        var selection = Selection.activeObject;
        var sceneCam = GameObject.FindObjectOfType<Camera>();
        sceneCam.transform.position += sceneCam.transform.forward * 0.1f;
        Selection.activeObject = sceneCam;
        EditorApplication.ExecuteMenuItem("GameObject/Align View to Selected");
        sceneCam.transform.position -= sceneCam.transform.forward * 0.1f;
        Selection.activeObject = selection;
    }

    [MenuItem("CTR2/Tools/Toggle Pause Editor \t#F2")]
    static void Pause()
    {
        EditorApplication.isPaused = !EditorApplication.isPaused;
    }

    [MenuItem("CTR2/Tools/Enter playmode \t#F5")]
    static void Enter()
    {
        EditorApplication.isPlaying = true;
    }
}
