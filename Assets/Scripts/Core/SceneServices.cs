using UnityEngine;
using UnityEngine.SceneManagement;

namespace RunnerCube.Core
{
    /// <summary>
    /// Единственная точка входа сцены. Создаётся автоматически, если в сцене его нет.
    /// </summary>
    [DefaultExecutionOrder(-500)]
    public sealed class SceneServices : MonoBehaviour
    {
        public static SceneServices Instance { get; private set; }

        public RunSession Session { get; private set; }
        public GameEvents Events { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Bootstrap()
        {
            if (Instance == null)
                new GameObject("Scene Services").AddComponent<SceneServices>();
        }

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
            Debug.Log("SceneServices ready in " + SceneManager.GetActiveScene().name);
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }
    }
}
