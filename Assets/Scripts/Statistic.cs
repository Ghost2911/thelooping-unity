using UnityEngine;
using UnityEngine.Events;

public class Statistic : MonoBehaviour
{
    public static Statistic instance;
    public UnityEvent<string> OnDestroyObjectEvent;
    public UnityEvent<string> OnEnterInAreaEvent;
    public UnityEvent<string> OnItemUseEvent;
    public UnityEvent<string> OnItemInInventoryEvent;

    void Awake()
    {
        instance = this;
    }

    public UnityEvent<string> GetUnityEvent(QuestType questType)
    {
        switch (questType)
        {
            case QuestType.ItemUse:
                return OnItemUseEvent;
            case QuestType.ItemInInventory:
                return OnItemInInventoryEvent;
            case QuestType.EnterInArea:
                return OnEnterInAreaEvent;
            case QuestType.DestroyObject:
                return OnDestroyObjectEvent;
        }
        return null;
    }


    public void OnDestroyObject(string damageSource, string destroyedObject)
    {
        Debug.Log($"'{damageSource}' ���������� ������ '{destroyedObject}'");
        OnDestroyObjectEvent.Invoke(destroyedObject);
    }

    public void OnEnterInArea(string areaName)
    {
        Debug.Log($"�������� ������ � ������� {areaName}");
        OnEnterInAreaEvent.Invoke(areaName);
    }

    public void OnItemUse(string itemName)
    {
        Debug.Log($"�������� ���������� ������� {itemName}");
        OnItemUseEvent.Invoke(itemName);
    }
}
