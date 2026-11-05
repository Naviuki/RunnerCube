using System;

namespace RunnerCube.Core
{
    /// <summary>
    /// Шина событий для UI, звука и эффектов. Подписчики не знают друг о друге.
    /// </summary>
    public sealed class GameEvents
    {
        public event Action<GameState> StateChanged;
        public event Action<int> ScoreChanged;
        public event Action<int, int> CoinsChanged;
        public event Action<PickupKind> PickupCollected;
        public event Action<ObstacleKind> ObstacleHit;
        public event Action<BonusKind, float> BonusStarted;
        public event Action<BonusKind> BonusEnded;
        public event Action<string> CheatMessage;

        public void RaiseState(GameState state) => StateChanged?.Invoke(state);
        public void RaiseScore(int score) => ScoreChanged?.Invoke(score);
        public void RaiseCoins(int runCoins, int total) => CoinsChanged?.Invoke(runCoins, total);
        public void RaisePickup(PickupKind kind) => PickupCollected?.Invoke(kind);
        public void RaiseObstacle(ObstacleKind kind) => ObstacleHit?.Invoke(kind);
        public void RaiseBonusStarted(BonusKind kind, float seconds) => BonusStarted?.Invoke(kind, seconds);
        public void RaiseBonusEnded(BonusKind kind) => BonusEnded?.Invoke(kind);
        public void RaiseCheatMessage(string message) => CheatMessage?.Invoke(message);
    }
}
