using UnityEngine;
using UnityEngine.UI;

namespace RunnerCube.UI
{
    /// <summary>
    /// Открывает панель по кнопке и закрывает по кнопке «назад».
    /// </summary>
    public sealed class PanelSwitcher : MonoBehaviour
    {
        [SerializeField] private GameObject page;
        [SerializeField] private Button openButton;
        [SerializeField] private Button closeButton;

        private void Awake()
        {
            if (openButton != null)
                openButton.onClick.AddListener(Open);

            if (closeButton != null)
                closeButton.onClick.AddListener(Close);
        }

        private void OnDestroy()
        {
            if (openButton != null)
                openButton.onClick.RemoveListener(Open);

            if (closeButton != null)
                closeButton.onClick.RemoveListener(Close);
        }

        public void Open()
        {
            if (page != null)
                page.SetActive(true);
        }

        public void Close()
        {
            if (page != null)
                page.SetActive(false);
        }
    }
}
