using System;
using UnityEditor.SceneManagement;

namespace RunnerCube.Tests
{
    /// <summary>
    /// Дымовые проверки сцен: на каждом корне сцены висят нужные компоненты.
    /// </summary>
    public static class SceneChecks
    {
        private static int _failed;

        public static void Run()
        {
            _failed = 0;

            CheckGame();
            CheckMenu();

            if (_failed > 0)
                throw new InvalidOperationException($"Scene checks failed: {_failed}");

            UnityEngine.Debug.Log("SCENE CHECKS PASSED");
        }

        private static void CheckGame()
        {
            UnityEngine.SceneManagement.Scene scene = EditorSceneManager.OpenScene("Assets/_Scenes/Game.unity");
            Check(scene, "Systems", typeof(Core.SceneServices));
            Check(scene, "Player", typeof(Player.PlayerMotor));
            Check(scene, "Player", typeof(Player.PlayerContact));
            Check(scene, "Player", typeof(Player.PlayerBonuses));
            Check(scene, "Track", typeof(Track.TrackSpawner));
            Check(scene, "Track", typeof(Track.RoadMover));
            Check(scene, "Track", typeof(Pooling.TrackFactory));
            Check(scene, "Bootstrap", typeof(Lifecycle.RunBootstrap));
            Check(scene, "Canvas", typeof(UI.GameHud));
        }

        private static void CheckMenu()
        {
            UnityEngine.SceneManagement.Scene scene = EditorSceneManager.OpenScene("Assets/_Scenes/Menu.unity");
            Check(scene, "Systems", typeof(Core.SceneServices));
            Check(scene, "Systems", typeof(Core.SceneFlow));
            Check(scene, "Canvas", typeof(UI.MenuController));
        }

        private static void Check(UnityEngine.SceneManagement.Scene scene, string rootName, Type component)
        {
            UnityEngine.GameObject root = Find(scene, rootName);
            if (root == null || root.GetComponent(component) == null)
            {
                _failed++;
                UnityEngine.Debug.LogError($"SCENE CHECK FAILED: {component.Name} on {rootName} in {scene.name}");
            }
        }

        private static UnityEngine.GameObject Find(UnityEngine.SceneManagement.Scene scene, string name)
        {
            foreach (UnityEngine.GameObject root in scene.GetRootGameObjects())
            {
                if (root.name == name)
                    return root;
            }

            return null;
        }
    }
}
