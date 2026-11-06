using UnityEngine;

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

        private Core.PlayerProgress _progress;
        private int _index;

        private void Start()
        {
            _progress = Core.SceneServices.Instance.Session.Progress;
            _index = Mathf.Clamp(_progress.SelectedSkin, 0, catalog.Count - 1);
            Refresh();
        }

        public void Next() => Step(1);

        public void Previous() => Step(-1);

        public void Buy()
        {
            Data.SkinEntry entry = catalog.Get(_index);
            if (IsUnlocked() || !_progress.TrySpend(entry.Price))
                return;

            _progress.UnlockSkin(entry.DisplayName);
            _progress.SelectedSkin = _index;
            Refresh();
        }

        private void Step(int direction)
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
            priceLabel.Set(entry.Price);
            if (buyButton != null)
                buyButton.SetActive(!unlocked);
        }
    }
}
