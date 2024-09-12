using UnityEditor;

namespace LoopingBee.Shared
{
    public class LoopingBeeBuilder
    {
        [MenuItem("LoopingBee/Build WebGL")]
        public static void BuildWebGL()
        {
            BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, "Builds/WebGL", BuildTarget.WebGL, BuildOptions.None);
        }
    }
}