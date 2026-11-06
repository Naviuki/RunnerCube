using System;
using System.Collections.Generic;
using UnityEngine;

namespace RunnerCube.Data
{
    /// <summary>
    /// Пара «префаб дороги плюс как его встретить». Один префаб может быть и препятствием, и подбором.
    /// </summary>
    [Serializable]
    public sealed class TrackItemWeight
    {
        [SerializeField] private TrackItemKind kind;
        [SerializeField] private GameObject prefab;
        [SerializeField] private bool obstacle;
        [SerializeField, Min(0f)] private float weight = 1f;

        public TrackItemKind Kind => kind;
        public GameObject Prefab => prefab;
        public bool IsObstacle => obstacle;
        public float Weight => weight;
    }
}
