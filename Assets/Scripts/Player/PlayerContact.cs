using UnityEngine;
using RunnerCube.Core;
using RunnerCube.Data;
using RunnerCube.Track;

namespace RunnerCube.Player
{
    /// <summary>
    /// Обрабатывает встречи игрока с предметами дороги и завершение забега.
    /// </summary>
    public sealed class PlayerContact : MonoBehaviour
    {
        [SerializeField] private PlayerMotor motor;
        [SerializeField] private PlayerBonuses bonuses;
        [SerializeField] private GameEffects effects;
        [SerializeField] private RunSettings settings;
        [SerializeField] private GameObject deathEffect;

        private bool _dead;

        private RunSettings Config => settings != null ? settings : RunSettings.Default;

        private void Awake()
        {
            if (motor == null)
                motor = GetComponent<PlayerMotor>();

            if (bonuses == null)
                bonuses = GetComponent<PlayerBonuses>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_dead || !other.TryGetComponent(out TrackItemMarker item) || !item.IsPickup)
                return;

            TakePickup(item);
            item.Collect();
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (_dead || hit.collider.isTrigger)
                return;

            if (hit.collider.TryGetComponent(out TrackItemMarker item) && item.IsObstacle)
                HitObstacle(item);
        }

        private void TakePickup(TrackItemMarker item)
        {
            RunSession session = SceneServices.Instance.Session;
            PickupKind kind = item.ToPickup();

            switch (kind)
            {
                case PickupKind.Coin:
                    session.AddCoins(GameConstants.CoinValue);
                    if (effects != null)
                        effects.PlayCoin(transform.position);
                    break;
                case PickupKind.Shield:
                    if (bonuses != null)
                        bonuses.GrantShield();
                    break;
                case PickupKind.Star:
                    if (bonuses != null)
                        bonuses.GrantStar();
                    break;
                case PickupKind.Dumbbell:
                    if (motor != null)
                        motor.EatDumbbell();

                    if (bonuses != null)
                        bonuses.EatDumbbell();
                    break;
            }

            session.RaisePickup(kind);
        }

        private void HitObstacle(TrackItemMarker item)
        {
            SceneServices.Instance.Session.RaiseObstacle(item.ToObstacle());

            if (motor != null && motor.State.Shielded)
            {
                if (!TrackItemRules.PassesUnder(item.Kind) && bonuses != null)
                    bonuses.BreakShield();

                item.Collect();
                return;
            }

            Die();
        }

        private void Die()
        {
            if (_dead)
                return;

            _dead = true;
            if (effects != null)
                effects.Play(deathEffect, transform.position);

            if (motor != null)
                motor.Kill();

            CancelInvoke(nameof(FinishRun));
            Invoke(nameof(FinishRun), Config.DeathDelay);
        }

        private void FinishRun() => SceneServices.Instance.Session.End();
    }
}
