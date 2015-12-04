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

        string projectName = buildFile.Split('.')[0];

        string sourceDirectory = Application.dataPath + Path.AltDirectorySeparatorChar + "Config";
        string finalTarget = buildDirectory + projectName + "_Data" + Path.AltDirectorySeparatorChar + "Config"; ;

        CopyDirectory(sourceDirectory, targetDirectory);

        sourceDirectory = Application.dataPath + Path.AltDirectorySeparatorChar + "Donation";
        finalTarget = buildDirectory + projectName + "_Data" + Path.AltDirectorySeparatorChar + "Donation"; ;

        CopyDirectory(sourceDirectory, targetDirectory);
    }

    private static void CopyDirectory(string sourceDirectory, string targetDirectory)
    {
        Directory.CreateDirectory(targetDirectory);

        foreach (var file in sourceDirectory.GetFiles())
        {
            File.Copy(file, Path.Combine(targetDirectory, Path.GetFileName(file)), true);
        }
    }
}
