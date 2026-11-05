using UnityEngine;
using RunnerCube.Core;
using RunnerCube.Data;
using RunnerCube.Pooling;

namespace RunnerCube.Track
{
    /// <summary>
    /// Расставляет ряд предметов по полосам и делает из префаба нужный вариант.
    /// </summary>
    public sealed class RowLayout
    {
        private readonly TrackSettings _settings;
        private readonly RunSettings _run;
        private readonly TrackFactory _factory;

        public RowLayout(TrackSettings settings, RunSettings run, TrackFactory factory)
        {
            _settings = settings;
            _run = run;
            _factory = factory;
        }

        public void Place(System.Collections.Generic.List<PlacedItem> row)
        {
            foreach (PlacedItem item in row)
            {
                GameObject prefab = _settings.FindPrefab(item.Kind);
                if (prefab == null)
                    continue;

                Vector3 position = new Vector3(Lanes.ToX(item.Lane, _run.LineWidth), 0f, item.Z);
                PooledObject instance = _factory.Spawn(prefab, position);
                TrackItemMarker marker = instance.GetComponent<TrackItemMarker>();
                if (marker != null)
                    marker.SetKind(item.Kind, TrackItemRules.IsObstacle(item.Kind));
            }
        }
    }

    public sealed class RoadSide
    {
        public RoadSide(int lane, float lineWidth)
        {
            Lane = lane;
            LineWidth = lineWidth;
        }

        public int Lane { get; }
        public float LineWidth { get; }
    }
}
