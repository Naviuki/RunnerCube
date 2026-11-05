using System.Collections.Generic;
using UnityEngine;
using RunnerCube.Data;

namespace RunnerCube.Track
{
    /// <summary>
    /// Собирает ряд предметов: либо готовый паттерн, либо случайная пара препятствие и подбор.
    /// </summary>
    public sealed class RowBuilder
    {
        private readonly TrackSettings _settings;
        private readonly List<PlacedItem> _row = new List<PlacedItem>(4);

        public RowBuilder(TrackSettings settings)
        {
            _settings = settings;
        }

        public List<PlacedItem> Build(float z)
        {
            _row.Clear();

            if (Random.value < _settings.EmptyRowChance)
                return _row;

            TilePattern pattern = PickPattern();
            if (pattern != null)
            {
                foreach (PatternItem item in pattern.Items)
                    _row.Add(new PlacedItem(item.Kind, Lanes.Move(item.Lane, Lanes.Middle), z));

                return _row;
            }

            int blocked = Random.Range(0, Lanes.Count);
            int free = RandomFreeLane(blocked);

            _row.Add(new PlacedItem(Pick(_settings.Obstacles), blocked, z));
            _row.Add(new PlacedItem(Pick(_settings.Pickups), free, z));
            return _row;
        }

        private static int RandomFreeLane(int blocked)
        {
            int lane = Random.Range(0, Lanes.Count);
            return lane == blocked ? Lanes.Opposite(blocked) : lane;
        }

        private TilePattern PickPattern()
        {
            if (_settings.Patterns.Count == 0 || Random.value > _settings.PatternChance)
                return null;

            float total = 0f;
            foreach (TilePattern pattern in _settings.Patterns)
                total += pattern.Weight;

            float roll = Random.value * total;
            foreach (TilePattern pattern in _settings.Patterns)
            {
                roll -= pattern.Weight;
                if (roll <= 0f)
                    return pattern;
            }

            return _settings.Patterns[_settings.Patterns.Count - 1];
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
