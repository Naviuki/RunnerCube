using UnityEngine;
using RunnerCube.Core;
using RunnerCube.Player;
using RunnerCube.Track;

namespace RunnerCube.Lifecycle
{
    /// <summary>
    /// Запускает забег в сцене Game: обнуляет игрока, ставит дорогу и включает сессию.
    /// </summary>
    [DefaultExecutionOrder(-50)]
    public sealed class RunBootstrap : MonoBehaviour
    {
        [SerializeField] private PlayerMotor player;
        [SerializeField] private TrackSpawner track;
        [SerializeField] private RoadMover mover;

        private void Start()
        {
            player.ResetRun();
            track.ResetTrack();
            mover.Begin();
            SceneServices.Instance.Session.Begin();
        }

        private void Update()
        {
            SceneServices.Instance.Session.Tick(Time.deltaTime);
        }
    }
}
