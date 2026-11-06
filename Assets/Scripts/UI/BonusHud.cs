using UnityEngine;
using RunnerCube.Player;

namespace RunnerCube.UI
{
    /// <summary>
    /// Показывает кружки бонусов из таймеров игрока. Без ссылок просто не рисует ничего.
    /// </summary>
    public sealed class BonusHud : MonoBehaviour
    {
        [SerializeField] private PlayerBonuses bonuses;
        [SerializeField] private GameObject shieldIcon;
        [SerializeField] private FillView shieldFill;
        [SerializeField] private GameObject starIcon;
        [SerializeField] private FillView starFill;

        private void Update()
        {
            if (bonuses == null)
                return;

            Set(shieldIcon, shieldFill, bonuses.HasShield, bonuses.ShieldFill);
            Set(starIcon, starFill, bonuses.HasStar, bonuses.StarFill);
        }

        private static void Set(GameObject icon, FillView fill, bool active, float value)
        {
            if (icon != null && icon.activeSelf != active)
                icon.SetActive(active);

            if (fill != null)
                fill.SetFill(value);
        }
    }
}
