using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RunnerCube.Core;
using RunnerCube.Data;

namespace RunnerCube.UI
{
    /// <summary>
    /// Магазин скинов. Если каталог не подключён, показывает один скин без покупки.
    /// </summary>
    public sealed class ShopController : MonoBehaviour
    {
        [SerializeField] private SkinCatalog catalog;
        [SerializeField] private ShopPreview preview;
        [SerializeField] private GameObject buyButton;
        [SerializeField] private NumberLabel priceLabel;
        [SerializeField] private NumberLabel coinsLabel;

        private PlayerProgress _progress;
        private int _index;

        private int Count => catalog == null ? 0 : catalog.Count;

        private void Start()
        {
            _progress = SceneServices.Instance.Session.Progress;
            _index = Mathf.Max(0, Mathf.Min(_progress.SelectedSkin, Count - 1));
            Refresh();
        }

        public void Next() => Step(1);

        public void Previous() => Step(-1);

        public void Buy()
        {
            if (Count == 0)
                return;

            SkinEntry entry = catalog.Get(_index);
            if (IsUnlocked() || !_progress.TrySpend(entry.Price))
                return;

            _progress.UnlockSkin(entry.DisplayName);
            _progress.SelectedSkin = _index;
            Refresh();
        }

        public void Close()
        {
            if (transform.parent != null)
                transform.parent.gameObject.SetActive(false);
        }

        private void Step(int direction)
        {
            if (Count <= 1)
                return;

            _index = (_index + direction + Count) % Count;
            if (IsUnlocked())
                _progress.SelectedSkin = _index;

            Refresh();
        }

        private bool IsUnlocked()
        {
            SkinEntry entry = catalog.Get(_index);
            return _progress.IsSkinUnlocked(entry.DisplayName, entry.Price);
        }

        private void Refresh()
        {
            SkinEntry entry = Count == 0 ? null : catalog.Get(_index);

            if (preview != null)
                preview.Show(_index);

            if (coinsLabel != null)
                coinsLabel.Set(_progress.Coins);

            if (priceLabel != null && entry != null)
                priceLabel.Set(entry.Price);

            if (buyButton == null)
                return;

            bool locked = entry != null && !IsUnlocked();
            buyButton.SetActive(locked);
            Button button = buyButton.GetComponent<Button>();
            if (button != null)
                button.interactable = locked && _progress.Coins >= entry.Price;
        }
    }
}
