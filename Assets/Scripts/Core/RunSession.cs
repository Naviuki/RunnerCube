using UnityEngine;

namespace RunnerCube.Core
{
    /// <summary>
    /// Забег: состояние, счёт и монеты. Время паузы меняется только в одном месте.
    /// </summary>
    public sealed class RunSession
    {
        private readonly GameEvents _events;
        private readonly GameLoop _loop = new GameLoop();
        private readonly ScoreModel _score = new ScoreModel();

        public RunSession(GameEvents events)
        {
            _events = events;
            Progress = new PlayerProgress();
            _score.Changed += score => _events.RaiseScore(score);
        }

        public PlayerProgress Progress { get; }
        public GameState State => _loop.State;
        public bool IsPlaying => _loop.IsPlaying;
        public int Score => _score.Total;
        public int RunCoins { get; private set; }

        public void Begin()
        {
            RunCoins = 0;
            _score.Reset();
            if (_loop.Begin())
                ChangeTimeAndState(State);

            _events.RaiseCoins(RunCoins, Progress.Coins);
        }

        public void Tick(float deltaTime)
        {
            if (_loop.IsPlaying)
                _score.Tick(deltaTime);
        }

        public void End()
        {
            if (!_loop.End())
                return;

            Progress.RegisterRun(_score.Total);
            ChangeTimeAndState(State);
        }

        public void TogglePause()
        {
            if (_loop.TogglePause())
                ChangeTimeAndState(State);
        }

        public void Pause()
        {
            if (_loop.Pause())
                ChangeTimeAndState(State);
        }

        public void Resume()
        {
            if (_loop.Resume())
                ChangeTimeAndState(State);
        }

        public void AddCoins(int amount)
        {
            RunCoins += amount;
            Progress.AddCoins(amount);
            _events.RaiseCoins(RunCoins, Progress.Coins);
        }

        public void SetCoins(int amount)
        {
            Progress.Coins = amount;
            _events.RaiseCoins(RunCoins, Progress.Coins);
        }

        public void SetMultiplier(int multiplier) => _score.SetMultiplier(multiplier);

        public void RaisePickup(PickupKind kind) => _events.RaisePickup(kind);
        public void RaiseObstacle(ObstacleKind kind) => _events.RaiseObstacle(kind);
        public void RaiseBonusStarted(BonusKind kind, float seconds) => _events.RaiseBonusStarted(kind, seconds);
        public void RaiseBonusEnded(BonusKind kind) => _events.RaiseBonusEnded(kind);

        private void ChangeTimeAndState(GameState state)
        {
            Time.timeScale = state == GameState.Paused ? 0f : 1f;
            _events.RaiseState(state);
        }
    }
}
