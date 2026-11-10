using UnityEngine;

namespace RunnerCube.Track
{
    /// <summary>
    /// Фабрика плиток и предметов. Для плиток тип не важен, для предметов задаётся из ряда.
    /// </summary>
    public sealed class TrackFactory : MonoBehaviour
    {
        [SerializeField] private Pooling.PoolRegistry registry;

        private void Awake()
        {
            if (registry == null)
                registry = GetComponent<Pooling.PoolRegistry>();

            if (registry == null)
                registry = gameObject.AddComponent<Pooling.PoolRegistry>();
        }

        public Pooling.PooledObject SpawnTile(GameObject prefab, Vector3 position)
        {
            return registry == null ? null : registry.Rent(prefab, position);
        }

        public Pooling.PooledObject SpawnItem(GameObject prefab, Vector3 position, Data.TrackItemKind kind)
        {
            Pooling.PooledObject instance = SpawnTile(prefab, position);
            if (instance == null)
                return null;

            TrackItemMarker marker = instance.GetComponent<TrackItemMarker>();
            if (marker == null)
                return instance;

            if (Data.TrackItemRules.IsPickup(kind))
                marker.SetPickup(Data.TrackItemRules.ToPickup(kind));
            else
                marker.SetObstacle(Data.TrackItemRules.ToObstacle(kind));

            return instance;
        }
    }
}
