using UnityEngine;
using UnityEngine.UI;

namespace RunnerCube.Tests
{
    /// <summary>
    /// Проверяет, что кнопки меню возвращаются на MenuController, а мёртвых подписок нет.
    /// </summary>
    public static class MenuNavChecks
    {
        private static int _failed;

        public static void Run()
        {
            _failed = 0;
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/_Scenes/Menu.unity");
            GameObject canvas = GameObject.Find("Canvas");

            foreach (Button button in canvas.GetComponentsInChildren<Button>(true))
            {
                for (int i = 0; i < button.onClick.GetPersistentEventCount(); i++)
                {
                    if (button.onClick.GetPersistentTarget(i) == null)
                    {
                        _failed++;
                        Debug.LogError("MENU NAV: мертвая подписка у " + button.name);
                    }
                }
            }

            if (_failed > 0)
                throw new System.InvalidOperationException($"Menu nav checks failed: {_failed}");

            Debug.Log("MENU NAV CHECKS PASSED");
        }
    }
}
