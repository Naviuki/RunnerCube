using UnityEngine;

namespace RunnerCube.Pooling
{
    /// <summary>
    /// База для объектов из пула. Сама помнит, из какого пула пришла.
    /// </summary>
    public class PooledObject : MonoBehaviour
    {
        private ObjectPool<PooledObject> _pool;

        public GameObject SourcePrefab { get; private set; }

        public void Bind(GameObject prefab)
        {
            SourcePrefab = prefab;
        }

        public void BindPool(ObjectPool<PooledObject> pool)
        {
            _pool = pool;
        }

        public void ReturnToPool()
        {
            if (_pool != null)
                _pool.Return(this);
            else
                gameObject.SetActive(false);
        }
    }
}
