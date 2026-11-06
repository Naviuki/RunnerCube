using UnityEngine;
using RunnerCube.Core;
using RunnerCube.Data;

namespace RunnerCube.Player
{
    /// <summary>
    /// Щит и звезда с таймерами. Время идёт только во время забега.
    /// </summary>
    public sealed class PlayerBonuses : MonoBehaviour
    {
        [SerializeField] private RunSettings settings;
        [SerializeField] private PlayerMotor motor;
        [SerializeField] private GameEffects effects;
        [SerializeField] private GameObject shieldEffect;
        [SerializeField] private GameObject shieldBreakEffect;
        [SerializeField] private GameObject starEffect;
        [SerializeField] private GameObject dumbbellEffect;

        private RunSettings Config => settings != null ? settings : RunSettings.Default;

        private float _shieldLeft;
        private float _starLeft;

        public float ShieldFill => Config.ShieldDuration <= 0f ? 0f : _shieldLeft / Config.ShieldDuration;
        public float StarFill => Config.StarDuration <= 0f ? 0f : _starLeft / Config.StarDuration;
        public bool HasShield => _shieldLeft > 0f;
        public bool HasStar => _starLeft > 0f;

        private void Update()
        {
            if (SceneServices.Instance == null || !SceneServices.Instance.Session.IsPlaying)
                return;

            TickShield();
            TickStar();
        }

        public void GrantShield()
        {
            _shieldLeft = Config.ShieldDuration;
            if (motor != null)
                motor.SetShield(true);

            Play(shieldEffect);
            SceneServices.Instance.Session.RaiseBonusStarted(BonusKind.Shield, Config.ShieldDuration);
        }

        public void GrantStar()
        {
            _starLeft = Config.StarDuration;
            SceneServices.Instance.Session.SetMultiplier(GameConstants.StarMultiplier);
            Play(starEffect);
            SceneServices.Instance.Session.RaiseBonusStarted(BonusKind.Star, Config.StarDuration);
        }

        public void EatDumbbell()
        {
            Play(dumbbellEffect);
        }

        public void BreakShield()
        {
            if (_shieldLeft <= 0f)
                return;

            _shieldLeft = 0f;
            if (motor != null)
                motor.SetShield(false);

            Play(shieldBreakEffect);
            SceneServices.Instance.Session.RaiseBonusEnded(BonusKind.Shield);
        }

        private void Play(GameObject effect)
        {
            if (effects != null)
                effects.Play(effect, transform.position);
        }

        private void TickShield()
        {
            if (_shieldLeft <= 0f)
                return;

            _shieldLeft -= Time.deltaTime;
            if (_shieldLeft <= 0f)
                BreakShield();
        }

        private void TickStar()
        {
            if (_starLeft <= 0f)
                return;

            _starLeft -= Time.deltaTime;
            if (_starLeft > 0f)
                return;

            _starLeft = 0f;
            SceneServices.Instance.Session.SetMultiplier(1);
            SceneServices.Instance.Session.RaiseBonusEnded(BonusKind.Star);
        }
    }
}
