using UnityEngine;

[CreateAssetMenu(fileName = "LiquidState0", menuName = "LiquidState")]
public class LiquidState: ScriptableObject
{
    public string name;
    public Sprite sprite;
    public RuntimeAnimatorController animator;
    public Color color;
    public LiquidState[] stateTransitionOptions;
    public StatusData statusTransitionCondition;
    public StatusData statusOnStay;
    public StatusData statusOnStart;
}
