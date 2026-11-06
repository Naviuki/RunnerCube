using UnityEngine;
using UnityEngine.UI;

namespace RunnerCube.UI
{
    /// <summary>
    /// Кнопка паузы на экране забега.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public sealed class PauseButton : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(Toggle);
        }

        private void OnDestroy()
        {
            Button button = GetComponent<Button>();
            if (button != null)
                button.onClick.RemoveListener(Toggle);
        }

        private void Toggle() => Core.SceneServices.Instance.Session.TogglePause();
    }
}
