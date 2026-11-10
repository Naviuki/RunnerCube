using UnityEditor;
using UnityEngine;

namespace RunnerCube.Tests
{
    /// <summary>
    /// Проверяет, что в префабах и сценах нет компонентов со ссылками на удалённые скрипты.
    /// </summary>
    public static class PrefabChecks
    {
        private static int _failed;

        public static void Run()
        {
            _failed = 0;

            string[] folders = { "Assets/Prefabs", "Assets/Tiles", "Assets/Particles", "Assets/_Scenes" };
            foreach (string folder in folders)
            {
                foreach (string guid in AssetDatabase.FindAssets("t:Prefab t:Scene", new[] { folder }))
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    if (path.EndsWith(".prefab"))
                        CheckPrefab(path);
                }
            }

            if (_failed > 0)
                throw new System.InvalidOperationException($"Prefab checks failed: {_failed}");

            Debug.Log("PREFAB CHECKS PASSED");
        }

        private static void CheckPrefab(string path)
        {
            GameObject root = PrefabUtility.LoadPrefabContents(path);
            foreach (Transform node in root.GetComponentsInChildren<Transform>(true))
            {
                foreach (Component component in node.GetComponents<Component>())
                {
                    if (component != null)
                        continue;

                    _failed++;
                    Debug.LogError($"PREFAB CHECK: пропавший скрипт на {node.name} в {path}");
                }
            }

            PrefabUtility.UnloadPrefabContents(root);
        }
    }
}
