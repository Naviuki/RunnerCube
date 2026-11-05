using UnityEngine;

namespace RunnerCube.UI
{
    /// <summary>
    /// Показывает и прячет объекты по кнопке. Используется для всплывающих панелей меню.
    /// </summary>
    public sealed class MenuPage : MonoBehaviour
    {
        [SerializeField] private GameObject page;

        private void Awake()
        {
            if (page == null)
                page = gameObject;
        }

        public void Open() => page.SetActive(true);

        public void Close() => page.SetActive(false);

        public void Toggle() => page.SetActive(!page.activeSelf);
    }
}
