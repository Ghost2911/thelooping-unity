using System.Collections;
using UnityEngine;

public class TeleportInArea : MonoBehaviour
{
    public float time = 5f;
    public Transform trap;
    public Transform target;
    private bool isMove = false;

    public void SetMove(bool isMove)
    {
        StopAllCoroutines();
        if (isMove)
            StartCoroutine(TeleportInRange());

        this.isMove = isMove;
    }

    public void OnTriggerEnter(Collider other)
    { 
        if (other.CompareTag("Player"))
        {
            target = other.transform;
            SetMove(true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            SetMove(false);
    }

    IEnumerator TeleportInRange()
    {
        while (true)
        {
            trap.position = target.position + new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1));
            yield return new WaitForSeconds(time);
        }
    }

}
