using UnityEngine;
using RunnerCube.Core;

namespace RunnerCube.Player
{
    /// <summary>
    /// Читает клавиатуру и мышь: A/D полосы, W или пробел прыжок, S подкат, плюс свайпы мышью.
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
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                Pressed?.Invoke(GameAction.Left);
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                Pressed?.Invoke(GameAction.Right);
            else if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                Pressed?.Invoke(GameAction.Jump);
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
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
