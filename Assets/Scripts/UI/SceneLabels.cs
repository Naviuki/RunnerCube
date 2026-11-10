using UnityEngine;
using RunnerCube.Core;

namespace RunnerCube.UI
{
    /// <summary>
    /// Применяет перевод к подписям сцены при старте.
    /// </summary>
    public sealed class SceneLabels : MonoBehaviour
    {
        [SerializeField] private MenuLabel[] labels = new MenuLabel[0];

        private void Start()
        {
            int language = SceneServices.Instance.Session.Progress.Language;

            if (labels == null || labels.Length == 0)
                labels = FindObjectsByType<MenuLabel>(FindObjectsInactive.Include);

            if (labels == null)
                return;

            foreach (MenuLabel label in labels)
            {
                if (label != null)
                    label.Apply(language);
            }
        }
    }
}
