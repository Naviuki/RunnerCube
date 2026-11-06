using UnityEngine;
using UnityEngine.SceneManagement;

namespace RunnerCube.Core
{
    /// <summary>
    /// Переходы между сценами. Время сбрасывается перед загрузкой.
    /// </summary>
    public sealed class SceneFlow : MonoBehaviour
    {
        public void StartRun() => Load(GameConstants.GameSceneName);

        public void LoadMenu() => Load(GameConstants.MenuSceneName);

        public void Restart() => Load(SceneManager.GetActiveScene().name);

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
