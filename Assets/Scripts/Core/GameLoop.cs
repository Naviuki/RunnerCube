namespace RunnerCube.Core
{
    /// <summary>
    /// Переходы состояния: меню или финал дают старт, из игры можно в паузу или финал, из паузы только назад в игру.
    /// </summary>
    public sealed class GameLoop
    {
        public GameState State { get; private set; } = GameState.Menu;

        public bool IsPlaying => State == GameState.Playing;
        public bool IsPaused => State == GameState.Paused;

        public bool Begin()
        {
            if (State == GameState.Playing || State == GameState.Paused)
                return false;

            State = GameState.Playing;
            return true;
        }

        public bool Pause()
        {
            if (State != GameState.Playing)
                return false;

            State = GameState.Paused;
            return true;
        }

        public bool Resume()
        {
            if (State != GameState.Paused)
                return false;

            State = GameState.Playing;
            return true;
        }

        public bool End()
        {
            if (State != GameState.Playing)
                return false;

            State = GameState.GameOver;
            return true;
        }

        public bool TogglePause()
        {
            if (State == GameState.Playing)
                return Pause();

            return State == GameState.Paused && Resume();
        }
    }
}
