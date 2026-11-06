using System.Collections.Generic;
using UnityEngine;
using RunnerCube.Data;

namespace RunnerCube.Track
{
    /// <summary>
    /// Собирает ряд предметов: одно препятствие в случайной полосе и один подбор в соседней.
    /// </summary>
    public sealed class RowBuilder
    {
        private readonly TrackSettings _settings;
        private readonly List<PlacedItem> _row = new List<PlacedItem>(2);

        public RowBuilder(TrackSettings settings)
        {
            _settings = settings;
        }

        public List<PlacedItem> Build(float z)
        {
            _row.Clear();

            if (_settings.Obstacles.Count == 0 || Random.value < _settings.EmptyRowChance)
                return _row;

            int blocked = Random.Range(0, Lanes.Count);
            _row.Add(new PlacedItem(Pick(_settings.Obstacles), blocked, z));

            if (_settings.Pickups.Count > 0)
                _row.Add(new PlacedItem(Pick(_settings.Pickups), FreeLane(blocked), z));

            return _row;
        }

        private static int FreeLane(int blocked)
        {
            int lane = Random.Range(0, Lanes.Count - 1);
            return lane >= blocked ? lane + 1 : lane;
        }

        private static TrackItemKind Pick(IReadOnlyList<TrackItemWeight> weights)
        {
            float total = 0f;
            foreach (TrackItemWeight weight in weights)
                total += weight.Weight;

            float roll = Random.value * total;
            foreach (TrackItemWeight weight in weights)
            {
                roll -= weight.Weight;
                if (roll <= 0f)
                    return weight.Kind;
            }

            return weights[weights.Count - 1].Kind;
        }
    }
}
