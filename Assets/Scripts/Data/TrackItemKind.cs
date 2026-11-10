using UnityEngine;

namespace RunnerCube.Data
{
    public enum TrackItemKind
    {
        Coin,
        Shield,
        Star,
        Dumbbell,
        BigLog,
        Log,
        LowBarrier
    }

    /// <summary>
    /// Правила для предметов: что подбирается, что бьёт и какой тип у каждого варианта.
    /// </summary>
    public static class TrackItemRules
    {
        public static bool IsPickup(TrackItemKind kind)
        {
            switch (kind)
            {
                case TrackItemKind.Coin:
                case TrackItemKind.Shield:
                case TrackItemKind.Star:
                case TrackItemKind.Dumbbell:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsObstacle(TrackItemKind kind) => !IsPickup(kind);

        public static Core.PickupKind ToPickup(TrackItemKind kind)
        {
            switch (kind)
            {
                case TrackItemKind.Shield:
                    return Core.PickupKind.Shield;
                case TrackItemKind.Star:
                    return Core.PickupKind.Star;
                case TrackItemKind.Dumbbell:
                    return Core.PickupKind.Dumbbell;
                default:
                    return Core.PickupKind.Coin;
            }
        }

        public static Core.ObstacleKind ToObstacle(TrackItemKind kind)
        {
            return kind == TrackItemKind.LowBarrier ? Core.ObstacleKind.LowBarrier : Core.ObstacleKind.Log;
        }

        /// <summary>
        /// Под низкой перекладиной проходит даже стоя, поэтому она не сбивает щит.
        /// </summary>
        public static bool PassesUnder(TrackItemKind kind) => kind == TrackItemKind.LowBarrier;
    }
}
