using System.Collections.Generic;
using UnityEngine;
using RunnerCube.Core;
using RunnerCube.Data;
using RunnerCube.Pooling;

namespace RunnerCube.Track
{
    /// <summary>
    /// Собирает дорогу под игроком: плитки и ряды предметов появляются впереди, ушедшие в пул.
    /// </summary>
    public sealed class TrackSpawner : MonoBehaviour
    {
        [SerializeField] private TrackSettings settings;
        [SerializeField] private RunSettings run;
        [SerializeField] private TrackFactory factory;
        [SerializeField] private Transform player;

        private readonly List<PooledObject> _tiles = new List<PooledObject>();
        private readonly List<PooledObject> _items = new List<PooledObject>();

        private RowBuilder _rows;
        private float _nextTileZ;
        private float _nextRowZ;

        private float LineWidth => (run != null ? run : RunSettings.Default).LineWidth;

        private void Awake()
        {
            if (factory == null)
                factory = GetComponent<TrackFactory>();

            if (player == null)
            {
                Player.PlayerMotor motor = FindAnyObjectByType<Player.PlayerMotor>(FindObjectsInactive.Include);
                if (motor != null)
                    player = motor.transform;
            }
        }

        public void ResetTrack()
        {
            if (settings == null || factory == null)
            {
                Debug.LogError("TrackSpawner: не заданы настройки дороги или фабрика", this);
                return;
            }

            foreach (PooledObject item in _items)
                item.ReturnToPool();

            foreach (PooledObject tile in _tiles)
                tile.ReturnToPool();

            _items.Clear();
            _tiles.Clear();

            _rows = new RowBuilder(settings);
            _nextTileZ = 0f;
            _nextRowZ = settings.FirstRowDistance;

            SpawnTile(settings.StartTiles.Count > 0 ? settings.StartTiles[0] : RandomRoadTile());

            for (int i = 1; i < settings.InitialTiles; i++)
                SpawnTile(RandomRoadTile());
        }

        private void Update()
        {
            if (settings == null || factory == null || player == null)
                return;

            if (!SceneServices.Instance.Session.IsPlaying)
                return;

            float ahead = player.position.z + settings.SpawnDistance;

            while (_nextTileZ < ahead)
                SpawnTile(RandomRoadTile());

            while (_nextRowZ < ahead)
            {
                SpawnRow(_nextRowZ);
                _nextRowZ += settings.RowGap;
            }

            Recycle();
        }

        private GameObject RandomRoadTile()
        {
            return settings.RoadTiles.Count == 0 ? null : settings.RoadTiles[Random.Range(0, settings.RoadTiles.Count)];
        }

        private void SpawnTile(GameObject prefab)
        {
            if (prefab == null)
                return;

            PooledObject tile = factory.SpawnTile(prefab, new Vector3(0f, 0f, _nextTileZ));
            if (tile != null)
                _tiles.Add(tile);

            _nextTileZ += settings.TileLength;
        }

        private void SpawnRow(float z)
        {
            if (_rows == null)
                return;

            foreach (PlacedItem placed in _rows.Build(z))
            {
                GameObject prefab = settings.FindPrefab(placed.Kind);
                if (prefab == null)
                    continue;

                Vector3 position = new Vector3(Lanes.ToX(placed.Lane, LineWidth), 0f, z);
                PooledObject instance = factory.SpawnItem(prefab, position, placed.Kind);
                if (instance != null)
                    _items.Add(instance);
            }
        }

        private void Recycle()
        {
            float behind = player.position.z - settings.DespawnDistance;

            for (int i = _tiles.Count - 1; i >= 0; i--)
            {
                if (_tiles[i].transform.position.z + settings.TileLength > behind)
                    continue;

                _tiles[i].ReturnToPool();
                _tiles.RemoveAt(i);
            }

            for (int i = _items.Count - 1; i >= 0; i--)
            {
                PooledObject item = _items[i];
                if (item == null)
                {
                    _items.RemoveAt(i);
                    continue;
                }

                if (item.gameObject.activeSelf && item.transform.position.z > behind)
                    continue;

                item.ReturnToPool();
                _items.RemoveAt(i);
            }
        }
    }
}
