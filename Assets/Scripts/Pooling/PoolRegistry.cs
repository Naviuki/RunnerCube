using UnityEngine;

namespace RunnerCube.Pooling
{
    /// <summary>
    /// Связка «префаб - пул». Экземпляры живут под общим родителем и включаются по запросу.
    /// </summary>
    public sealed class PoolRegistry : MonoBehaviour
    {
        private readonly System.Collections.Generic.Dictionary<GameObject, ObjectPool<PooledObject>> _pools =
            new System.Collections.Generic.Dictionary<GameObject, ObjectPool<PooledObject>>();

        public ObjectPool<PooledObject> For(GameObject prefab, int prewarm = 0)
        {
            if (_pools.TryGetValue(prefab, out ObjectPool<PooledObject> pool))
                return pool;

            Transform root = new GameObject(prefab.name + " Pool").transform;
            root.SetParent(transform, false);

            pool = new ObjectPool<PooledObject>(() =>
            {
                GameObject instance = Instantiate(prefab, root);
                PooledObject pooled = instance.GetComponent<PooledObject>();
                pooled.Bind(prefab);
                return pooled;
            }, root, prewarm);

            _pools.Add(prefab, pool);
            return pool;
        }

        public void Clear()
        {
            foreach (ObjectPool<PooledObject> pool in _pools.Values)
                pool.Clear();

            _pools.Clear();
        }
    }
}
