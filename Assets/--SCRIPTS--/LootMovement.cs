using UnityEngine;

public class LootMovement : MonoBehaviour
{
    private Vector3 targetPosition;
    public float lootSpeed = 5f;
    public float lootRange = 5f;
    void Start()
    {
        targetPosition = transform.position+RandomVector(-lootRange, lootRange);
    }
    void Update()
    {
        if (transform.position == targetPosition)
            Destroy(this);
        else
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, lootSpeed * Time.deltaTime);
    }
    private Vector3 RandomVector(float min, float max)
    {
        var x = Random.Range(min, max);
        var y = Random.Range(min, max);
        return new Vector3(x, 0, y);
    }
}
