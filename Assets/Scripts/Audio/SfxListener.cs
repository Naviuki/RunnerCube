using UnityEngine;
using RunnerCube.Core;

namespace RunnerCube.Audio
{
    /// <summary>
    /// Подписывается на игровые события и проигрывает нужный звук.
    /// </summary>
    public sealed class SfxListener : MonoBehaviour
    {
        [SerializeField] private SfxPlayer player;

        private GameEvents _events;

        private void Start()
        {
            _events = SceneServices.Instance.Events;
            _events.PickupCollected += OnPickup;
            _events.BonusStarted += OnBonusStarted;
            _events.BonusEnded += OnBonusEnded;
            _events.StateChanged += OnState;
        }

        private void OnDestroy()
        {
            if (_events == null)
                return;

            _events.PickupCollected -= OnPickup;
            _events.BonusStarted -= OnBonusStarted;
            _events.BonusEnded -= OnBonusEnded;
            _events.StateChanged -= OnState;
        }

        private void OnPickup(PickupKind kind)
        {
            switch (kind)
            {
                case PickupKind.Coin:
                    player.PlayCoin();
                    break;
                case PickupKind.Shield:
                    player.PlayShieldOn();
                    break;
                case PickupKind.Star:
                    player.PlayStar();
                    break;
                case PickupKind.Dumbbell:
                    player.PlayDumbbell();
                    break;
            }
        }

        private void OnBonusStarted(BonusKind kind, float seconds)
        {
            if (kind == BonusKind.Star)
                player.PlayStar();
        }

        private void OnBonusEnded(BonusKind kind)
        {
            if (kind == BonusKind.Shield)
                player.PlayShieldBreak();
        }

        private void OnState(GameState state)
        {
            if (state == GameState.GameOver)
                player.PlayDeath();
        }
    }
}
