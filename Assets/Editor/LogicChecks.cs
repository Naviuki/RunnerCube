using System;
using RunnerCube.Core;
using RunnerCube.Data;
using RunnerCube.Player;
using RunnerCube.Track;

namespace RunnerCube.Tests
{
    /// <summary>
    /// Проверки логики без сцены: состояние, счёт, полосы, правила встреч и скорость.
    /// </summary>
    public static class LogicChecks
    {
        private static int _failed;

        public static void Run()
        {
            _failed = 0;

            StateGoesOnlyForward();
            ScoreDoubleWithStar();
            LanesFit();
            PickupsAreNotObstacles();
            SpeedStopsAtLimit();
            PauseDoesNotStartFromMenu();

            if (_failed > 0)
                throw new InvalidOperationException($"Logic checks failed: {_failed}");

            UnityEngine.Debug.Log("LOGIC CHECKS PASSED: 6");
        }

        private static void Check(bool condition, string name)
        {
            if (condition)
                return;

            _failed++;
            UnityEngine.Debug.LogError("LOGIC CHECK FAILED: " + name);
        }

        private static void StateGoesOnlyForward()
        {
            GameLoop loop = new GameLoop();
            Check(!loop.Pause(), "pause from menu");
            Check(loop.Begin(), "begin");
            Check(loop.Pause() && loop.IsPaused, "pause");
            Check(!loop.Begin(), "begin from pause");
            Check(loop.Resume(), "resume");
            Check(loop.End() && loop.State == GameState.GameOver, "end");
            Check(!loop.TogglePause(), "toggle from game over");
            Check(loop.Begin(), "restart from game over");
        }

        private static void ScoreDoubleWithStar()
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
            Check(Lanes.ToX(0, 4f) == -4f, "left lane");
            Check(Lanes.ToX(1, 4f) == 0f, "middle lane");
            Check(Lanes.ToX(2, 4f) == 4f, "right lane");
        }

        private static void PickupsAreNotObstacles()
        {
            Check(TrackItemRules.IsPickup(TrackItemKind.Coin), "coin is pickup");
            Check(!TrackItemRules.IsObstacle(TrackItemKind.Coin), "coin is not obstacle");
            Check(TrackItemRules.PassesUnder(TrackItemKind.LowBarrier), "passes under barrier");
            Check(!TrackItemRules.PassesUnder(TrackItemKind.BigLog), "no pass under log");
        }

        private static void SpeedStopsAtLimit()
        {
            RunSettings settings = UnityEngine.ScriptableObject.CreateInstance<RunSettings>();
            RunSpeed speed = new RunSpeed(settings);
            speed.Force(10000f);
            Check(speed.Current == settings.SpeedCheatLimit, "speed clamp");

            speed.Force(0f);
            speed.Grow(1f);
            Check(speed.Current > 0f, "speed grows");
        }

        private static void PauseDoesNotStartFromMenu()
        {
            GameLoop loop = new GameLoop();
            Check(!loop.TogglePause(), "toggle from menu");
            Check(loop.State == GameState.Menu, "state stays menu");
        }
    }
}
