using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserLine : MonoBehaviour
{
    public Transform startPos;
    public Transform endPos;
    private LineRenderer _line;

    private void Start()
    {
        _line = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        _line.SetPositions(new[] { startPos.position, endPos.position });
    }
}
