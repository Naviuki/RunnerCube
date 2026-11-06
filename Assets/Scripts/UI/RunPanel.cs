using UnityEngine;
using RunnerCube.Core;

namespace RunnerCube.UI
{
    /// <summary>
    /// Кнопки паузы и экрана проигрыша.
    /// </summary>
    public sealed class RunPanel : MonoBehaviour
    {
        [SerializeField] private SceneFlow scenes;
        [SerializeField] private NumberLabel recordLabel;

        public void Pause() => SceneServices.Instance.Session.TogglePause();

        public void Resume() => SceneServices.Instance.Session.TogglePause();

        public void Restart() => scenes.StartRun();

        public void ToMenu() => scenes.LoadMenu();

        public void ShowRecord()
        {
            if (recordLabel != null)
                recordLabel.Set(SceneServices.Instance.Session.Progress.Record);
        }
    }
}
