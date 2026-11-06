using UnityEngine;

namespace RunnerCube.UI
{
    /// <summary>
    /// Показывает модель выбранного скина и ничего не решает.
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
