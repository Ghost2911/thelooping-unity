using UnityEngine;

[CreateAssetMenu(fileName = "Psyhotype", menuName = "Statistics/Psyhotype", order = 0)]
public class PsyhotypeData: ScriptableObject
{   
    public int maxPoints = 1000;
    private int[] points = new int[typeof(Psyhotype).GetFields().Length];

    public void AddPoints(Psyhotype psyhotype, int additivePoints)
    {
        points[(int)psyhotype] += additivePoints;
        if (points[(int)psyhotype] > maxPoints)
            points[(int)psyhotype] = maxPoints;
    }

    public void Clear()
    {
        for (int i=0; i<points.Length-1; i++)
            points[i] = 0;
    }

    public void PrintPoints()
    {
        Debug.Log($"---------------- Psyhotype PlayerData ----------------"); 
        for (int i=0; i<points.Length; i++)
            Debug.Log($"Psyhotype: {(Psyhotype)i}    points: {points[i]}"); 
    }
}
public enum Psyhotype {Killer,Socializer,Explorer,Achievers}

