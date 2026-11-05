using UnityEngine;

namespace RunnerCube.Track
{
    /// <summary>
    /// Полосы дороги. Три штуки, средняя стартовая.
    /// </summary>
    public static class Lanes
    {
        public const int Count = 3;
        public const int Middle = 1;

        public static int Opposite(int lane) => lane == Middle ? 0 : Count - 1;

        public static int Move(int lane, int step) => Mathf.Clamp(lane + step, 0, Count - 1);

        public static float ToX(int lane, float lineWidth) => (lane - Middle) * lineWidth;
    }

    public readonly struct PlacedItem
    {
        public PlacedItem(Data.TrackItemKind kind, int lane, float z)
        {
            Kind = kind;
            Lane = lane;
            Z = z;
        }

        public Data.TrackItemKind Kind { get; }
        public int Lane { get; }
        public float Z { get; }
    }
}
