using UnityEngine;
using UnityEngine.UI;
using RunnerCube.Data;

namespace RunnerCube.UI
{
    /// <summary>
    /// Подпись меню. Текст берётся из таблицы локализации по ключу и языку.
    /// </summary>
    public sealed class MenuLabel : MonoBehaviour
    {
        [SerializeField] private Localization localization;
        [SerializeField] private TextKey key;
        [SerializeField] private Text text;

        private void Awake()
        {
            if (text == null)
                text = GetComponent<Text>();

            if (text == null)
                text = GetComponentInChildren<Text>(true);
        }

        public void Apply(int language)
        {
            if (localization == null || text == null)
                return;

            text.text = localization.Get(key, language);
        }
    }
}
