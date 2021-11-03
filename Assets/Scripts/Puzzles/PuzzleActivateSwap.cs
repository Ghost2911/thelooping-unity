using UnityEngine;

public class PuzzleActivateSwap : MonoBehaviour
{
    public GameObject prizeItem;
    private Statue[] elements;

    void Start()
    {
        elements = transform.GetComponentsInChildren<Statue>();
        for (int i = 0; i < elements.Length; i++)
        {
            elements[i].number = i;
            elements[i].SetStatueStatus(Random.Range(0,100) < 50);
        }
    }

    public void OnActivate(int elementsNumber)
    {
        int countIsActive = 0;
        foreach (Statue element in elements)
        {
            if (elementsNumber != element.number)
                element.SetStatueStatus(!element.isActive);
            countIsActive += (element.isActive) ? 1 : 0;
        }

        if (countIsActive == elements.Length)
            PuzzleResolved();
            
    }

    public void PuzzleResolved()
    {
        foreach (Statue element in elements)
            Destroy(element);
        Instantiate(prizeItem, transform.position, new Quaternion(0, 0, 0, 0));
    }

}
