using UnityEngine;

namespace RunnerCube.Core
{
    /// <summary>
    /// Пауза по Esc. В меню сессии нет, поэтому проверка на null.
    /// </summary>
    public sealed class PauseHotkey : MonoBehaviour
    {
        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Escape) || SceneServices.Instance == null)
                return;

            SceneServices.Instance.Session.TogglePause();
        }
    }
}
