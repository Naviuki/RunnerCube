using System;
using System.Collections.Generic;
using UnityEngine;

namespace RunnerCube.Pooling
{
    /// <summary>
    /// Пул компонентов одного префаба: держит выключенные экземпляры и отдаёт их повторно.
    /// </summary>
    public sealed class ObjectPool<T> where T : Component
    {
        private readonly Func<T> _create;
        private readonly Transform _root;
        private readonly Stack<T> _free = new Stack<T>();

        public ObjectPool(Func<T> create, Transform root, int prewarm = 0)
        {
            _create = create ?? throw new ArgumentNullException(nameof(create));
            _root = root;

            for (int i = 0; i < prewarm; i++)
            {
                T item = CreateDisabled();
                if (item != null)
                    _free.Push(item);
            }
        }

        public int Available => _free.Count;

        public T Rent(Vector3 position, Quaternion rotation)
        {
            T item = _free.Count > 0 ? _free.Pop() : CreateDisabled();
            if (item == null)
                return null;

            item.transform.SetPositionAndRotation(position, rotation);
            item.gameObject.SetActive(true);
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
        }

        public void Clear()
        {
            _free.Clear();
        }

        private T CreateDisabled()
        {
            T item = _create();
            if (item == null)
                return null;

            item.gameObject.SetActive(false);
            return item;
        }
    }
}
