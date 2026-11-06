using UnityEngine;

namespace RunnerCube.Core
{
    /// <summary>
    /// Пауза по Esc. В меню сессии ещё нет, поэтому проверяем её на null.
    /// </summary>
    public sealed class PauseHotkey : MonoBehaviour
    {
        private void Update()
        {
            if (!Input.GetKeyDown(InputKeys.Pause) || SceneServices.Instance == null)
                return;

            SceneServices.Instance.Session.TogglePause();
        }
    }
}
