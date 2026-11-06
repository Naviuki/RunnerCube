using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace RunnerCube.Tests
{
    /// <summary>
    /// Значения настроек в сценах и ассетах: скорость, полосы и размер плитки не нулевые.
    /// </summary>
    public static class AssetChecks
    {
        private static int _failed;

        public static void Run()
        {
            _failed = 0;

            Data.RunSettings run = AssetDatabase.LoadAssetAtPath<Data.RunSettings>("Assets/Scripts/Data/Settings/RunSettings.asset");
            Check(run != null && run.StartSpeed > 0f && run.LineWidth > 0f, "RunSettings");

            Data.TrackSettings track = AssetDatabase.LoadAssetAtPath<Data.TrackSettings>("Assets/Scripts/Data/Settings/TrackSettings.asset");
            Check(track != null && track.TileLength > 0f && track.RoadTiles.Count > 0, "TrackSettings");

            if (_failed > 0)
                throw new System.InvalidOperationException($"Asset checks failed: {_failed}");

            Debug.Log("ASSET CHECKS PASSED");
        }

        private static void Check(bool value, string name)
        {
            if (value)
                return;

            _failed++;
            Debug.LogError("ASSET CHECK FAILED: " + name);
        }
    }
}
