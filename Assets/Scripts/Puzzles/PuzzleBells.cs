using System.Collections;
using UnityEngine;

public class PuzzleBells : MonoBehaviour
{
    public GameObject prizeItem;
    public Sprite spriteResolved;

    private Bell[] elements;
    private int stage = 0;
    private int[] activateOrder;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        elements = transform.GetComponentsInChildren<Bell>();
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        GetComponentInParent<LocationTrigger>().OnLocationEnter.AddListener(ResetPuzzle);
    }

    public void ResetPuzzle()
    {
        StopAllCoroutines();
        stage = 0;
        activateOrder = ActivateCombination(transform.childCount);
        StartCoroutine(AnimateBells(activateOrder));
    }

    public void OnActivate(int elementsNumber)
    {
        if (elementsNumber == activateOrder[stage++])
        {
            if (stage == activateOrder.Length)
                PuzzleResolved();
        }
        else
        {
            ResetPuzzle();
        }
    }

    private int[] ActivateCombination(int arrayLength)
    {
        int[] array = new int[arrayLength];
        int temporaryValue, randomIndex, currentIndex = array.Length;

        for (int i = 1; i <= array.Length; i++)
            array[i - 1] = i;

        while (0 != currentIndex)
        {
            randomIndex = Random.Range(0, array.Length);
            currentIndex -= 1;

            temporaryValue = array[currentIndex];
            array[currentIndex] = array[randomIndex];
            array[randomIndex] = temporaryValue;
        }

        return array;
    }
    public void PuzzleResolved()
    {
        spriteRenderer.sprite = spriteResolved;
        Instantiate(prizeItem, transform.position, new Quaternion(0, 0, 0, 0));
    }

    IEnumerator AnimateBells(int[] array)
    {
        yield return new WaitForSeconds(1f);
        foreach (int i in array)
        {
            elements[i-1].animator.Play(0);
            yield return new WaitForSeconds(1f);
        }
    }
}
