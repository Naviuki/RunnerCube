using UnityEngine;

namespace RunnerCube.Core
{
    /// <summary>
    /// Одна точка входа сцены: сессия забега и шина событий.
    /// </summary>
    [DefaultExecutionOrder(-100)]
    public sealed class SceneServices : MonoBehaviour
    {
        public static SceneServices Instance { get; private set; }

        public RunSession Session { get; private set; }
        public GameEvents Events { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            Events = new GameEvents();
            Session = new RunSession(Events);
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }
    }
}
