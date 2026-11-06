using System;
using RunnerCube.Core;
using RunnerCube.Data;

namespace RunnerCube.Tests
{
    /// <summary>
    /// Мини-проверки логики, которым хватает обычного запуска Unity в batch-режиме.
    /// </summary>
    public static class LogicChecks
    {
        private static int _failed;

        public static void Run()
        {
            _failed = 0;

            PauseOnlyFromPlaying();
            ScoreGrowsWithMultiplier();
            LanesFit();
            PickupsAreNotObstacles();
            TrackStartIsClear();

            if (_failed > 0)
                throw new InvalidOperationException($"Logic checks failed: {_failed}");

            UnityEngine.Debug.Log("LOGIC CHECKS PASSED: 5");
        }

        private static void Check(bool condition, string name)
        {
            if (condition)
                return;

            _failed++;
            UnityEngine.Debug.LogError("LOGIC CHECK FAILED: " + name);
        }

        private static void PauseOnlyFromPlaying()
        {
            GameLoop loop = new GameLoop();
            Check(!loop.Pause(), "pause before begin");
            Check(loop.Begin(), "begin");
            Check(loop.Pause(), "pause");
            Check(loop.IsPaused, "is paused");
            Check(!loop.Begin(), "begin while paused");
            Check(loop.Resume(), "resume");
            Check(loop.End(), "end");
            Check(loop.State == GameState.GameOver, "game over");
            Check(!loop.TogglePause(), "toggle in game over");
        }

        private static void ScoreGrowsWithMultiplier()
        {
            ScoreModel score = new ScoreModel();
            score.Tick(1f);
            score.SetMultiplier(GameConstants.StarMultiplier);
            score.Tick(1f);
            Check(score.Total == 3, "score with star");

            score.Reset();
            Check(score.Total == 0 && score.Multiplier == 1, "score reset");
        }

        private static void LanesFit()
        {
            Check(Track.Lanes.ToX(0, 4f) < Track.Lanes.ToX(1, 4f), "lanes ordered");
            Check(Track.Lanes.ToX(1, 4f) == 0f, "middle lane centered");
        }

        private static void PickupsAreNotObstacles()
        {
            Check(TrackItemRules.IsPickup(TrackItemKind.Coin), "coin is pickup");
            Check(!TrackItemRules.IsObstacle(TrackItemKind.Coin), "coin is not obstacle");
            Check(TrackItemRules.IsObstacle(TrackItemKind.LowBarrier), "barrier is obstacle");
            Check(TrackItemRules.PassesUnder(TrackItemKind.LowBarrier), "passes under barrier");
            Check(!TrackItemRules.PassesUnder(TrackItemKind.BigLog), "no pass under log");
        }

        private static void TrackStartIsClear()
        {
            TrackSettings settings = UnityEngine.ScriptableObject.CreateInstance<TrackSettings>();
            Check(settings.FirstRowDistance > 60f, "rows start ahead of the player");
            Check(settings.TileLength > 0f, "tile length set");
        }
    }
}
