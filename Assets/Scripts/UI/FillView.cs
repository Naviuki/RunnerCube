using UnityEngine;
using UnityEngine.UI;

namespace RunnerCube.UI
{
    /// <summary>
    /// Заполнение радиальной иконки щита или звезды.
    /// </summary>
    public sealed class FillView : MonoBehaviour
    {
        [SerializeField] private Image image;

        public void SetFill(float value)
        {
            if (image != null)
                image.fillAmount = Mathf.Clamp01(value);
        }
    }
}
