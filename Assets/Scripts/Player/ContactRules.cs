using UnityEngine;
using RunnerCube.Core;
using RunnerCube.Data;

namespace RunnerCube.Player
{
    /// <summary>
    /// Правила встреч с предметами и таймеров бонусов, вынесенные из MonoBehaviour.
    /// </summary>
    public static class ContactRules
    {
        public static bool BlocksDamage(bool shielded) => !shielded;

        public static bool ConsumesShield(TrackItemKind obstacle) => !TrackItemRules.PassesUnder(obstacle);

        public static PickupKind PickupOf(TrackItemKind kind) => TrackItemRules.ToPickup(kind);
    }

    /// <summary>
    /// Арифметика таймеров бонусов.
    /// </summary>
    public static class BonusRules
    {
        public static float ClampLeft(float left, float delta) => Mathf.Max(0f, left - Mathf.Max(0f, delta));

        public static float Fill(float left, float duration) => duration <= 0f ? 0f : Mathf.Clamp01(left / duration);
    }
}
