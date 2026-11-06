using UnityEngine;
using UnityEngine.UI;

namespace RunnerCube.UI
{
    /// <summary>
    /// Показывает или прячет панель. Ссылка на панель задаётся в инспекторе или берётся с этого объекта.
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
            if (panel == null)
                return;

            if (panel.activeSelf != visible)
                panel.SetActive(visible);
        }

        public void Toggle()
        {
            if (panel != null)
                panel.SetActive(!panel.activeSelf);
        }
    }
}
