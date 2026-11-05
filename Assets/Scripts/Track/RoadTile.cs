using UnityEngine;

namespace RunnerCube.Track
{
    /// <summary>
    /// Плитка дороги: длина отрезка и сдвиг назад или вперёд.
    /// </summary>
    public sealed class RoadTile : MonoBehaviour
    {
        [SerializeField] private float length = 100f;

        public float Length => length;

        public void MoveBack(float distance) => transform.position += Vector3.back * distance;

        public void MoveForward(float distance) => transform.position += Vector3.forward * distance;
    }
}
