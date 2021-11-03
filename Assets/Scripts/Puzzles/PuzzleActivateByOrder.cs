using UnityEngine;

public class PuzzleActivateByOrder : MonoBehaviour
{
    public GameObject prizeItem;

    private Statue[] elements;
    private int stage = 1;
    private int[] activateOrder;

    void Start()
    {
        elements = transform.GetComponentsInChildren<Statue>();
        activateOrder = ActivateCombination(transform.childCount);
        for (int i = 0; i < activateOrder.Length; i++)
        {
            elements[i].number = activateOrder[i];
        }
    }

    public void OnActivate(int elementsNumber)
    {
        if (elementsNumber == stage)
        {
            stage++;
            if (stage == activateOrder.Length + 1)
                PuzzleResolved();
        }
        else
        {
            stage = 1;
            foreach (Statue element in elements)
                element.SetStatueStatus(false);
        }
    }

    private int[] ActivateCombination(int arrayLength)
    {
        int[] array = new int[arrayLength];
        int temporaryValue, randomIndex, currentIndex = array.Length;

        for (int i = 1; i <= array.Length; i++)
            array[i-1] = i;

        while (0 != currentIndex)
        {
            randomIndex = Random.Range(0,array.Length);
            currentIndex -= 1;

            temporaryValue = array[currentIndex];
            array[currentIndex] = array[randomIndex];
            array[randomIndex] = temporaryValue;
        }

        return array;
    }
    public void PuzzleResolved()
    {
        foreach (Statue element in elements)
            element.enabled = false;
        Instantiate(prizeItem, transform.position, new Quaternion(0, 0, 0, 0));
    }
}
