using UnityEngine;
using RunnerCube.Data;

namespace RunnerCube.Player
{
    /// <summary>
    /// Поза и скорость бегуна. Ничего не знает про анимации и коллайдер.
    /// </summary>
    public sealed class PlayerState
    {
        private readonly RunSettings _settings;

        public PlayerState(RunSettings settings)
        {
            _settings = settings != null ? settings : RunSettings.Default;
            Speed = new RunSpeed(_settings);
            Pose = PlayerPose.Running;
        }

        public PlayerPose Pose { get; private set; }

        public RunSpeed Speed { get; }

        public float SlideLeft { get; private set; }

        public bool Shielded { get; private set; }

        public bool Dead => Pose == PlayerPose.Dead;

        public bool Sliding => SlideLeft > 0f;

        public void Reset()
        {
            Pose = PlayerPose.Running;
            SlideLeft = 0f;
            Shielded = false;
            Speed.Reset();
        }

        public void Tick(float deltaTime)
        {
            if (Dead || deltaTime <= 0f)
                return;

            Speed.Grow(deltaTime);
            if (SlideLeft <= 0f)
                return;

            SlideLeft -= deltaTime;
            if (SlideLeft <= 0f)
            {
                SlideLeft = 0f;
                Pose = PlayerPose.Running;
            }
        }

        public bool CanJump(bool onGround) => !Dead && onGround && !Sliding;

        public void Jump() => Pose = PlayerPose.Jumping;

        public void Land()
        {
            if (!Sliding && !Dead)
                Pose = PlayerPose.Running;
        }

        public bool StartSlide()
        {
            if (Dead)
                return false;

            SlideLeft = _settings.SlideDuration;
            Pose = PlayerPose.Sliding;
            return true;
        }

        public void SetShield(bool value) => Shielded = value;

        public void Kill() => Pose = PlayerPose.Dead;
    }

    public enum PlayerPose
    {
        Running,
        Jumping,
        Sliding,
        Dead
    }
}
