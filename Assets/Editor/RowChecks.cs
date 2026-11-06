using System;
using RunnerCube.Core;
using RunnerCube.Data;
using RunnerCube.Track;

namespace RunnerCube.Tests
{
    /// <summary>
    /// Проверки сборки ряда предметов с разными настройками.
    /// </summary>
    public static class RowChecks
    {
        private static int _failed;

        public static void Run()
        {
            _failed = 0;

            TrackSettings settings = UnityEngine.ScriptableObject.CreateInstance<TrackSettings>();
            RowBuilder builder = new RowBuilder(settings);

            for (int i = 0; i < 200; i++)
            {
                ArraySegment<PlacedItem> row = new ArraySegment<PlacedItem>(builder.Build(i).ToArray());
                Check(row.Count <= 2, "row has at most two items");
                foreach (PlacedItem item in row)
                    Check(item.Lane >= 0 && item.Lane < Lanes.Count, "lane in range");
            }

            if (_failed > 0)
                throw new InvalidOperationException($"Row checks failed: {_failed}");

            UnityEngine.Debug.Log("ROW CHECKS PASSED");
        }

        private static void Check(bool condition, string name)
        {
            if (condition)
                return;

            _failed++;
            UnityEngine.Debug.LogError("ROW CHECK FAILED: " + name);
        }
    }
}
