using UnityEngine;

[CreateAssetMenu(fileName = "StatusData", menuName = "Status (Create New)", order = 0)]
public class StatusData : ScriptableObject
{
    public Color color;
    public Sprite icon;
    public StatusType type;
    public RuntimeAnimatorController animator;
    public float duration = 10f;
    public float deltaTick;
    public int damagePerTick = 1;
}

