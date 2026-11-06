using UnityEngine;
using RunnerCube.Core;

namespace RunnerCube.UI
{
    /// <summary>
    /// Кнопки меню: переходы, звук, язык и удаление прогресса.
    /// </summary>
    public sealed class MenuController : MonoBehaviour
    {
        [SerializeField] private SceneFlow scenes;
        [SerializeField] private MenuLabel[] labels = new MenuLabel[0];
        [SerializeField] private NumberLabel coinsLabel;
        [SerializeField] private NumberLabel recordLabel;
        [SerializeField] private AudioSource clickSource;

        private void Start()
        {
            if (scenes == null)
                scenes = FindFirstObjectByType<SceneFlow>(FindObjectsInactive.Include);

            PlayerProgress progress = SceneServices.Instance.Session.Progress;
            int language = progress.Language;

            if (labels != null)
            {
                foreach (MenuLabel label in labels)
                {
                    if (label != null)
                        label.Apply(language);
                }
            }

            if (coinsLabel != null)
                coinsLabel.Set(progress.Coins);

            if (recordLabel != null)
                recordLabel.Set(progress.Record);
        }

        public void Play()
        {
            Click();
            if (scenes != null)
                scenes.StartRun();
        }

        public void Quit()
        {
            Click();
            if (scenes != null)
                scenes.Quit();
            else
                Application.Quit();
        }

        public void DeleteProgress()
        {
            Click();
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }

        public void NextSkin()
        {
            Click();
            ShopController shop = FindFirstObjectByType<ShopController>(FindObjectsInactive.Include);
            if (shop != null)
                shop.Next();
        }

        public void PreviousSkin()
        {
            Click();
            ShopController shop = FindFirstObjectByType<ShopController>(FindObjectsInactive.Include);
            if (shop != null)
                shop.Previous();
        }

        public void BuySkin()
        {
            Click();
            ShopController shop = FindFirstObjectByType<ShopController>(FindObjectsInactive.Include);
            if (shop != null)
                shop.Buy();
        }

        public void ToggleShop()
        {
            Click();
            MenuToggle shop = FindFirstObjectByType<MenuToggle>(FindObjectsInactive.Include);
            if (shop != null)
                shop.Toggle();
        }

        public void ToggleSound()
        {
            Click();
            AudioListener.volume = AudioListener.volume > 0f ? 0f : 1f;
        }

        public void SetRussian() => SetLanguage(Languages.Russian);

        public void SetEnglish() => SetLanguage(Languages.English);

        public void SetTatar() => SetLanguage(Languages.Tatar);

        private void Click()
        {
            if (clickSource != null && clickSource.clip != null)
                clickSource.PlayOneShot(clickSource.clip);
        }

        private void SetLanguage(int language)
        {
            Click();
            SceneServices.Instance.Session.Progress.SetLanguage(language);

            if (scenes != null)
                scenes.Restart();
        }
    }
}
