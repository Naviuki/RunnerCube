using UnityEngine;

namespace RunnerCube.UI
{
    /// <summary>
    /// Показывает модель скина из каталога. Если каталог не подключён или модель не загрузилась,
    /// просто ничего не делает, без исключений.
    /// </summary>
    public sealed class ShopPreview : MonoBehaviour
    {
        [SerializeField] private Data.SkinCatalog catalog;
        [SerializeField] private Transform anchor;

        private GameObject[] _instances = new GameObject[0];

        private void Start()
        {
            if (catalog == null || anchor == null)
            {
                Debug.LogWarning("ShopPreview: не задан каталог или якорь", this);
                return;
            }

            _instances = new GameObject[catalog.Count];
            for (int i = 0; i < _instances.Length; i++)
            {
                GameObject prefab = catalog.Get(i).Prefab;
                if (prefab == null)
                    continue;

                _instances[i] = Instantiate(prefab, anchor);
                _instances[i].SetActive(false);
            }
        }

        public void Show(int index)
        {
            for (int i = 0; i < _instances.Length; i++)
            {
                GameObject instance = _instances[i];
                if (instance != null)
                    instance.SetActive(i == index);
            }
        }
    }
}
