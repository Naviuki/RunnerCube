using UnityEngine;
using RunnerCube.Core;

namespace RunnerCube.Player
{
    /// <summary>
    /// Клавиши и свайпы с экрана. Отдаёт одно действие за кадр, чтобы игрок не смещался дважды.
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

            ReadKeyboard();
            ReadSwipe();
        }

        private void ReadKeyboard()
        {
            if (Input.GetKeyDown(InputKeys.Left) || Input.GetKeyDown(KeyCode.LeftArrow))
                Pressed?.Invoke(GameAction.Left);
            else if (Input.GetKeyDown(InputKeys.Right) || Input.GetKeyDown(KeyCode.RightArrow))
                Pressed?.Invoke(GameAction.Right);
            else if (Input.GetKeyDown(InputKeys.Jump) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                Pressed?.Invoke(GameAction.Jump);
            else if (Input.GetKeyDown(InputKeys.Slide) || Input.GetKeyDown(KeyCode.DownArrow))
                Pressed?.Invoke(GameAction.Slide);
        }

        private void ReadSwipe()
        {
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

        private static GameAction DirectionOf(Vector2 delta)
        {
            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                return delta.x < 0f ? GameAction.Left : GameAction.Right;

            return delta.y < 0f ? GameAction.Slide : GameAction.Jump;
        }
    }
}
