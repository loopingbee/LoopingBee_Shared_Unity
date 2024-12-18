using UnityEditor;

namespace LoopingBee.Shared
{
    public class LoopingBeeBuilder
    {
        [MenuItem("LoopingBee/Build WebGL")]
        public static void BuildWebGL()
        {
            UnityEditor.WebGL.UserBuildSettings.codeOptimization = UnityEditor.WebGL.WasmCodeOptimization.RuntimeSpeedLTO;
            BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, "Builds/WebGL", BuildTarget.WebGL, BuildOptions.None);
        }
    }
}