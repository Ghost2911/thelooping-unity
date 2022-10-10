using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserLine : MonoBehaviour
{
    public Transform startPos;
    public Transform endPos;
    public float speed = 5f;
    private Vector3 lastPos;
    private LineRenderer _line;

    private void OnEnable()
    {
        endPos = GetComponentInParent<Unit>().target;
        lastPos = endPos.position + new Vector3(0f,0f,2f);
    }

    private void Start()
    {
        startPos = transform;
        _line = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        lastPos = Vector3.MoveTowards(lastPos,endPos.position,Time.deltaTime*speed);
        if (endPos!=null)
            _line.SetPositions(new[] { startPos.position, lastPos });
    }
}
