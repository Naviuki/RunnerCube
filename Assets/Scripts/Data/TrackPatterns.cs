using System;
using System.Collections.Generic;
using UnityEngine;

namespace RunnerCube.Data
{
    [Serializable]
    public sealed class TrackItemWeight
    {
        [SerializeField] private TrackItemKind kind;
        [SerializeField] private GameObject prefab;
        [SerializeField, Min(0f)] private float weight = 1f;

        public TrackItemWeight()
        {
        }

        public TrackItemWeight(TrackItemKind kind, GameObject prefab, float weight)
        {
            this.kind = kind;
            this.prefab = prefab;
            this.weight = weight;
        }

        public TrackItemKind Kind => kind;
        public GameObject Prefab => prefab;
        public float Weight => weight;
    }

    [Serializable]
    public sealed class PatternItem
    {
        [SerializeField] private TrackItemKind kind;
        [SerializeField, Range(-1, 1)] private int lane;

        public PatternItem()
        {
        }

        public PatternItem(TrackItemKind kind, int lane)
        {
            this.kind = kind;
            this.lane = lane;
        }

        public TrackItemKind Kind => kind;
        public int Lane => lane;
    }

    [Serializable]
    public sealed class TilePattern
    {
        [SerializeField] private string patternName;
        [SerializeField, Min(0f)] private float weight = 1f;
        [SerializeField] private List<PatternItem> items = new List<PatternItem>();

        public TilePattern()
        {
        }

        public TilePattern(string name, float weight, params PatternItem[] items)
        {
            patternName = name;
            this.weight = weight;
            this.items = new List<PatternItem>(items);
        }

        public string Name => patternName;
        public float Weight => weight;
        public IReadOnlyList<PatternItem> Items => items;
    }
}
