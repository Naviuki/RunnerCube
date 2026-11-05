using UnityEngine;

namespace RunnerCube.Track
{
    /// <summary>
    /// Отметка стартового момента: игрок в середине полосы, дорога выровнена.
    /// </summary>
    public sealed class StartPoint : MonoBehaviour
    {
        [SerializeField] private Vector3 playerPosition = new Vector3(0f, 0f, 0f);

        public Vector3 PlayerPosition => playerPosition;
    }
}
