using System.Collections;
using TMPro;
using UnityEngine;

public class CraftTable : MonoBehaviour, IUsable
{
    [Header("Settings")]
    public CollectableType mainResource;
    public int mainResourceCount;
    public SlotType rewardType;

    public TextMeshProUGUI textMesh;
    public GameObject craftAnimator;
    private bool canCraft = true;
    private Collider tableCollider;
    private Item craftedItem;

    IEnumerator CraftItem()
    {   
        if (tableCollider==null)
            tableCollider = GetComponent<Collider>();
        for (int i=0;i<mainResourceCount;i++)
        {
            textMesh.text += $"<sprite name=\"icon_res_{mainResource.ToString()}\">";
            yield return new WaitForSeconds(0.4f);
        }
        craftAnimator.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        craftedItem = GlobalSettings.instance.CreateItem(rewardType, transform.position + new Vector3(0, 0.6f, 0.4f));
        textMesh.text = string.Empty;
        StartCoroutine(WaitPlayerPickupItem());
    }

    IEnumerator WaitPlayerPickupItem()
    {
        tableCollider.enabled = false;
        canCraft = false;
        while(craftedItem!=null)
            yield return new WaitForSeconds(0.3f);
        canCraft = true;
        craftAnimator.SetActive(false);
        tableCollider.enabled = true;
    }

    public void Use(EntityStats entity)
    {
        if (canCraft)
            if (Inventory.instance.ChangeCollectableItem(mainResource, -mainResourceCount))
                StartCoroutine(CraftItem());
    }

}
