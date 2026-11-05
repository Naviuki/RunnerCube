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
        [SerializeField] private RunSettings settings;

        private bool _dead;

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
                    break;
                case PickupKind.Shield:
                    bonuses.GrantShield();
                    break;
                case PickupKind.Star:
                    bonuses.GrantStar();
                    break;
                case PickupKind.Dumbbell:
                    motor.EatDumbbell();
                    break;
            }

            session.RaisePickup(kind);
        }

        private void HitObstacle(TrackItemMarker item)
        {
            ObstacleKind kind = item.ToObstacle();
            SceneServices.Instance.Session.RaiseObstacle(kind);

            if (motor.State.Shielded)
            {
                if (!TrackItemRules.PassesUnder(item.Kind))
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
            motor.Kill();
            Invoke(nameof(FinishRun), settings.DeathDelay);
        }

        private void FinishRun() => SceneServices.Instance.Session.End();
    }
}
