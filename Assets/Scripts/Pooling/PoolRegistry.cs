using System.Collections.Generic;
using UnityEngine;

namespace RunnerCube.Pooling
{
    /// <summary>
    /// Связка «префаб - пул». Экземпляры берутся только из своего префаба.
    /// </summary>
    public sealed class PoolRegistry : MonoBehaviour
    {
        private readonly Dictionary<GameObject, ObjectPool<PooledObject>> _pools =
            new Dictionary<GameObject, ObjectPool<PooledObject>>();

        public ObjectPool<PooledObject> For(GameObject prefab, int prewarm = 0)
        {
            if (prefab == null)
                return null;

            if (_pools.TryGetValue(prefab, out ObjectPool<PooledObject> pool))
                return pool;

            Transform root = new GameObject(prefab.name + " Pool").transform;
            root.SetParent(transform, false);

            pool = new ObjectPool<PooledObject>(() => Create(prefab, root), root, prewarm);
            _pools.Add(prefab, pool);
            return pool;
        }

        /// <summary>
        /// Префаб нужен только чтобы выбрать пул: у экземпляров он всегда один и тот же.
        /// </summary>
        public PooledObject Rent(GameObject prefab, Vector3 position)
        {
            ObjectPool<PooledObject> pool = For(prefab);
            if (pool == null)
                return null;

            PooledObject instance = pool.Rent(position, Quaternion.identity);
            if (instance == null)
                return null;

            instance.BindPool(pool);
            return instance;
        }

        public void Clear()
        {
            foreach (ObjectPool<PooledObject> pool in _pools.Values)
                pool.Clear();

            _pools.Clear();
        }

        private PooledObject Create(GameObject prefab, Transform root)
        {
            GameObject instance = Instantiate(prefab, root);
            PooledObject pooled = instance.GetComponent<PooledObject>();
            if (pooled == null)
                pooled = instance.AddComponent<PooledObject>();

            return pooled;
        }
    }
}
