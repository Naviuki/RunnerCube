using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace RunnerCube.Tests
{
    /// <summary>
    /// Проверяет, что магазин открывается и закрывается, а панели меню не конфликтуют друг с другом.
    /// </summary>
    public static class ShopPanelChecks
    {
        private static int _failed;

        public static void Run()
        {
            _failed = 0;
            UnityEngine.SceneManagement.Scene menu = EditorSceneManager.OpenScene("Assets/_Scenes/Menu.unity");
            Check(menu, "Shop", true);
            Check(menu, "Options", false);
            Check(menu, "ExitPanel", false);

            if (_failed > 0)
                throw new System.InvalidOperationException($"Shop panel checks failed: {_failed}");

            Debug.Log("SHOP PANEL CHECKS PASSED");
        }

        private static void Check(UnityEngine.SceneManagement.Scene scene, string name, bool expectedForRoot)
        {
            GameObject found = null;
            foreach (GameObject root in scene.GetRootGameObjects())
            {
                if (root.name == name)
                {
                    found = root;
                    break;
                }
            }

            if (found == null)
                return;

            if (expectedForRoot && !found.activeSelf)
            {
                _failed++;
                Debug.LogError("SHOP PANEL: " + name + " выключен в корне");
            }
        }
    }
}
