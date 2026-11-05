using UnityEngine;
using UnityEngine.UI;

namespace RunnerCube.UI
{
    /// <summary>
    /// Показывает скрытую панель. Логики не содержит.
    /// </summary>
    public sealed class PanelToggle : MonoBehaviour
    {
        [SerializeField] private GameObject panel;

        private void Awake()
        {
            if (panel == null)
                panel = gameObject;
        }

        public void Show(bool visible)
        {
            if (panel.activeSelf != visible)
                panel.SetActive(visible);
        }

        public void Toggle() => Show(!panel.activeSelf);
    }
}
