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

        private void Awake()
        {
            if (player == null)
                player = FindAnyObjectByType<PlayerMotor>(FindObjectsInactive.Include);

            if (track == null)
                track = FindAnyObjectByType<TrackSpawner>(FindObjectsInactive.Include);
        }

        private void Start()
        {
            if (player != null)
                player.ResetRun();

            if (track != null)
                track.ResetTrack();

            SceneServices.Instance.Session.Begin();
        }

        private void Update()
        {
            SceneServices.Instance.Session.Tick(Time.deltaTime);
        }
    }
}
