using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        [SerializeField] private TMP_Text priceText;
        [SerializeField] private NumberLabel coinsLabel;

        private Core.PlayerProgress _progress;
        private int _index;

        private int Count => catalog == null ? 0 : catalog.Count;

        private Core.PlayerProgress Progress
        {
            get
            {
                if (_progress == null)
                    _progress = Core.SceneServices.Instance.Session.Progress;

                return _progress;
            }
        }

        private void Start()
        {
            _index = Mathf.Clamp(Progress.SelectedSkin, 0, Mathf.Max(0, Count - 1));
            Refresh();
        }

        public void Next() => Step(1);

        public void Previous() => Step(-1);

        public void Buy()
        {
            if (Count == 0)
                return;

            Data.SkinEntry entry = catalog.Get(_index);
            if (entry == null || IsUnlocked() || !Progress.TrySpend(entry.Price))
                return;

            Progress.UnlockSkin(entry.DisplayName);
            Progress.SelectedSkin = _index;
            Refresh();
        }

        private void Step(int direction)
        {
            if (Count <= 1)
                return;

            _index = (_index + direction + Count) % Count;
            if (IsUnlocked())
                Progress.SelectedSkin = _index;

            Refresh();
        }

        private bool IsUnlocked()
        {
            Data.SkinEntry entry = catalog.Get(_index);
            return entry != null && Progress.IsSkinUnlocked(entry.DisplayName, entry.Price);
        }

        private void Refresh()
        {
            Data.SkinEntry entry = Count == 0 ? null : catalog.Get(_index);

            if (preview != null)
                preview.Show(_index);

            if (coinsLabel != null)
                coinsLabel.Set(Progress.Coins);

            if (priceText != null)
                priceText.text = entry == null ? string.Empty : entry.Price.ToString();

            if (buyButton == null)
                return;

            bool locked = entry != null && !IsUnlocked();
            buyButton.SetActive(locked);

            Button button = buyButton.GetComponent<Button>();
            if (button != null)
                button.interactable = locked && entry != null && Progress.Coins >= entry.Price;
        }
    }
}
