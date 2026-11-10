using UnityEngine;

namespace RunnerCube.UI
{
    /// <summary>
    /// Показывает модель выбранного скина. Модели уже стоят в сцене, поэтому скрипт
    /// только включает нужную и выключает остальные.
    /// </summary>
    public sealed class ShopPreview : MonoBehaviour
    {
        [SerializeField] private Data.SkinCatalog catalog;
        [SerializeField] private Transform anchor;

        private GameObject[] _instances = new GameObject[0];

        private void Start()
        {
            Build();
        }

        public void Show(int index)
        {
            if (_instances.Length == 0)
                Build();

            for (int i = 0; i < _instances.Length; i++)
            {
                GameObject instance = _instances[i];
                if (instance != null)
                    instance.SetActive(i == index);
            }
        }

        private void Build()
        {
            if (anchor == null)
            {
                Debug.LogWarning("ShopPreview: не задан якорь", this);
                return;
            }

            if (anchor.childCount > 0)
            {
                _instances = new GameObject[anchor.childCount];
                for (int i = 0; i < _instances.Length; i++)
                    _instances[i] = anchor.GetChild(i).gameObject;

                return;
            }

            if (catalog == null)
            {
                Debug.LogWarning("ShopPreview: не задан каталог", this);
                return;
            }

            _instances = new GameObject[catalog.Count];
            for (int i = 0; i < _instances.Length; i++)
            {
                Data.SkinEntry entry = catalog.Get(i);
                if (entry == null || entry.Prefab == null)
                    continue;

                _instances[i] = Instantiate(entry.Prefab, anchor);
                _instances[i].SetActive(false);
            }
        }
    }
}
