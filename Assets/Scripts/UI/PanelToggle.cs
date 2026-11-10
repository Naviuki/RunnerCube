using UnityEngine;

namespace RunnerCube.UI
{
    /// <summary>
    /// Показывает или прячет панель. Если ссылка не задана, управляет своим объектом.
    /// </summary>
    public sealed class PanelToggle : MonoBehaviour
    {
        [SerializeField] private GameObject panel;

        private GameObject Target => panel != null ? panel : gameObject;

        public void Show(bool visible)
        {
            GameObject target = Target;
            if (target.activeSelf != visible)
                target.SetActive(visible);
        }

        public void Toggle()
        {
            GameObject target = Target;
            target.SetActive(!target.activeSelf);
        }
    }
}
