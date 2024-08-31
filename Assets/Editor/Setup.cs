using System.IO;
using UnityEditor;
using UnityEngine;

namespace LoopingBee.Shared
{
    public class Setup
    {
        [MenuItem("LoopingBee/Setup")]
        public static void SetupLoopingBee()
        {
            Debug.Log("Setting up LoopingBee");
            var originPath = Application.dataPath + "/../Library/PackageCache/com.loopingbee.shared/.template";
            Debug.Log("Origin path: " + originPath);
            var targetPath = Application.dataPath + "/WebGLTemplates";
            Debug.Log("Target path: " + targetPath);

            if (!Directory.Exists(targetPath))
            {
                Debug.Log("Creating target directory");
                Directory.CreateDirectory(targetPath);
            }

            // Use bash to create a sym link
            var process = new System.Diagnostics.Process();
            process.StartInfo.FileName = "/bin/bash";
            process.StartInfo.Arguments = $"-c \"ln -s {originPath} {targetPath}\"";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;

            Debug.Log("Creating symlink");
            process.Start();

            // Rename the symlink to the correct name "WebGLTemplate"
            process = new System.Diagnostics.Process();
            process.StartInfo.FileName = "/bin/bash";
            process.StartInfo.Arguments = $"-c \"mv {targetPath}/.template {targetPath}/Looping\\ Bee\"";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;

            Debug.Log("Renaming symlink");
            process.Start();

            // Reimport the assets
            AssetDatabase.Refresh();
        }
    }
}