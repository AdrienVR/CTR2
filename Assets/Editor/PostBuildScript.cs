// C# example:
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;

public class PostBuildScript
{
    [PostProcessBuildAttribute(1)]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        string[] splittedPath = pathToBuiltProject.Split(Path.AltDirectorySeparatorChar);
        string buildFile = splittedPath[splittedPath.Length - 1];
        string buildDirectory = pathToBuiltProject.Replace(buildFile, "");

        string name = buildFile.Split('.')[0];
        string configTarget = name + "_Data" + Path.AltDirectorySeparatorChar + "Config";

        string finalTarget = buildDirectory + configTarget;

        Directory.CreateDirectory(finalTarget);

        foreach (var file in Directory.GetFiles(Application.dataPath + Path.AltDirectorySeparatorChar + "Config"))
            File.Copy(file, Path.Combine(finalTarget, Path.GetFileName(file)), true);
    }
}
