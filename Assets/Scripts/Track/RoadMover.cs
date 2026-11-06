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

        private void Awake()
        {
            if (player == null)
            {
                Player.PlayerMotor motor = FindFirstObjectByType<Player.PlayerMotor>(FindObjectsInactive.Include);
                if (motor != null)
                    player = motor.transform;
            }

            if (roadParent == null)
                roadParent = transform;

            if (itemsParent == null)
                itemsParent = transform;
        }

        public void Begin()
        {
            _lastPlayerZ = player != null ? player.position.z : 0f;
        }

        private void LateUpdate()
        {
            if (player == null || SceneServices.Instance == null || !SceneServices.Instance.Session.IsPlaying)
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
