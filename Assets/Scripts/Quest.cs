using UnityEngine;

[CreateAssetMenu(fileName = "Quest0", menuName = "Quest", order = 0)]
public class Quest : ScriptableObject
{
    public string name;
    public string description;
    public QuestType completeCondition;
    public string completeName;
    public int completeCount = 1;
    public QuestType failCondition;
    public string failName;
    public int failCount = 1;
    public GameObject[] rewards;

    public string[] CompletedMessages;
    public QuestStatus status;

    public void ChangeQuestStatus(QuestStatus status)
    {
        if (QuestStatus.Await == status)
            this.status = status;
        Debug.Log($"Quest {name} - status: {this.status}");
    }
}

public enum QuestType
{
    EnterInArea, ItemUse, ItemInInventory, DestroyObject
}

public enum QuestStatus
{
    Await, Completed, Failed
}
