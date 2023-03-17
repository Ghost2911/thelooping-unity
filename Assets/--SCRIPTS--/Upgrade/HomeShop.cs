using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeShop : MonoBehaviour
{
    public PanelControl panel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            panel.ChangeActive();
    }
}
