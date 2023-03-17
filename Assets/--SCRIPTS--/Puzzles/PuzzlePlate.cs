using UnityEngine;

public class PuzzlePlate : MonoBehaviour
{
    private PressurePlate[] plates;
    public GameObject traps;
    private void Awake() {
        plates = transform.GetComponentsInChildren<PressurePlate>();
        traps.SetActive(false);
    }

    public void DeactivateAllPlates()
    {
        foreach(PressurePlate p in plates)
        {  
            if (p.isActive)
                p.Deactivate();
        }
        traps.SetActive(true);
    }
}
