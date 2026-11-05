using System;
using System.Collections.Generic;
using UnityEngine;

namespace RunnerCube.Pooling
{
    /// <summary>
    /// Пул компонентов: держит выключенные экземпляры и отдаёт их повторно, не плодя мусор.
    /// </summary>
    public sealed class ObjectPool<T> where T : Component
    {
        private readonly Func<T> _create;
        private readonly Transform _root;
        private readonly Stack<T> _free = new Stack<T>();
        private readonly List<T> _created = new List<T>();

        public ObjectPool(Func<T> create, Transform root, int prewarm = 0)
        {
            _create = create ?? throw new ArgumentNullException(nameof(create));
            _root = root;

            for (int i = 0; i < prewarm; i++)
                _free.Push(CreateDisabled());
        }

        public int Total => _created.Count;
        public int Available => _free.Count;

        public event Action<T> Taken;
        public event Action<T> Returned;

        public T Rent(Vector3 position, Quaternion rotation)
        {
            T item = _free.Count > 0 ? _free.Pop() : CreateDisabled();
            item.transform.SetPositionAndRotation(position, rotation);
            item.gameObject.SetActive(true);
            Taken?.Invoke(item);
            return item;
        }

        public void Return(T item)
        {
            if (item == null || !item.gameObject.activeSelf)
                return;

            item.gameObject.SetActive(false);
            if (_root != null)
                item.transform.SetParent(_root, false);

            _free.Push(item);
            Returned?.Invoke(item);
        }

        public void ReturnAll(IEnumerable<T> items)
        {
            foreach (T item in items)
                Return(item);
        }

        public void Clear()
        {
            _free.Clear();
            foreach (T item in _created)
            {
                if (item != null)
                    UnityEngine.Object.Destroy(item.gameObject);
            }

            _created.Clear();
        }

        private T CreateDisabled()
        {
            T item = _create();
            item.gameObject.SetActive(false);
            _created.Add(item);
            return item;
        }
    }
}
