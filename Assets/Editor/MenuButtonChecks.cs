using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RunnerCube.Tests
{
    /// <summary>
    /// Проверяет, что у кнопок в сцен�� меню не осталось ссылок на классы вне проекта.
    /// </summary>
    public static class MenuButtonChecks
    {
        private static readonly HashSet<string> Expected = new HashSet<string>
        {
            "Play", "Quit", "ToggleOptions", "ToggleShop", "ToggleAbout", "ToggleSound",
            "DeleteProgress", "SetRussian", "SetEnglish", "SetTatar", "ClosePanels",
            "NextSkin", "PreviousSkin", "BuySkin"
        };

        private static int _failed;

        public static void Run()
        {
            _failed = 0;
            UnityEngine.SceneManagement.Scene scene = UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/_Scenes/Menu.unity");

            foreach (GameObject root in scene.GetRootGameObjects())
            {
                foreach (UnityEngine.UI.Button button in root.GetComponentsInChildren<UnityEngine.UI.Button>(true))
                {
                    for (int i = 0; i < button.onClick.GetPersistentEventCount(); i++)
                    {
                        Object target = button.onClick.GetPersistentTarget(i);
                        string method = button.onClick.GetPersistentMethodName(i);

                        if (target == null)
                        {
                            _failed++;
                            Debug.LogError($"MENU BUTTON: пустая цель у {button.name} ({method})");
                            continue;
                        }

                        if (target is Component component && component.GetType().Assembly == typeof(MenuButtonChecks).Assembly)
                        {
                            if (!Expected.Contains(method))
                            {
                                _failed++;
                                Debug.LogError($"MENU BUTTON: {button.name} ведёт на {component.GetType().Name}.{method}");
                            }
                        }
                    }
                }
            }

            if (_failed > 0)
                throw new System.InvalidOperationException($"Menu button checks failed: {_failed}");

            Debug.Log("MENU BUTTON CHECKS PASSED");
        }
    }
}
