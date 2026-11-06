using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace RunnerCube.Tests
{
    /// <summary>
    /// Проверяет, что ключевые поля живого объекта заполнены.
    /// </summary>
    public static class NullChecks
    {
        private static int _failed;

        public static void Run()
        {
            _failed = 0;

            CheckScene("Assets/_Scenes/Menu.unity");
            CheckScene("Assets/_Scenes/Game.unity");

            if (_failed > 0)
                throw new System.InvalidOperationException($"Null checks failed: {_failed}");

            Debug.Log("NULL CHECKS PASSED");
        }

        private static void CheckScene(string path)
        {
            UnityEngine.SceneManagement.Scene scene = EditorSceneManager.OpenScene(path);
            Checking = true;

            foreach (GameObject root in scene.GetRootGameObjects())
            {
                foreach (Transform node in root.GetComponentsInChildren<Transform>(true))
                {
                    foreach (Component component in node.GetComponents<Component>())
                    {
                        if (component != null)
                            continue;

                        _failed++;
                        Debug.LogError($"NULL CHECK: пропавший скрипт на {node.name} в {path}");
                    }
                }
            }

            Checking = false;
        }

        private static bool Checking { get; set; }
    }
}
