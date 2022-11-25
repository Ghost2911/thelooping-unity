using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class QuestMessage : Message
{
    [TextArea]
    public string[] CompletedMessages;
    public bool isClaimedReward = false;
    public Quest quest;

    public void Start()
    {
        if (quest != null)
            Statistic.instance.GetUnityEvent(quest.completeCondition).AddListener(ConditionCheck);
        else
            Debug.Log($"{transform.name} doesnt have quest or questCondition: script QuestMessage");
    }

    public void ConditionCheck(string condition)
    {
        if (quest.completeName == condition)
            QuestCompleted();
    }

    public void QuestCompleted()
    {
        quest.ChangeQuestStatus(QuestStatus.Completed);
        Messages = CompletedMessages;
        messageNum = 0;
    }

    public override IEnumerator ShowText()
    {
        StartCoroutine(base.ShowText());

        if (quest.status.Equals(QuestStatus.Completed) && !isClaimedReward && messageNum == 1)
        {
            foreach (GameObject drop in quest.rewards)
                Instantiate(drop, new Vector3(transform.parent.position.x + 2f, 0.05f, transform.parent.position.z - 0.5f), Quaternion.identity);
            isClaimedReward = true;
        }
        yield return null;
    }
}
