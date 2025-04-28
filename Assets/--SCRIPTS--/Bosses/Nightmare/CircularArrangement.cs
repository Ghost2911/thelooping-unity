using UnityEngine;

public class CircularMovement : MonoBehaviour
{
    [Header("Настройки окружности")]
    public float radius = 2f;
    public float speed = 30f;    
    public bool clockwise = true;    

    private Transform[] children;   
    private float[] angles;            
    private Vector3[] originalRotations; 

    void Start()
    {
        InitializeChildren();
        transform.rotation = Quaternion.Euler(new Vector3(-45,0,0)); 
    }

    void Update()
    {
        MoveChildren();
    }

    // Инициализация дочерних объектов
    private void InitializeChildren()
    {
        int childCount = transform.childCount;
        children = new Transform[childCount];
        angles = new float[childCount];
        originalRotations = new Vector3[childCount];

        float angleStep = 360f / childCount;

        for (int i = 0; i < childCount; i++)
        {
            children[i] = transform.GetChild(i);
            originalRotations[i] = children[i].rotation.eulerAngles;
            angles[i] = angleStep * i;
        }
    }

    // Перемещение объектов по окружности без изменения их поворота
    private void MoveChildren()
    {
        float rotationSpeed = speed * Time.deltaTime;
        if (!clockwise) rotationSpeed *= -1;

        for (int i = 0; i < children.Length; i++)
        {
            angles[i] += rotationSpeed;
            float radianAngle = angles[i] * Mathf.Deg2Rad;

            // Позиция на окружности (в локальных координатах родителя)
            Vector3 localPosition = new Vector3(
                Mathf.Cos(radianAngle) * radius,
                0,
                Mathf.Sin(radianAngle) * radius
            );

            // Применяем позицию и сохраняем оригинальный поворот
            children[i].localPosition = localPosition;
            children[i].rotation = Quaternion.Euler(originalRotations[i]);
        }
    }

    public void AddSkull()
    {
        Transform[] inactiveChildren = System.Array.FindAll(children, child => !child.gameObject.activeSelf);
        
        if (inactiveChildren.Length > 0)
        {
            int randomIndex = Random.Range(0, inactiveChildren.Length);
            inactiveChildren[randomIndex].gameObject.SetActive(true);
            Debug.Log($"Активирован объект: {inactiveChildren[randomIndex].name}");
        }
        else
        {
            Debug.LogWarning("Нет неактивных объектов для активации!");
        }
    }

    public void RemoveSkull()
    {
        Transform[] activeChildren = System.Array.FindAll(children, child => child.gameObject.activeSelf);
        
        if (activeChildren.Length > 0)
        {
            int randomIndex = Random.Range(0, activeChildren.Length);
            activeChildren[randomIndex].gameObject.SetActive(false);
            Debug.Log($"Деактивирован объект: {activeChildren[randomIndex].name}");
        }
        else
        {
            Debug.LogWarning("Нет активных объектов для деактивации!");
        }
    }
}