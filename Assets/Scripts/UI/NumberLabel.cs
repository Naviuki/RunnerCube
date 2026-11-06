using UnityEngine;
using UnityEngine.UI;

namespace RunnerCube.UI
{
    /// <summary>
    /// Подпись числа. Значение приходит от контроллера, сама она ничего не считает.
    /// </summary>
    public sealed class NumberLabel : MonoBehaviour
    {
        [SerializeField] private Text text;

        private void Awake()
        {
            if (text == null)
                text = GetComponent<Text>();
        }

        public void Set(int value)
        {
            if (text != null)
                text.text = value.ToString();
        }

        public void Set(string value)
        {
            if (text != null)
                text.text = value;
        }
    }
}
