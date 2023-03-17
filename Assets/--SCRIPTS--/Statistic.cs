using UnityEngine;
using UnityEngine.Events;

public class Statistic : MonoBehaviour
{
    public static Statistic instance;
    public UnityEvent<string> OnDestroyObjectEvent;
    public UnityEvent<string> OnEnterInAreaEvent;
    public UnityEvent<string> OnItemUseEvent;
    public UnityEvent<string> OnItemInInventoryEvent;
    public PsyhotypeData psyhotypeData;

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

    public void AddPsyhotypePoints()
    {
        
    }

    public void OnDestroyObject(string damageSource, string destroyedObject)
    {
        Debug.Log($"'{damageSource}' разрушает объект '{destroyedObject}'");
        OnDestroyObjectEvent.Invoke(destroyedObject);
    }

    public void OnEnterInArea(string areaName)
    {
        Debug.Log($"Игрок входит в зону - {areaName}");
        OnEnterInAreaEvent.Invoke(areaName);
    }

    public void OnItemUse(string itemName)
    {
        Debug.Log($"Игрок использует предмет - {itemName}");
        OnItemUseEvent.Invoke(itemName);
    }
}
