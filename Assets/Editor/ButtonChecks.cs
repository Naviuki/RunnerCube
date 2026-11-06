using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace RunnerCube.Tests
{
    /// <summary>
    /// Проверяет, что у всех кнопок в сценах есть живые цели событий.
    /// </summary>
    public static class ButtonChecks
    {
        private static int _failed;

        public static void Run()
        {
            _failed = 0;
            Check("Assets/_Scenes/Menu.unity");
            Check("Assets/_Scenes/Game.unity");

            if (_failed > 0)
                throw new System.InvalidOperationException($"Button checks failed: {_failed}");

            Debug.Log("BUTTON CHECKS PASSED");
        }

        private static void Check(string path)
        {
            UnityEngine.SceneManagement.Scene scene = UnityEditor.SceneManagement.EditorSceneManager.OpenScene(path);
            foreach (GameObject root in scene.GetRootGameObjects())
            {
                foreach (Button button in root.GetComponentsInChildren<Button>(true))
                {
                    for (int i = 0; i < button.onClick.GetPersistentEventCount(); i++)
                    {
                        if (button.onClick.GetPersistentTarget(i) != null)
                            continue;

                        _failed++;
                        Debug.LogError($"BUTTON CHECK: пустая цель у {button.name} в {path}");
                    }
                }
            }
        }
    }
}
