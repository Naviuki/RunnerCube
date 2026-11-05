using UnityEngine;
using RunnerCube.Core;

namespace RunnerCube.Track
{
    /// <summary>
    /// Сдвигает дорогу и предметы назад ровно на дистанцию, которую прошёл игрок за кадр.
    /// </summary>
    [DefaultExecutionOrder(100)]
    public sealed class RoadMover : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private Transform roadParent;
        [SerializeField] private Transform itemsParent;

        private float _lastPlayerZ;

        public void Begin()
        {
            _lastPlayerZ = player.position.z;
        }

        private void LateUpdate()
        {
            if (SceneServices.Instance == null || !SceneServices.Instance.Session.IsPlaying)
                return;

            float delta = player.position.z - _lastPlayerZ;
            _lastPlayerZ = player.position.z;
            if (delta <= 0f)
                return;

            Vector3 shift = Vector3.back * delta;
            roadParent.position += shift;
            itemsParent.position += shift;
        }
    }
}
