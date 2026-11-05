using System;

namespace RunnerCube.Core
{
    /// <summary>
    /// Очки за время забега. Множитель меняется звездой.
    /// </summary>
    public sealed class ScoreModel
    {
        private const float PointsPerSecond = 1f;

        private float _pending;

        public int Total { get; private set; }

        public int Multiplier { get; private set; } = 1;

        public event Action<int> Changed;

        public void Tick(float deltaTime)
        {
            if (deltaTime <= 0f)
                return;

            _pending += PointsPerSecond * Multiplier * deltaTime;
            int whole = (int)_pending;
            if (whole <= 0)
                return;

            _pending -= whole;
            Total += whole;
            Changed?.Invoke(Total);
        }

        public void SetMultiplier(int multiplier)
        {
            Multiplier = multiplier < 1 ? 1 : multiplier;
        }

        public void Reset()
        {
            Total = 0;
            Multiplier = 1;
            _pending = 0f;
            Changed?.Invoke(Total);
        }
    }
}
