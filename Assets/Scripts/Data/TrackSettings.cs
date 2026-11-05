using System.Collections.Generic;
using UnityEngine;

namespace RunnerCube.Data
{
    /// <summary>
    /// Дорога: какие плитки ставить, как часто появляются предметы и какой префаб соответствует варианту.
    /// </summary>
    [CreateAssetMenu(fileName = "TrackSettings", menuName = "RunnerCube/Track Settings")]
    public sealed class TrackSettings : ScriptableObject
    {
        [Header("Плитки")]
        [SerializeField] private List<GameObject> startTiles = new List<GameObject>();
        [SerializeField] private List<GameObject> roadTiles = new List<GameObject>();
        [SerializeField, Min(1f)] private float tileLength = 100f;
        [SerializeField, Min(1)] private int initialTiles = 6;
        [SerializeField, Min(0f)] private float spawnDistance = 150f;
        [SerializeField, Min(0f)] private float despawnDistance = 200f;

        [Header("Ряды")]
        [SerializeField, Min(1f)] private float rowGap = 25f;
        [SerializeField, Min(10f)] private float firstRowDistance = 120f;
        [SerializeField, Range(0f, 1f)] private float emptyRowChance = 0.15f;
        [SerializeField, Range(0f, 1f)] private float patternChance = 0.35f;

        [Header("Варианты")]
        [SerializeField] private List<TrackItemWeight> pickups = new List<TrackItemWeight>();
        [SerializeField] private List<TrackItemWeight> obstacles = new List<TrackItemWeight>();
        [SerializeField] private List<TilePattern> patterns = new List<TilePattern>();

        public IReadOnlyList<GameObject> StartTiles => startTiles;
        public IReadOnlyList<GameObject> RoadTiles => roadTiles;
        public float TileLength => tileLength;
        public int InitialTiles => initialTiles;
        public float SpawnDistance => spawnDistance;
        public float DespawnDistance => despawnDistance;
        public float RowGap => rowGap;
        public float FirstRowDistance => firstRowDistance;
        public float EmptyRowChance => emptyRowChance;
        public float PatternChance => patternChance;
        public IReadOnlyList<TrackItemWeight> Pickups => pickups;
        public IReadOnlyList<TrackItemWeight> Obstacles => obstacles;
        public IReadOnlyList<TilePattern> Patterns => patterns;

        public GameObject FindPrefab(TrackItemKind kind)
        {
            for (int i = 0; i < pickups.Count; i++)
            {
                if (pickups[i].Kind == kind)
                    return pickups[i].Prefab;
            }

            for (int i = 0; i < obstacles.Count; i++)
            {
                if (obstacles[i].Kind == kind)
                    return obstacles[i].Prefab;
            }

            return null;
        }
    }
}
