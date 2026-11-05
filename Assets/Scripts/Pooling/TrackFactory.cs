using UnityEngine;

namespace RunnerCube.Pooling
{
    /// <summary>
    /// Создаёт объекты дороги из пулов. Один экземпляр на сцену.
    /// </summary>
    public sealed class TrackFactory : MonoBehaviour
    {
        [SerializeField] private PoolRegistry registry;

        public PooledObject Spawn(GameObject prefab, Vector3 position)
        {
            ObjectPool<PooledObject> pool = registry.For(prefab);
            PooledObject instance = pool.Rent(position, Quaternion.identity);
            instance.BindPool(pool);
            return instance;
        }
    }
}
