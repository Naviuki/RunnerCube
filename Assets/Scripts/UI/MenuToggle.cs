using UnityEngine;

namespace RunnerCube.UI
{
    /// <summary>
    /// Включает и выключает панель без отдельной подложки.
    /// </summary>
    public sealed class MenuToggle : MonoBehaviour
    {
        [SerializeField] private GameObject content;

        private void Awake()
        {
            if (content == null)
                content = gameObject;

            content.SetActive(false);
        }

        public void Toggle()
        {
            if (content != null)
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
