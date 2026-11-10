using UnityEngine;

namespace RunnerCube.Player
{
    /// <summary>
    /// Держит камеру за игроком на заданном смещении.
    /// </summary>
    public sealed class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new Vector3(0f, 13f, -30f);

        private void Awake()
        {
            if (target == null)
            {
                PlayerMotor motor = FindAnyObjectByType<PlayerMotor>(FindObjectsInactive.Include);
                if (motor != null)
                    target = motor.transform;
            }
        }

        private void LateUpdate()
        {
            if (target == null)
                return;

            transform.position = target.position + offset;
        }
    }
}
