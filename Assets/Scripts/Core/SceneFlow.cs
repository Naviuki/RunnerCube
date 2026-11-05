using UnityEngine;
using UnityEngine.SceneManagement;
using RunnerCube.Core;

namespace RunnerCube.Core
{
    /// <summary>
    /// Переходы между сценами и выход. Перед загрузкой сбрасывает паузу.
    /// </summary>
    public sealed class SceneFlow : MonoBehaviour
    {
        public void StartRun() => Load(GameConstants.GameSceneName);

        public void LoadMenu() => Load(GameConstants.MenuSceneName);

        public void Restart() => Load(SceneManager.GetActiveScene().name);

        public void ReloadCurrent()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private static void Load(string sceneName)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(sceneName);
        }
    }
}
