using UnityEngine;

public class DragAndDrop3D : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    private float fixedZCoord; // Фиксированная координата Z

    [Header("Настройки колеса прокрутки")]
    [SerializeField] private float scrollSensitivity = 1.0f; // Чувствительность колеса прокрутки

    void OnMouseDown()
    {
        if (!isDragging)
        {
            fixedZCoord = Camera.main.WorldToScreenPoint(transform.position).z;

            // Вычисляем смещение между позицией объекта и позицией курсора
            Vector3 mouseWorldPos = GetMouseWorldPos(fixedZCoord);
            offset = transform.position - mouseWorldPos;

            isDragging = true;
        }
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            // Перемещаем объект, сохраняя фиксированную координату Z
            Vector3 mouseWorldPos = GetMouseWorldPos(fixedZCoord);
            Vector3 newPosition = mouseWorldPos + offset;

            // Запрет на установку Y меньше нуля
            if (newPosition.y < 0)
            {
                newPosition.y = 0;
            }

            transform.position = newPosition;

            // Обработка колеса прокрутки для перемещения по оси Z
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0)
            {
                // Изменяем позицию объекта по оси Z
                Vector3 newZPosition = transform.position + new Vector3(0, 0, scroll * scrollSensitivity);
                transform.position = newZPosition;

                // Обновляем фиксированную координату Z
                //fixedZCoord = Camera.main.WorldToScreenPoint(transform.position).z;
            }
        }
    }

    void OnMouseUp()
    {
        isDragging = false;
    }

    private Vector3 GetMouseWorldPos(float zCoord)
    {
        // Получаем позицию курсора в мировых координатах
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord; // Используем фиксированную координату Z
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}