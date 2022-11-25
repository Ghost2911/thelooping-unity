using UnityEngine;

public class TotemPuzzle : MonoBehaviour
{
    public Totem[] totems;
    public GameObject reward;
    public enum RewardType {Destroy,Spawn};
    public RewardType rewardType;

    public void CheckActivation()
    {
        foreach (Totem totem in totems)
            if (!totem.isActive)
                return;

        if (rewardType == RewardType.Spawn)
            Instantiate(reward, transform.position, new Quaternion(0, 0, 0, 0));
        else
            Destroy(reward);
    }
}
