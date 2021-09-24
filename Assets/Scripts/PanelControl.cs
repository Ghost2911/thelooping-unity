using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelControl : MonoBehaviour
{
    private GameObject panel;

    void Start()
    {
        panel = this.gameObject;
        panel.SetActive(false);
    }

    public void PanelChangeActive()
    {
        panel.SetActive(!panel.activeSelf);
    }
}
