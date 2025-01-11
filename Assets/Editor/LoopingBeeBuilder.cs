using UnityEditor;
using UnityEngine;

namespace LoopingBee.Shared
{
    public class LoopingBeeBuilder
    {
        static void SetupBuild()
        {
            PlayerSettings.defaultWebScreenWidth = 960;
            PlayerSettings.defaultWebScreenHeight = 600;
            PlayerSettings.runInBackground = false;
            PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Brotli;
            PlayerSettings.SplashScreen.show = false;
            PlayerSettings.WebGL.template = "PROJECT:Looping Bee";
            QualitySettings.SetQualityLevel(1, true);
        }

        [MenuItem("LoopingBee/Build WebGL")]
        public static void BuildWebGL()
        {
            SetupBuild();
            UnityEditor.WebGL.UserBuildSettings.codeOptimization = UnityEditor.WebGL.WasmCodeOptimization.RuntimeSpeedLTO;
            BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, "Builds/WebGL", BuildTarget.WebGL, BuildOptions.None);
        }

        [MenuItem("LoopingBee/Build WebGL Quick")]
        public static void BuildWebGLQuick()
        {
            SetupBuild();
            UnityEditor.WebGL.UserBuildSettings.codeOptimization = UnityEditor.WebGL.WasmCodeOptimization.BuildTimes;
            BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, "Builds/WebGL", BuildTarget.WebGL, BuildOptions.None);
        }
    }
}