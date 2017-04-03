
using UnityEditor;
using UnityEditor.Animations;

public class AnimatorPostProcessor : AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string str in importedAssets)
        {
            var controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(str);
            if (controller)
            {
                AnimEnumGenerator.GenerateAnimEnum(controller);
            }
        }
        foreach (string str in deletedAssets)
        {
            if (str.Contains(".controller"))
            {
                AnimEnumGenerator.DeleteEnum(System.IO.Path.GetFileNameWithoutExtension(str));
            }
        }
    }
}
