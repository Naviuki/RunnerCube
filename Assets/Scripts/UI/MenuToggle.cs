using UnityEngine;

namespace RunnerCube.UI
{
    /// <summary>
    /// Включает и выключает панель. Если ссылка не задана, берёт объект с самим скриптом.
    /// </summary>
    public sealed class MenuToggle : MonoBehaviour
    {
        [SerializeField] private GameObject content;

        private void Awake()
        {
            if (content == null)
                content = gameObject;
        }

        public void Toggle()
        {
            if (content == null)
                return;

            content.SetActive(!content.activeSelf);
        }

        public void Open()
        {
            if (content != null)
                content.SetActive(true);
        }

        public void Close()
        {
            if (content != null)
                content.SetActive(false);
        }
    }
}
