using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace RunnerCube.Tests
{
    /// <summary>
    /// Проверки собранных сцен: нужные компоненты есть в обеих сценах.
    /// </summary>
    public static class SceneChecks
    {
        private static int _failed;

        public static void Run()
        {
            _failed = 0;
            CheckMenu();
            CheckGame();

            if (_failed > 0)
                throw new System.InvalidOperationException($"Scene checks failed: {_failed}");

            Debug.Log("SCENE CHECKS PASSED");
        }

        private static void CheckMenu()
        {
            UnityEngine.SceneManagement.Scene scene = EditorSceneManager.OpenScene("Assets/_Scenes/Menu.unity");
            Check(scene, "MenuController");
            Check(scene, "SceneFlow");
            Check(scene, "ShopController");
            Check(scene, "MenuToggle");
        }

        private static void CheckGame()
        {
            UnityEngine.SceneManagement.Scene scene = EditorSceneManager.OpenScene("Assets/_Scenes/Game.unity");
            Check(scene, "PlayerMotor");
            Check(scene, "PlayerContact");
            Check(scene, "TrackSpawner");
            Check(scene, "GameHud");
            Check(scene, "RunBootstrap");
        }

        private static void Check(UnityEngine.SceneManagement.Scene scene, string componentName)
        {
            foreach (GameObject root in scene.GetRootGameObjects())
            {
                foreach (MonoBehaviour behaviour in root.GetComponentsInChildren<MonoBehaviour>(true))
                {
                    if (behaviour != null && behaviour.GetType().Name == componentName)
                        return;
                }
            }

            _failed++;
            Debug.LogError($"SCENE CHECK FAILED: {componentName} не найден в {scene.name}");
        }
    }
}
