using UnityEngine;
using RunnerCube.Data;

namespace RunnerCube.Player
{
    /// <summary>
    /// Скорость забега. Растёт по времени, падает от гантели, ограничена настройками.
    /// </summary>
    public sealed class RunSpeed
    {
        private readonly RunSettings _settings;

        public RunSpeed(RunSettings settings)
        {
            _settings = settings != null ? settings : RunSettings.Default;
            Current = _settings.StartSpeed;
        }

        public float Current { get; private set; }

        public void Reset() => Current = _settings.StartSpeed;

        public void Grow(float deltaTime)
        {
            if (deltaTime <= 0f || Current >= _settings.MaxSpeed)
                return;

            Current = Mathf.Min(Current + _settings.SpeedIncreasePerSecond * deltaTime, _settings.MaxSpeed);
        }

        public void SlowDownByDumbbell()
        {
            Current = Mathf.Max(0f, Current - _settings.DumbbellPenalty);
        }

        public void Force(float speed) => Current = Mathf.Clamp(speed, 0f, _settings.SpeedCheatLimit);
    }
}
