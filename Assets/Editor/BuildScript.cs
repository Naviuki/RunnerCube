using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace RunnerCube.EditorTools
{
    /// <summary>
    /// Собирает Windows-плеер. Путь задаётся аргументом командной строки -buildPath.
    /// </summary>
    public static class BuildScript
    {
        [MenuItem("Tools/Runner Cube/Build Windows")]
        public static void BuildWindows()
        {
            BuildPlayerOptions options = new BuildPlayerOptions
            {
                scenes = new[] { "Assets/_Scenes/Menu.unity", "Assets/_Scenes/Game.unity" },
                locationPathName = OutputPath(),
                target = BuildTarget.StandaloneWindows64,
                options = BuildOptions.None
            };

            BuildReport report = BuildPipeline.BuildPlayer(options);
            Debug.Log($"BUILD RESULT result={report.summary.result} errors={report.summary.totalErrors} size={report.summary.totalSize}");

            if (report.summary.result != BuildResult.Succeeded)
                throw new UnityEditor.Build.BuildFailedException("Сборка не удалась, подробности в консоли.");
        }

        private static string OutputPath()
        {
            string[] args = System.Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length - 1; i++)
            {
                if (args[i] == "-buildPath")
                    return args[i + 1];
            }

            return "Builds/Windows/RunnerCube.exe";
        }
    }
}
