using UnityEngine;
using RunnerCube.Core;
using RunnerCube.Data;

namespace RunnerCube.Track
{
    /// <summary>
    /// Помечает префаб предмета дороги: какой это вариант и как с ним встречаться.
    /// </summary>
    public sealed class TrackItemMarker : MonoBehaviour
    {
        [SerializeField] private TrackItemKind kind = TrackItemKind.Coin;
        [SerializeField] private bool obstacle;

        public TrackItemKind Kind => kind;
        public bool IsObstacle => obstacle;
        public bool IsPickup => !obstacle;

        public void SetKind(TrackItemKind value, bool isObstacle)
        {
            kind = value;
            obstacle = isObstacle;

            foreach (Collider body in GetComponentsInChildren<Collider>(true))
                body.isTrigger = !isObstacle;
        }

        public PickupKind ToPickup() => TrackItemRules.ToPickup(kind);

        public ObstacleKind ToObstacle() => kind == TrackItemKind.LowBarrier ? ObstacleKind.LowBarrier : ObstacleKind.Log;

        public void Collect()
        {
            Pooling.PooledObject pooled = GetComponent<Pooling.PooledObject>();
            if (pooled != null)
                pooled.ReturnToPool();
        }
    }
}
