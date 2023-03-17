using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBetween : MonoBehaviour
{
    public Transform[] positions;
    public float speed = 5;
    private bool isMove = false;
    private byte cursor = 0;

    public void SetMove(bool isMove)
    {
        StopAllCoroutines();
        if (isMove)
            StartCoroutine(MoveObject());
        
        this.isMove = isMove;
    }

    IEnumerator MoveObject()
    {
        float delta = 0;
        while (Vector3.Distance(transform.position, positions[cursor].position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, positions[cursor].position,delta);
            delta += Time.deltaTime * speed;
            yield return new WaitForSeconds(0.01f);
        }
        if (cursor < positions.Length-1)
            cursor++;
        else
            cursor = 0;
        StartCoroutine(MoveObject());
    }

}
