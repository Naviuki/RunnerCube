using UnityEngine;
using RunnerCube.Core;
using RunnerCube.Data;

namespace RunnerCube.UI
{
    /// <summary>
    /// Меню: подписи, счётчики и переходы. Панели меню живут в сцене и управляются PanelToggle.
    /// </summary>
    public sealed class MenuController : MonoBehaviour
    {
        [SerializeField] private SceneFlow scenes;
        [SerializeField] private MenuLabel[] labels = new MenuLabel[0];
        [SerializeField] private NumberLabel coinsLabel;
        [SerializeField] private NumberLabel recordLabel;

        private void Start()
        {
            PlayerProgress progress = SceneServices.Instance.Session.Progress;
            int language = progress.Language;

            foreach (MenuLabel label in labels)
                label.Apply(language);

            coinsLabel.Set(progress.Coins);
            recordLabel.Set(progress.Record);
        }

        public void Play() => scenes.StartRun();

        public void Quit() => scenes.Quit();

        public void DeleteProgress()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
    }
}
