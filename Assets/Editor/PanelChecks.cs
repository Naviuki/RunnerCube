using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace RunnerCube.Tests
{
    /// <summary>
    /// Проверяет, что панели пауз и проигрыша спрятаны на старте, а меню видно.
    /// </summary>
    public static class PanelChecks
    {
        private static int _failed;

        public static void Run()
        {
            _failed = 0;

            UnityEngine.SceneManagement.Scene game = EditorSceneManager.OpenScene("Assets/_Scenes/Game.unity");
            Check(game, "PausePanel", false);
            Check(game, "Lose Panel", false);
            Check(game, "Pause Button", true);

            UnityEngine.SceneManagement.Scene menu = EditorSceneManager.OpenScene("Assets/_Scenes/Menu.unity");
            Check(menu, "Canvas", true);
            Check(menu, "Main Menu", true);

            if (_failed > 0)
                throw new System.InvalidOperationException($"Panel checks failed: {_failed}");

            Debug.Log("PANEL CHECKS PASSED");
        }

        private static void Check(UnityEngine.SceneManagement.Scene scene, string name, bool expected)
        {
            GameObject node = null;
            foreach (GameObject root in scene.GetRootGameObjects())
            {
                foreach (Transform child in root.GetComponentsInChildren<Transform>(true))
                {
                    if (child.name != name)
                        continue;

                    node = child.gameObject;
                    break;
                }

                if (node != null)
                    break;
            }

            if (node == null)
            {
                _failed++;
                Debug.LogError("PANEL CHECK: нет " + name);
                return;
            }

            if (node.activeSelf != expected)
            {
                _failed++;
                Debug.LogError($"PANEL CHECK: {name} active={node.activeSelf}, ожидалось {expected}");
            }
        }
    }
}
