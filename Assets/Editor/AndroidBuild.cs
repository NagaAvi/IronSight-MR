using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.Build;

namespace IronSight.Editor
{
    public static class AndroidBuild
    {
        public static void BuildQuestDevelopmentApk()
        {
            var outputDirectory = Path.GetFullPath("Builds/Android");
            Directory.CreateDirectory(outputDirectory);

            var buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = new[]
                {
                    "Assets/Scenes/BootScene.unity",
                    "Assets/Scenes/MainScene.unity"
                },
                locationPathName = Path.Combine(outputDirectory, "IronSight-Quest-Dev.apk"),
                target = BuildTarget.Android,
                options = BuildOptions.Development
            };

            var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            if (report.summary.result != BuildResult.Succeeded)
            {
                throw new BuildFailedException($"Android build failed with result: {report.summary.result}");
            }
        }
    }
}
