using UnityEngine;

namespace RunnerCube.Track
{
    /// <summary>
    /// Помечает префаб предмета дороги: подбор или препятствие и его тип.
    /// </summary>
    public sealed class TrackItemMarker : MonoBehaviour
    {
        [SerializeField] private bool pickup = true;
        [SerializeField] private Core.PickupKind pickupKind = Core.PickupKind.Coin;
        [SerializeField] private Core.ObstacleKind obstacleKind = Core.ObstacleKind.Log;

        public bool IsPickup => pickup;
        public bool IsObstacle => !pickup;
        public Core.PickupKind Pickup => pickupKind;
        public Core.ObstacleKind Obstacle => obstacleKind;

        public void SetPickup(Core.PickupKind kind)
        {
            pickup = true;
            pickupKind = kind;
            SetTriggers(true);
        }

        public void SetObstacle(Core.ObstacleKind kind)
        {
            pickup = false;
            obstacleKind = kind;
            SetTriggers(false);
        }

        public void Collect()
        {
            Pooling.PooledObject pooled = GetComponent<Pooling.PooledObject>();
            if (pooled != null)
                pooled.ReturnToPool();
        }

        private void SetTriggers(bool trigger)
        {
            foreach (Collider body in GetComponentsInChildren<Collider>(true))
                body.isTrigger = trigger;
        }
    }
}
