using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace RunnerCube.Tests
{
    /// <summary>
    /// Ссылки HUD, меню и магазина заполнены, события кнопок не пустуют.
    /// </summary>
    public static class LinkChecks
    {
        private static int _failed;

        public static void Run()
        {
            _failed = 0;

            UnityEngine.SceneManagement.Scene game = EditorSceneManager.OpenScene("Assets/_Scenes/Game.unity", OpenSceneMode.Single);
            UI.GameHud hud = GameObject.Find("Canvas").GetComponent<UI.GameHud>();
            Check("GameHud.scoreLabel", Field(hud, "scoreLabel"));
            Check("GameHud.coinsLabel", Field(hud, "coinsLabel"));
            Check("GameHud.pausePanel", Field(hud, "pausePanel"));
            Check("GameHud.losePanel", Field(hud, "losePanel"));
            Check("GameHud.pauseButton", Field(hud, "pauseButton"));
            Check("Game events", NoEmptyEvents(game));

            UnityEngine.SceneManagement.Scene menu = EditorSceneManager.OpenScene("Assets/_Scenes/Menu.unity", OpenSceneMode.Single);
            UI.MenuController controller = GameObject.Find("Canvas").GetComponent<UI.MenuController>();
            Check("MenuController.scenes", Field(controller, "scenes"));
            Check("MenuController.coinsLabel", Field(controller, "coinsLabel"));
            Check("MenuController.recordLabel", Field(controller, "recordLabel"));
            Check("MenuController.soundIcon", Field(controller, "soundIcon"));
            Check("MenuController.soundOn", Field(controller, "soundOn"));
            Check("MenuController.soundOff", Field(controller, "soundOff"));

            UI.ShopController shop = Object.FindAnyObjectByType<UI.ShopController>(FindObjectsInactive.Include);
            Check("ShopController.catalog", Field(shop, "catalog"));
            Check("ShopController.priceText", Field(shop, "priceText"));
            Check("ShopController.preview", Field(shop, "preview"));
            Check("Menu events", NoEmptyEvents(menu));

            if (_failed > 0)
                throw new System.InvalidOperationException($"Link checks failed: {_failed}");

            Debug.Log("LINK CHECKS PASSED");
        }

        private static bool Field(Component target, string name)
        {
            if (target == null)
                return false;

            SerializedProperty property = new SerializedObject(target).FindProperty(name);
            return property != null && property.objectReferenceValue != null;
        }

        private static bool NoEmptyEvents(UnityEngine.SceneManagement.Scene scene)
        {
            foreach (GameObject root in scene.GetRootGameObjects())
            {
                foreach (UnityEngine.UI.Button button in root.GetComponentsInChildren<UnityEngine.UI.Button>(true))
                {
                    for (int i = 0; i < button.onClick.GetPersistentEventCount(); i++)
                    {
                        if (button.onClick.GetPersistentTarget(i) == null)
                            return false;
                    }
                }
            }

            return true;
        }

        private static void Check(string name, bool ok)
        {
            if (ok)
                return;

            _failed++;
            Debug.LogError("LINK CHECK FAILED: " + name);
        }
    }
}
