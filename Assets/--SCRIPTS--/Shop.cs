using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject trader;
    public Item item;
    public SlotType category = SlotType.Body;
    public int shopCount = 3;
    public float rangeBetweenItems = 5f;

    void Start()
    {
        Vector3 startPos = transform.position - new Vector3((shopCount / 2f - 0.5f) * rangeBetweenItems, 0f, 0f);
        for (int i = 0; i < shopCount; i++)
        {
            item.stats = GlobalSettings.instance.itemPool.GetItemFromCategory(category);
            Item spawnedItem = Instantiate(item,
                startPos + new Vector3(i * rangeBetweenItems, 0, 0), Quaternion.identity, transform);
            spawnedItem.inMarket = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Vector3 startPos = transform.position - new Vector3((shopCount/2f-0.5f)*rangeBetweenItems,0f,0f);
        for (int i = 0; i < shopCount; i++)
            Gizmos.DrawSphere(startPos + new Vector3(i * rangeBetweenItems, 0, 0),0.5f);
    }
}
