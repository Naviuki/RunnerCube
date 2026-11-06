using UnityEngine;
using RunnerCube.Core;
using RunnerCube.Data;
using RunnerCube.Track;

namespace RunnerCube.Player
{
    /// <summary>
    /// Ведёт игрока вперёд, переключает полосы и позы. Столкновения обрабатывает PlayerContact.
    /// </summary>
    public sealed class PlayerMotor : MonoBehaviour
    {
        [SerializeField] private RunSettings settings;
        [SerializeField] private PlayerBody body;
        [SerializeField] private PlayerView view;
        [SerializeField] private InputReader input;
        [SerializeField] private GameEffects effects;

        private PlayerState _state;
        private Vector3 _velocity;
        private int _lane = Lanes.Middle;
        private bool _wasSliding;

        public PlayerState State => _state;

        private RunSettings Config => settings != null ? settings : RunSettings.Default;

        private void Awake()
        {
            _state = new PlayerState(Config);
            if (body == null)
                body = GetComponent<PlayerBody>();
        }

        private void OnEnable()
        {
            if (input != null)
                input.Pressed += Handle;
        }

        private void OnDisable()
        {
            if (input != null)
                input.Pressed -= Handle;
        }

        private void FixedUpdate()
        {
            if (!CanMove)
                return;

            _velocity.z = _state.Speed.Current;
            _velocity.y += Config.Gravity * Time.fixedDeltaTime;
            if (body.OnGround && _velocity.y < 0f)
                _velocity.y = -1f;

            body.Move(_velocity * Time.fixedDeltaTime);
            KeepLane();

            if (body.OnGround)
                _state.Land();
        }

        private void Update()
        {
            if (!CanMove)
                return;

            _state.Tick(Time.deltaTime);

            bool sliding = _state.Sliding;
            if (sliding != _wasSliding)
            {
                body.SetSliding(sliding);
                _wasSliding = sliding;
            }

            if (view != null)
                view.Run(body.OnGround && !sliding);
        }

        public void ResetRun()
        {
            _state.Reset();
            _velocity = Vector3.zero;
            _lane = Lanes.Middle;
            _wasSliding = false;
            body.Stand();

            if (view != null)
                view.Run(true);
        }

        public void SetShield(bool value) => _state.SetShield(value);

        public void Kill()
        {
            _state.Kill();
            if (view != null)
                view.Die();
        }

        public void EatDumbbell() => _state.Speed.SlowDownByDumbbell();

        private bool CanMove => SceneServices.Instance != null && SceneServices.Instance.Session.IsPlaying && !_state.Dead;

        private void KeepLane()
        {
            float targetX = Lanes.ToX(_lane, Config.LineWidth);
            Vector3 position = transform.position;
            position.x = Mathf.MoveTowards(position.x, targetX, Config.SidestepSpeed * Time.fixedDeltaTime);
            transform.position = position;
        }

        private void Handle(GameAction action)
        {
            if (!CanMove)
                return;

            if (action == GameAction.Left)
                _lane = Mathf.Max(0, _lane - 1);
            else if (action == GameAction.Right)
                _lane = Mathf.Min(Lanes.Count - 1, _lane + 1);
            else if (action == GameAction.Jump)
                TryJump();
            else if (action == GameAction.Slide && _state.StartSlide() && view != null)
                view.Slide();
        }

        private void TryJump()
        {
            if (!_state.CanJump(body.OnGround))
                return;

            _velocity.y = Config.JumpForce;
            _state.Jump();

            if (view != null)
                view.Jump();
        }
    }
}
