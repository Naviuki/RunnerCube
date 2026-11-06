using UnityEngine;

namespace RunnerCube.Track
{
    /// <summary>
    /// Три полосы дороги: левая, средняя и правая.
    /// </summary>
    public static class Lanes
    {
        public const int Count = 3;
        public const int Middle = 1;

        public static float ToX(int lane, float lineWidth) => (lane - Middle) * lineWidth;
    }

    /// <summary>
    /// Позиция предмета в ряду: полоса и место на дороге.
    /// </summary>
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
