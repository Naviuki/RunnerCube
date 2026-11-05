using System;
using UnityEngine;

namespace RunnerCube.Data
{
    [Serializable]
    public sealed class StringTable
    {
        [SerializeField] private string[] lines = new string[0];

        public string Get(int index) => index >= 0 && index < lines.Length ? lines[index] : string.Empty;
    }

    /// <summary>
    /// Тексты интерфейса на трёх языках. Строка берётся по индексу языка из PlayerPrefs.
    /// </summary>
    [CreateAssetMenu(fileName = "Localization", menuName = "RunnerCube/Localization")]
    public sealed class Localization : ScriptableObject
    {
        [SerializeField] private StringTable russian = new StringTable();
        [SerializeField] private StringTable english = new StringTable();
        [SerializeField] private StringTable tatar = new StringTable();

        public string Get(TextKey key, int language)
        {
            return Table(language).Get((int)key);
        }

        private StringTable Table(int language)
        {
            if (language == 1)
                return english;

            if (language == 2)
                return tatar;

            return russian;
        }
    }
}
