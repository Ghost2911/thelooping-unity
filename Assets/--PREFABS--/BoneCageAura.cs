using UnityEngine;
using System.Collections.Generic;

public class BoneCageAura : Status
{
    [Header("Settings")]
    public string targetTag = "enemy";
    public float radius = 10f;
    public Color lineColor = Color.white;
    private Vector3 offset = new Vector3(0f, 0.1f, 0f);
    public float lineWidth = 0.1f;
    private List<Transform> nearbyObjects = new List<Transform>();

    private int additiveArmor = 0;
    private LineRenderer lineRenderer;

    public override void OnActivate() 
    { 
        targetTag = target.gameObject.tag;
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
    } 

    public override void Tick()
    {
        int prevAdditiveArmor = additiveArmor;
        FindNearbyObjects();
        DrawLines();
        target.armor += additiveArmor - prevAdditiveArmor;
    }

    void FindNearbyObjects()
    {
        additiveArmor = 0;
        nearbyObjects.Clear();
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(targetTag);

        foreach (GameObject obj in taggedObjects)
        {
            float distance = Vector3.Distance(transform.position, obj.transform.position);
            if (distance <= radius)
            {
                nearbyObjects.Add(obj.transform);
                additiveArmor += 3;
            }
        }
    }

    void DrawLines()
    {
        lineRenderer.positionCount = nearbyObjects.Count * 2;
        int index = 0;

        foreach (Transform target in nearbyObjects)
        {
            if (target != null)
            {
                lineRenderer.SetPosition(index, transform.position + offset);
                lineRenderer.SetPosition(index + 1, target.position + offset);
                index += 2;
            }
        }
    }
}