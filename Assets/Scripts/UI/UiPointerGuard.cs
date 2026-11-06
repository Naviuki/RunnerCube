using UnityEngine;
using UnityEngine.EventSystems;

namespace RunnerCube.UI
{
    /// <summary>
    /// Пока курсор над панелью интерфейса, игрок не реагирует на клики.
    /// </summary>
    public static class UiPointerGuard
    {
        public static bool IsPointerOverUi
        {
            get
            {
                EventSystem system = EventSystem.current;
                return system != null && system.IsPointerOverGameObject();
            }
        }
    }
}
