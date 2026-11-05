using UnityEngine;
using UnityEngine.UI;

namespace RunnerCube.UI
{
    /// <summary>
    /// Кнопка меню со звуком клика и действием.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public sealed class MenuButton : MonoBehaviour
    {
        [SerializeField] private AudioSource clickSource;
        [SerializeField] private AudioClip clickClip;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(PlayClick);
        }

        private void PlayClick()
        {
            if (clickSource == null || clickClip == null)
                return;

            clickSource.PlayOneShot(clickClip);
        }
    }
}
