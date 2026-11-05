using System;
using UnityEngine;

namespace RunnerCube.Data
{
    /// <summary>
    /// Скины в порядке магазина. Индекс в списке совпадает с SelectedScin в PlayerPrefs.
    /// </summary>
    [CreateAssetMenu(fileName = "SkinCatalog", menuName = "RunnerCube/Skin Catalog")]
    public sealed class SkinCatalog : ScriptableObject
    {
        [SerializeField] private SkinEntry[] skins = new SkinEntry[0];

        public int Count => skins.Length;

        public SkinEntry Get(int index) => skins[Mathf.Clamp(index, 0, skins.Length - 1)];
    }

    [Serializable]
    public sealed class SkinEntry
    {
        [SerializeField] private string displayName;
        [SerializeField, Min(0)] private int price;
        [SerializeField] private GameObject prefab;

        public string DisplayName => displayName;
        public int Price => price;
        public GameObject Prefab => prefab;
    }
}
