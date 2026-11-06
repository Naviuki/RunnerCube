using UnityEditor;
using UnityEngine;

namespace RunnerCube.EditorTools
{
    /// <summary>
    /// Собирает Windows-билд из меню Tools/Runner Cube. Папка Builds в git не попадает.
    /// </summary>
    public static class BuildScript
    {
        private const string Output = "Builds/Windows/RunnerCube.exe";
        private const string MenuPath = "Tools/Runner Cube/Build Windows";

        [MenuItem(MenuPath)]
        public static void BuildWindows()
        {
            PlayerSettings.productName = "RunnerCube";
            PlayerSettings.companyName = "UKEI UG";

            BuildPlayerOptions options = new BuildPlayerOptions
            {
                scenes = new[] { "Assets/_Scenes/Menu.unity", "Assets/_Scenes/Game.unity" },
                locationPathName = Output,
                target = BuildTarget.StandaloneWindows64,
                options = BuildOptions.None
            };

            UnityEditor.Build.Reporting.BuildReport report = BuildPipeline.BuildPlayer(options);
            Debug.Log($"BUILD RESULT result={report.summary.result} errors={report.summary.totalErrors} size={report.summary.totalSize}");

            if (report.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded)
                throw new UnityEditor.Build.BuildFailedException("Сборка не удалась, подробности в консоли.");
        }
    }
}
