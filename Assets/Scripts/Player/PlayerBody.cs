using UnityEngine;
using RunnerCube.Data;

namespace RunnerCube.Player
{
    /// <summary>
    /// Коллайдер игрока: высота для стойки и для подката.
    /// </summary>
    public sealed class PlayerBody : MonoBehaviour
    {
        [SerializeField] private CharacterController controller;

        private Vector3 _standCenter;
        private float _standHeight;
        private float _standRadius;

        public bool OnGround => controller.isGrounded;

        private void Awake()
        {
            _standCenter = controller.center;
            _standHeight = controller.height;
            _standRadius = controller.radius;
        }

        public void Move(Vector3 motion) => controller.Move(motion);

        public void SetSliding(bool sliding)
        {
            if (sliding)
            {
                controller.height = _standHeight * 0.77f;
                controller.radius = _standRadius * 0.1f;
                controller.center = new Vector3(_standCenter.x, controller.height * 0.5f, _standCenter.z);
                return;
            }

            Stand();
        }

        public void Stand()
        {
            controller.height = _standHeight;
            controller.radius = _standRadius;
            controller.center = _standCenter;
        }
    }
}
