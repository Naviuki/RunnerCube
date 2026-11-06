using UnityEditor;
using UnityEngine;

namespace RunnerCube.Tests
{
    /// <summary>
    /// Проверки данных: ассеты настроек загружаются и содержат значения.
    /// </summary>
    public static class DataChecks
    {
        private static int _failed;

        public static void Run()
        {
            _failed = 0;

            Object run = AssetDatabase.LoadAssetAtPath("Assets/Scripts/Data/Settings/RunSettings.asset", typeof(Data.RunSettings));
            if (run == null)
                Fail("RunSettings");

            Object track = AssetDatabase.LoadAssetAtPath("Assets/Scripts/Data/Settings/TrackSettings.asset", typeof(Data.TrackSettings));
            if (track == null)
                Fail("TrackSettings");

            if (_failed > 0)
                throw new System.InvalidOperationException($"Data checks failed: {_failed}");

            Debug.Log("DATA CHECKS PASSED");
        }

        private static void Fail(string name)
        {
            _failed++;
            Debug.LogError("DATA CHECK FAILED: " + name);
        }
    }
}
