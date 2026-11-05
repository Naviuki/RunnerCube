using UnityEngine;

namespace RunnerCube.Track
{
    /// <summary>
    /// Поворачивает монету вокруг своей оси. Только визуал.
    /// </summary>
    public sealed class Spin : MonoBehaviour
    {
        [SerializeField] private float degreesPerSecond = 180f;

        private void Update()
        {
            transform.Rotate(0f, degreesPerSecond * Time.deltaTime, 0f, Space.World);
        }
    }
}
