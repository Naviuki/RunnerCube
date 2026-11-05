using System.Collections.Generic;
using UnityEngine;
using RunnerCube.Core;
using RunnerCube.Data;
using RunnerCube.Pooling;

namespace RunnerCube.Track
{
    /// <summary>
    /// Собирает дорогу под игроком: плитки и ряды предметов появляются впереди, ушедшие в пул.
    /// Движение выполняет RoadMover, тут только наполнение.
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

        public void ResetTrack()
        {
            foreach (PooledObject item in _items)
                item.ReturnToPool();

            foreach (PooledObject tile in _tiles)
                tile.ReturnToPool();

            _items.Clear();
            _tiles.Clear();

            _rows = new RowBuilder(settings);
            _nextTileZ = 0f;
            _nextRowZ = settings.FirstRowDistance;

            SpawnTile(settings.StartTiles[0]);
            for (int i = 1; i < settings.InitialTiles; i++)
                SpawnTile(settings.RoadTiles[Random.Range(0, settings.RoadTiles.Count)]);
        }

        private void Update()
        {
            if (!SceneServices.Instance.Session.IsPlaying)
                return;

            float ahead = player.position.z + settings.SpawnDistance;

            while (_nextTileZ < ahead)
            {
                SpawnTile(settings.RoadTiles[Random.Range(0, settings.RoadTiles.Count)]);
            }

            while (_nextRowZ < ahead)
            {
                SpawnRow(_nextRowZ);
                _nextRowZ += settings.RowGap;
            }

            Recycle();
        }

        private void SpawnTile(GameObject prefab)
        {
            PooledObject tile = factory.Spawn(prefab, new Vector3(0f, 0f, _nextTileZ));
            _tiles.Add(tile);
            _nextTileZ += settings.TileLength;
        }

        private void SpawnRow(float z)
        {
            List<PlacedItem> row = _rows.Build(z);
            foreach (PlacedItem placed in row)
            {
                GameObject prefab = settings.FindPrefab(placed.Kind);
                if (prefab == null)
                    continue;

                Vector3 position = new Vector3(Lanes.ToX(placed.Lane, run.LineWidth), 0f, z);
                PooledObject instance = factory.Spawn(prefab, position);
                TrackItemMarker marker = instance.GetComponent<TrackItemMarker>();
                if (marker != null)
                    marker.SetKind(placed.Kind, IsObstacle(placed.Kind));

                _items.Add(instance);
            }
        }

        private static bool IsObstacle(TrackItemKind kind) => TrackItemRules.IsObstacle(kind);

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
                if (_items[i] != null && _items[i].gameObject.activeSelf && _items[i].transform.position.z > behind)
                    continue;

                _items.RemoveAt(i);
            }
        }
    }
}
