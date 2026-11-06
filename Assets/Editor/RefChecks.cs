using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace RunnerCube.Tests
{
    /// <summary>
    /// Проверяет, что критические ссылки игровых компонентов заданы в сцене игры.
    /// </summary>
    public static class RefChecks
    {
        private static int _failed;

        public static void Run()
        {
            _failed = 0;

            UnityEngine.SceneManagement.Scene scene = EditorSceneManager.OpenScene("Assets/_Scenes/Game.unity");
            CheckReferences(scene);

            if (_failed > 0)
                throw new System.InvalidOperationException($"Ref checks failed: {_failed}");

            Debug.Log("REF CHECKS PASSED");
        }

        private static void CheckReferences(UnityEngine.SceneManagement.Scene scene)
        {
            Check<UI.GameHud>(scene, "scoreLabel", "pausePanel", "losePanel");
            Check<Player.PlayerMotor>(scene, "settings", "body", "input");
            Check<Track.TrackSpawner>(scene, "settings", "factory", "player");
            Check<Lifecycle.RunBootstrap>(scene, "player", "track", "mover");
        }

        private static void Check<T>(UnityEngine.SceneManagement.Scene scene, params string[] fields) where T : Component
        {
            T target = null;
            foreach (GameObject root in scene.GetRootGameObjects())
            {
                target = root.GetComponentInChildren<T>(true);
                if (target != null)
                    break;
            }

            if (target == null)
            {
                _failed++;
                Debug.LogError("REF CHECK: нет " + typeof(T).Name);
                return;
            }

            SerializedObject so = new SerializedObject(target);
            foreach (string field in fields)
            {
                SerializedProperty property = so.FindProperty(field);
                if (property != null && property.objectReferenceValue != null)
                    continue;

                _failed++;
                Debug.LogError($"REF CHECK: {typeof(T).Name}.{field} пустой");
            }
        }
    }
}
