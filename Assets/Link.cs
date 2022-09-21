using UnityEngine;

public class Link : MonoBehaviour
{
    public Transform startPosition;
    public Transform endPosition;
    public float offsetSpeed = 5f;
    public float distance = 0;
    private LineRenderer line;
    private Material material;
    public Vector3 offsetTarget = new Vector3(0, 0.75f, 0.75f);
    public Vector3 offsetHolder = new Vector3(0, 0.75f, 0.75f);

    private void Start()
    {
        line = GetComponent<LineRenderer>();
        line.enabled = false;
        material = line.material;
    }

    public void ClearPosition()
    {
        line.enabled = false;
        startPosition = null;
        endPosition = null;
        distance = 0;
    }

    public void SetPosition(Transform startPos, Transform endPos)
    {
        startPosition = startPos;
        endPosition = endPos;
        line.enabled = true;
    }

    void Update()
    {
        try
        {
            if (line.enabled)
            {
                line.SetPosition(0, startPosition.position + offsetHolder);
                line.SetPosition(1, endPosition.position + offsetTarget);
                distance = Vector3.Distance(startPosition.position, endPosition.position);
                material.mainTextureOffset = new Vector2(Time.realtimeSinceStartup * offsetSpeed, 0);
            }
        }
        catch
        {
            line.enabled = false;
        }
    }
}
