using UnityEngine;

public class BlessingStatue : MonoBehaviour, IUsable
{
    public StatusData blessStatus;
    public QuestMessage qmessage;

    public void Use(EntityStats entity)
    {
        entity.AddStatus(blessStatus);
        qmessage.StopAllCoroutines();
        StartCoroutine(qmessage.ShowText());
    }
}
