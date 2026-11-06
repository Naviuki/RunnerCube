namespace RunnerCube.Core
{
    /// <summary>
    /// Игровые клавиши одним списком, чтобы логика не знала про KeyCode.
    /// </summary>
    public static class InputKeys
    {
        public const UnityEngine.KeyCode Left = UnityEngine.KeyCode.A;
        public const UnityEngine.KeyCode Right = UnityEngine.KeyCode.D;
        public const UnityEngine.KeyCode Jump = UnityEngine.KeyCode.Space;
        public const UnityEngine.KeyCode Slide = UnityEngine.KeyCode.S;
        public const UnityEngine.KeyCode Pause = UnityEngine.KeyCode.Escape;
    }
}
