using UnityEngine;
using UnityEngine.EventSystems;

namespace RunnerCube.UI
{
    /// <summary>
    /// Пока курсор над меню, игрок не реагирует на клики. Пустое, пока нет забега.
    /// </summary>
    public sealed class UiPointerGuard : MonoBehaviour
    {
        public static bool IsPointerOverUi => EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }
}
