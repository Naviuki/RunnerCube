using UnityEngine;
using RunnerCube.Core;

namespace RunnerCube.UI
{
    /// <summary>
    /// Кнопки паузы и экрана проигрыша: продолжить, начать заново, уйти в меню.
    /// </summary>
    public sealed class RunPanel : MonoBehaviour
    {
        [SerializeField] private SceneFlow scenes;

        private void Awake()
        {
            if (scenes == null)
                scenes = FindAnyObjectByType<SceneFlow>(FindObjectsInactive.Include);
        }

        public void Pause() => SceneServices.Instance.Session.Pause();

        public void Resume() => SceneServices.Instance.Session.Resume();

        public void Restart()
        {
            if (scenes != null)
                scenes.Restart();
        }

        public void ToMenu()
        {
            if (scenes != null)
                scenes.LoadMenu();
        }

        public void Quit()
        {
            if (scenes != null)
                scenes.Quit();
        }
    }
}
