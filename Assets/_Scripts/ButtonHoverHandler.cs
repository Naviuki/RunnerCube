using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Animator Gray;
    public Animator Alfedov;
    public Animator Melon; // Ссылка на аниматор кнопки

    void Start()
    {
        Gray.Play("Gray");
    }


    // Этот метод вызывается, когда указатель мыши начинает находиться над кнопкой
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Gray != null)
        {
            Gray.SetTrigger("Gray"); // Замените "Hover" на триггер вашей анимации
            Gray.SetBool("MainCharacter", false);
        }
    }

    // Этот метод вызывается, когда указатель мыши покидает область кнопки
    public void OnPointerExit(PointerEventData eventData)
    {
        if (Gray != null)
        {
            Gray.SetTrigger("Gray");
            Gray.SetBool("MainCharacter", true); // Замените "Normal" на триггер для возвращения в исходное состояние
        }
    }
}
