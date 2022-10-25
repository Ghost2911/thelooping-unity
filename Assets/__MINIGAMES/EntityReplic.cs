using UnityEngine;

public class EntityReplic<T>
{
    [SerializeField]
    public T text;
    [SerializeField]
    public float time;
    [SerializeField]
    public Color color;
    [SerializeField]
    public Side side;
}

public enum Side { left, right}

[System.Serializable]
public class Replic : EntityReplic<string> { }
