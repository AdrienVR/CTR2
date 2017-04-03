
using System.IO;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class AnimEnumGenerator : MonoBehaviour
{
    public const string c_animEnumsPath = "Assets/src/AnimEnums/";

    [MenuItem("Assets/Generate Anim Enum")]
    static void GenerateAnimEnum()
    {
        GenerateAnimEnum(Selection.activeObject as AnimatorController);
    }

    static string GetName(string _fileName)
    {
        return _fileName.CapitalizeFirstLetter().Replace(" ", "") + "Anims";
    }

    public static void DeleteEnum(string _fileName)
    {
        string name = GetName(_fileName);
        string path = c_animEnumsPath + name + ".cs";

        if (File.Exists(path))
            AssetDatabase.DeleteAsset(path);
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }

    public static void GenerateAnimEnum(AnimatorController _controller)
    {
        string name = GetName(_controller.name);
        string path = c_animEnumsPath + name + ".cs";

        string enumStr = "public enum " + name + " : int\n{\n";
        foreach (var layer in _controller.layers)
        {
            AddStates(layer.stateMachine, ref enumStr);
        }
        enumStr += "}";

        File.WriteAllText(path, enumStr);
        AssetDatabase.ImportAsset(path);
    }

    static void AddStates(AnimatorStateMachine _stateMachine, ref string _enumStr, string subStateName = null)
    {
        foreach (var subState in _stateMachine.states)
        {
            string stateName = subState.state.name;
            string enumName = stateName.CapitalizeFirstLetter();
            if (subStateName != null)
            {
                stateName = subStateName + "." + stateName;
                enumName = subStateName + enumName;
            }
            enumName = enumName.Replace(" ", "");
            _enumStr += "\t" + enumName + " = " + Animator.StringToHash(stateName) + ",\n";
        }
        foreach (var sm in _stateMachine.stateMachines)
        {
            AddStates(sm.stateMachine, ref _enumStr, sm.stateMachine.name);
        }
    }

    [MenuItem("Assets/Generate Anim Enum", validate = true)]
    static bool GenerateAnimEnumValid()
    {
        var select = Selection.activeObject;
        return select is RuntimeAnimatorController;
    }
}
