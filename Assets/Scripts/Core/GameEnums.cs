namespace RunnerCube.Core
{
    /// <summary>
    /// Типы наград и препятствий. Общие для логики, данных и UI.
    /// </summary>
    public enum PickupKind
    {
        Coin,
        Shield,
        Star,
        Dumbbell
    }

    public enum ObstacleKind
    {
        Log,
        LowBarrier
    }

    public enum BonusKind
    {
        Shield,
        Star
    }

    public static class GameConstants
    {
        public const string MenuSceneName = "Menu";
        public const string GameSceneName = "Game";
        public const int CoinValue = 1;
        public const int StarMultiplier = 2;
    }
}
