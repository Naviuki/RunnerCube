using UnityEngine;

namespace RunnerCube.Pooling
{
    /// <summary>
    /// База для объектов из пула. Помнит пул, в который вернётся.
    /// </summary>
    public class PooledObject : MonoBehaviour
    {
        private ObjectPool<PooledObject> _pool;

        public void BindPool(ObjectPool<PooledObject> pool)
        {
            _pool = pool;
        }

        public void ReturnToPool()
        {
            if (_pool != null)
            {
                _pool.Return(this);
                return;
            }

            gameObject.SetActive(false);
        }
    }
}
