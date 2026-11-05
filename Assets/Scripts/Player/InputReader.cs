using UnityEngine;

namespace RunnerCube.Player
{
    /// <summary>
    /// Читает клавиатуру и свайпы и отдаёт действия по одному за кадр.
    /// </summary>
    public sealed class InputReader : MonoBehaviour
    {
        [SerializeField] private float swipeThresholdPixels = 100f;

        private Vector2 _dragStart;
        private bool _dragging;

        public System.Action<GameAction> Pressed;

        private void Update()
        {
            if (RunnerCube.UI.UiPointerGuard.IsPointerOverUi)
                return;

            if (ReadKey(out GameAction keyAction))
            {
                Pressed?.Invoke(keyAction);
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                _dragStart = Input.mousePosition;
                _dragging = true;
                return;
            }

            if (!_dragging)
                return;

            if (Input.GetMouseButtonUp(0))
            {
                _dragging = false;
                return;
            }

            Vector2 delta = (Vector2)Input.mousePosition - _dragStart;
            if (delta.magnitude < swipeThresholdPixels)
                return;

            _dragging = false;
            Pressed?.Invoke(DirectionOf(delta));
        }

        private static bool ReadKey(out GameAction action)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                action = GameAction.Left;
                return true;
            }

            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                action = GameAction.Right;
                return true;
            }

            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space))
            {
                action = GameAction.Jump;
                return true;
            }

            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                action = GameAction.Slide;
                return true;
            }

            action = GameAction.None;
            return false;
        }

        private static GameAction DirectionOf(Vector2 delta)
        {
            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                return delta.x < 0f ? GameAction.Left : GameAction.Right;

            return delta.y < 0f ? GameAction.Slide : GameAction.Jump;
        }
    }
}
