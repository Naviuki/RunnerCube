using UnityEngine;
using UnityEngine.UI;

namespace RunnerCube.UI
{
    /// <summary>
    /// Подпись меню. Текст берётся из таблицы локализации по ключу.
    /// </summary>
    [RequireComponent(typeof(Text))]
    public sealed class MenuLabel : MonoBehaviour
    {
        [SerializeField] private Data.Localization localization;
        [SerializeField] private Data.TextKey key;
        [SerializeField] private Text text;

        private void Awake()
        {
            if (text == null)
                text = GetComponent<Text>();
        }

        public void Apply(int language)
        {
            if (localization != null)
                text.text = localization.Get(key, language);
        }
    }
}
