using UnityEngine;
using UnityEngine.SceneManagement;
using RunnerCube.Core;

namespace RunnerCube.UI
{
    /// <summary>
    /// Выход из игры и перезагрузка текущей сцены. Переходы между сценами делает SceneFlow.
    /// </summary>
    public sealed class AppController : MonoBehaviour
    {
        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public void ReloadScene()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
