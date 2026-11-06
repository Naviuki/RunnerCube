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

        private float _shieldLeft;
        private float _starLeft;

        public float ShieldFill => settings.ShieldDuration <= 0f ? 0f : _shieldLeft / settings.ShieldDuration;
        public float StarFill => settings.StarDuration <= 0f ? 0f : _starLeft / settings.StarDuration;
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
            _shieldLeft = settings.ShieldDuration;
            motor.SetShield(true);
            effects.Play(shieldEffect, transform.position);
            SceneServices.Instance.Session.RaiseBonusStarted(BonusKind.Shield, settings.ShieldDuration);
        }

        public void GrantStar()
        {
            _starLeft = settings.StarDuration;
            SceneServices.Instance.Session.SetMultiplier(GameConstants.StarMultiplier);
            effects.Play(starEffect, transform.position);
            SceneServices.Instance.Session.RaiseBonusStarted(BonusKind.Star, settings.StarDuration);
        }

        public void EatDumbbell() => effects.Play(dumbbellEffect, transform.position);

        public void BreakShield()
        {
            if (_shieldLeft <= 0f)
                return;

            _shieldLeft = 0f;
            motor.SetShield(false);
            effects.Play(shieldBreakEffect, transform.position);
            SceneServices.Instance.Session.RaiseBonusEnded(BonusKind.Shield);
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
