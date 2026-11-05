using UnityEngine;
using UnityEngine.UI;

public class SwipeRotation : MonoBehaviour
{
    private Vector2 startPos;
    private Vector2 endPos;
    public float rotateSpeed = 0.2f;
    public Toggle toggle;
    public Animator animator;

    public Transform targetObject; // Объект для вращения
    public float rotationSpeed = -90f; // Скорость вращения (градусы в секунду)

    private bool isRotating = false;

    void Start()
    {
        // Проверьте, если toggle и targetObject заданы
        if (toggle == null || targetObject == null)
        {
            Debug.LogError("Toggle и/или целевой объект не заданы!");
            return;
        }

        // Добавление слушателя на изменение состояния чекбокса
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    void Update()
    {
        if (isRotating)
        {
            targetObject.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                endPos = touch.position;
                float swipeDistanceX = endPos.x - startPos.x;

                if (Mathf.Abs(swipeDistanceX) > 50) // Минимальное расстояние для считывания свайпа
                {
                    float rotationAmount = -swipeDistanceX * rotateSpeed * Time.deltaTime;
                    transform.Rotate(Vector3.up, rotationAmount);
                }
            }
        }
    }

    void OnToggleValueChanged(bool isOn)
    {
        // Установка флага вращения в зависимости от состояния галочки
        isRotating = isOn;
    }
    public void Animate()
    {
        animator.SetTrigger("ShopCharacters");
    }
}
