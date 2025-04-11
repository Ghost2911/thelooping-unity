using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcFlight : MonoBehaviour
{
    public Vector3 target;
    public Renderer renderer;
    public DamageType damageType = DamageType.Impact;
    public float speed = 5;
    private float time = 0;
    private float rotation = 0;
    private Vector3 startPos;

    
    public float initialSpeed = 8f; // Начальная скорость
    public float gravity = 15f; // Ускорение свободного падения

    private Vector3 initialPosition; // Исходная позиция (точка А)
    private Vector3 targetPosition; // Позиция цели (точка Б)
    private Vector3 velocity; // Вектор скорости

    void Start()
    {
        initialPosition = transform.position; // Запоминаем начальную позицию
        targetPosition = target; // Получаем позицию цели

        // Рассчитываем начальную скорость, чтобы попасть в цель
        CalculateLaunchVelocity();
        transform.Rotate(new Vector3(0, 0, 15));
        renderer = transform.GetComponentInChildren<Renderer>();
        renderer.material.SetFloat("_Offset", 1f);
    }

    void Update()
    {
        // Перемещаем объект с учетом гравитации и начальной скорости
        if (targetPosition != initialPosition)
        {
            transform.position += velocity * Time.deltaTime;
            velocity.y -= gravity * Time.deltaTime; // Применяем гравитацию по оси Y

            renderer.material.SetFloat("_RotateAngle", rotation);
            time += Time.deltaTime * speed;
            rotation -= Time.deltaTime * 20;


            // Прекращаем движение, если объект достиг цели
            if (transform.position.y <= 0f)
            {
                transform.rotation = new Quaternion(0, 0, 0, 0);
                transform.GetComponentInChildren<Collider>().enabled = true;
                Collider[] hitEnemies = Physics.OverlapSphere(transform.position, 0.5f);

                foreach (Collider enemy in hitEnemies)
                {
                    IDamageable damagable = enemy.GetComponent<IDamageable>();
                    damagable?.Damage(new HitInfo(damageType,5,0f,Vector3.zero,Color.red));
                }
                renderer.material.SetFloat("_RotateAngle", 0f);
                renderer.material.SetFloat("_Offset", 0f);
                Destroy(this);
                return;
            }
        }
    }

    void CalculateLaunchVelocity()
    {
        Vector3 displacement = targetPosition - initialPosition; // Вектор от A к B
        float horizontalDistance = new Vector2(displacement.x, displacement.z).magnitude; // Горизонтальное расстояние

        // Рассчитываем угол, под которым объект должен быть запущен
        float angle = 45f; // Мы предполагаем угол 45 градусов для наилучшей дуги
        float radianAngle = angle * Mathf.Deg2Rad; // Переводим угол в радианы

        // Рассчитываем компоненты скорости
        velocity = new Vector3(displacement.x, 0f, displacement.z).normalized * initialSpeed;
        velocity.y = horizontalDistance * Mathf.Tan(radianAngle); // Начальная вертикальная скорость

        // Корректируем скорость по вертикали с учетом гравитации
        velocity.y = Mathf.Sqrt(horizontalDistance * gravity / Mathf.Sin(2 * radianAngle));
    }
}



