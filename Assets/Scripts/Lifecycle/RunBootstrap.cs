using UnityEngine;
using RunnerCube.Core;
using RunnerCube.Player;
using RunnerCube.Track;

namespace RunnerCube.Lifecycle
{
    /// <summary>
    /// Запускает забег в сцене Game: обнуляет игрока, ставит дорогу и включает сессию.
    /// Недостающие ссылки ищет сам, чтобы сцена оставалась рабочей после правок.
    /// </summary>
    [DefaultExecutionOrder(-50)]
    public sealed class RunBootstrap : MonoBehaviour
    {
        [SerializeField] private PlayerMotor player;
        [SerializeField] private TrackSpawner track;
        [SerializeField] private RoadMover mover;

        private void Awake()
        {
            if (player == null)
                player = FindFirstObjectByType<PlayerMotor>(FindObjectsInactive.Include);

            if (track == null)
                track = FindFirstObjectByType<TrackSpawner>(FindObjectsInactive.Include);

            if (mover == null)
                mover = FindFirstObjectByType<RoadMover>(FindObjectsInactive.Include);
        }

        private void Start()
        {
            if (player != null)
                player.ResetRun();

            if (track != null)
                track.ResetTrack();

            if (mover != null)
                mover.Begin();

            SceneServices.Instance.Session.Begin();
        }

        private void Update()
        {
            SceneServices.Instance.Session.Tick(Time.deltaTime);
        }
    }
}
