using System.Collections.Generic;
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
                    if (button.onClick.GetPersistentEventCount() == 0)
                    {
                        _failed++;
                        Debug.LogError($"BUTTON CHECK: у кнопки {button.name} в {path} нет обработчика");
                    }

                    for (int i = 0; i < button.onClick.GetPersistentEventCount(); i++)
                    {
                        if (button.onClick.GetPersistentTarget(i) != null)
                            continue;

                        _failed++;
                        Debug.LogError($"BUTTON CHECK: пустая цель у {button.name} в {path}");
                    }
                }

                foreach (MonoBehaviour behaviour in root.GetComponentsInChildren<MonoBehaviour>(true))
                    CheckEvents(behaviour, path);
            }
        }

        private static void CheckEvents(MonoBehaviour behaviour, string path)
        {
            SerializedObject serialized = new SerializedObject(behaviour);
            List<string> paths = new List<string>();
            SerializedProperty iterator = serialized.GetIterator();

            while (iterator.NextVisible(true))
            {
                if (iterator.name == "m_PersistentCalls")
                    paths.Add(iterator.propertyPath);
            }

            foreach (string eventPath in paths)
            {
                SerializedProperty calls = serialized.FindProperty(eventPath).FindPropertyRelative("m_Calls");
                for (int i = 0; i < calls.arraySize; i++)
                {
                    SerializedProperty call = calls.GetArrayElementAtIndex(i);
                    Object receiver = call.FindPropertyRelative("m_Target").objectReferenceValue;
                    string method = call.FindPropertyRelative("m_MethodName").stringValue;

                    if (receiver != null && !string.IsNullOrEmpty(method))
                        continue;

                    _failed++;
                    Debug.LogError($"BUTTON CHECK: пустой вызов у {behaviour.name} в {path}");
                }
            }
        }
    }
}
