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

        public void Pause() => SceneServices.Instance.Session.TogglePause();

        public void Resume() => SceneServices.Instance.Session.TogglePause();

        public void Restart() => scenes.Restart();

        public void ToMenu() => scenes.LoadMenu();
    }
}
