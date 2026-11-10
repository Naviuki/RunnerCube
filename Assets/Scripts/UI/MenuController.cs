using UnityEngine;
using RunnerCube.Core;

namespace RunnerCube.UI
{
    /// <summary>
    /// Кнопки меню: переходы, звук, язык и панели. Вся логика меню собрана здесь.
    /// </summary>
    public sealed class MenuController : MonoBehaviour
    {
        private const string OptionsPath = "Options";
        private const string AboutPath = "Options/About Panel";
        private const string ExitPath = "ExitPanel";
        private const float ShopCameraDistance = 100f;

        [SerializeField] private SceneFlow scenes;
        [SerializeField] private MenuLabel[] labels = new MenuLabel[0];
        [SerializeField] private NumberLabel coinsLabel;
        [SerializeField] private NumberLabel recordLabel;
        [SerializeField] private AudioSource clickSource;

        private Camera _camera;
        private Vector3 _menuCameraPosition;
        private Quaternion _menuCameraRotation;

        private void Start()
        {
            if (scenes == null)
                scenes = FindAnyObjectByType<SceneFlow>(FindObjectsInactive.Include);

            _camera = Camera.main;
            if (_camera != null)
            {
                _menuCameraPosition = _camera.transform.position;
                _menuCameraRotation = _camera.transform.rotation;
            }

            PlayerProgress progress = SceneServices.Instance.Session.Progress;
            int language = progress.Language;

            if (labels == null || labels.Length == 0)
                labels = FindObjectsByType<MenuLabel>(FindObjectsInactive.Include);

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

        public void ToggleShop()
        {
            Click();
            GameObject shop = FindShopPanel();
            if (shop == null)
                return;

            SetShopVisible(!shop.activeSelf);
        }

        public void ToggleOptions()
        {
            Click();
            SetActive(FindChild(AboutPath), false);
            Toggle(FindChild(OptionsPath));
        }

        public void ToggleAbout()
        {
            Click();
            GameObject options = FindChild(OptionsPath);
            if (options != null)
                options.SetActive(true);

            Toggle(FindChild(AboutPath));
        }

        public void ClosePanels()
        {
            Click();
            SetActive(FindChild(OptionsPath), false);
            SetActive(FindChild(AboutPath), false);
            SetActive(FindChild(ExitPath), false);
            SetShopVisible(false);
        }

        public void NextSkin() => Shop(shop => shop.Next());

        public void PreviousSkin() => Shop(shop => shop.Previous());

        public void BuySkin() => Shop(shop => shop.Buy());

        public void ToggleSound()
        {
            Click();
            AudioListener.volume = AudioListener.volume > 0f ? 0f : 1f;
        }

        public void SetRussian() => SetLanguage(Languages.Russian);

        public void SetEnglish() => SetLanguage(Languages.English);

        public void SetTatar() => SetLanguage(Languages.Tatar);

        private void Shop(System.Action<ShopController> action)
        {
            Click();
            ShopController shop = FindAnyObjectByType<ShopController>(FindObjectsInactive.Include);
            if (shop != null)
                action(shop);
        }

        private void Toggle(GameObject target)
        {
            if (target != null)
                target.SetActive(!target.activeSelf);
        }

        private static void SetActive(GameObject target, bool visible)
        {
            if (target != null)
                target.SetActive(visible);
        }

        private GameObject FindChild(string path)
        {
            GameObject canvas = GameObject.Find("Canvas");
            if (canvas == null)
                return null;

            Transform node = canvas.transform.Find(path);
            return node == null ? null : node.gameObject;
        }

        private void SetShopVisible(bool visible)
        {
            GameObject shop = FindShopPanel();
            if (shop == null)
                return;

            shop.SetActive(visible);

            if (_camera == null)
                _camera = Camera.main;

            if (_camera == null)
                return;

            if (visible)
            {
                Vector3 position = shop.transform.position + Vector3.back * ShopCameraDistance;
                _camera.transform.SetPositionAndRotation(position, _menuCameraRotation);
                return;
            }

            _camera.transform.SetPositionAndRotation(_menuCameraPosition, _menuCameraRotation);
        }

        private static GameObject FindShopPanel()
        {
            GameObject shop = FindRoot("Shop");
            if (shop == null)
                return null;

            Transform canvas = shop.transform.Find("Canvas");
            return canvas != null ? canvas.gameObject : shop;
        }

        private static GameObject FindRoot(string name)
        {
            GameObject direct = GameObject.Find(name);
            if (direct != null)
                return direct;

            GameObject canvas = GameObject.Find("Canvas");
            if (canvas == null)
                return null;

            Transform node = canvas.transform.Find(name);
            return node == null ? null : node.gameObject;
        }

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
