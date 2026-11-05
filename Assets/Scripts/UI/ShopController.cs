using UnityEngine;
using RunnerCube.Core;

namespace RunnerCube.UI
{
    /// <summary>
    /// Магазин скинов: листает каталог, покупает и запоминает выбор.
    /// </summary>
    public sealed class ShopController : MonoBehaviour
    {
        [SerializeField] private Data.SkinCatalog catalog;
        [SerializeField] private ShopPreview preview;
        [SerializeField] private GameObject buyButton;
        [SerializeField] private NumberLabel priceLabel;
        [SerializeField] private NumberLabel coinsLabel;

        private PlayerProgress _progress;
        private int _index;

        private void Start()
        {
            _progress = SceneServices.Instance.Session.Progress;
            _index = Mathf.Clamp(_progress.SelectedSkin, 0, catalog.Count - 1);
            Refresh();
        }

        public void Next() => Move(1);

        public void Previous() => Move(-1);

        public void Buy()
        {
            Data.SkinEntry entry = catalog.Get(_index);
            if (IsUnlocked() || !_progress.TrySpend(entry.Price))
                return;

            _progress.UnlockSkin(entry.DisplayName);
            _progress.SelectedSkin = _index;
            Refresh();
        }

        private void Move(int direction)
        {
            _index = (_index + direction + catalog.Count) % catalog.Count;
            if (IsUnlocked())
                _progress.SelectedSkin = _index;

            Refresh();
        }

        private bool IsUnlocked()
        {
            Data.SkinEntry entry = catalog.Get(_index);
            return _progress.IsSkinUnlocked(entry.DisplayName, entry.Price);
        }

        private void Refresh()
        {
            Data.SkinEntry entry = catalog.Get(_index);
            bool unlocked = IsUnlocked();

            preview.Show(_index);
            coinsLabel.Set(_progress.Coins);
            buyButton.SetActive(!unlocked);
            priceLabel.Set(entry.Price);
        }
    }

    /// <summary>
    /// Только показывает выбранный скин и ничего не решает.
    /// </summary>
    public sealed class ShopPreview : MonoBehaviour
    {
        [SerializeField] private Data.SkinCatalog catalog;
        [SerializeField] private Transform anchor;

        private GameObject[] _instances = new GameObject[0];

        private void Awake()
        {
            _instances = new GameObject[catalog.Count];
            for (int i = 0; i < catalog.Count; i++)
            {
                _instances[i] = Instantiate(catalog.Get(i).Prefab, anchor);
                _instances[i].SetActive(false);
            }
        }

        public void Show(int index)
        {
            for (int i = 0; i < _instances.Length; i++)
                _instances[i].SetActive(i == index);
        }
    }
}
