using UnityEngine;

[CreateAssetMenu(fileName = "StatusData", menuName = "Status (Create New)", order = 0)]
public class StatusData : ScriptableObject
{
    public Color color;
    public Sprite icon;
    public StatusType type;
    public StatusData additiveStatus;
    public RuntimeAnimatorController animator;
    public float duration = 10f;
    public int layer = 0;
    public float deltaTick = 3;
    public int damagePerTick = 0;
}

