using UnityEngine;
using UnityEngine.UI;
using RunnerCube.Core;
using RunnerCube.Data;

namespace RunnerCube.UI
{
    /// <summary>
    /// Громкость, звук и язык. Никаких правил игры.
    /// </summary>
    public sealed class SettingsController : MonoBehaviour
    {
        [SerializeField] private Slider volume;
        [SerializeField] private Image soundIcon;
        [SerializeField] private Sprite soundOn;
        [SerializeField] private Sprite soundOff;
        [SerializeField] private SceneFlow scenes;

        private void Start()
        {
            volume.SetValueWithoutNotify(AudioListener.volume);
            volume.onValueChanged.AddListener(SetVolume);
        }

        public void SetVolume(float value) => AudioListener.volume = Mathf.Clamp01(value);

        public void ToggleSound()
        {
            bool muted = AudioListener.volume > 0f;
            AudioListener.volume = muted ? 0f : 1f;
            volume.SetValueWithoutNotify(AudioListener.volume);
            if (soundIcon != null)
                soundIcon.sprite = muted ? soundOff : soundOn;
        }

        public void SetQuality(int index) => QualitySettings.SetQualityLevel(index);

        public void SetLanguage(int index)
        {
            SceneServices.Instance.Session.Progress.SetLanguage(index);
            scenes.Restart();
        }
    }
}
