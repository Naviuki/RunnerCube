using UnityEngine;

namespace RunnerCube.Player
{
    /// <summary>
    /// Включает визуал щита и звезды вокруг игрока.
    /// </summary>
    public sealed class PlayerAura : MonoBehaviour
    {
        [SerializeField] private GameObject shield;
        [SerializeField] private GameObject star;

        public void SetShield(bool value) => shield.SetActive(value);

        public void SetStar(bool value) => star.SetActive(value);

        public void HideAll()
        {
            shield.SetActive(false);
            star.SetActive(false);
        }
    }
}
