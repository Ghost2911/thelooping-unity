using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelControl : MonoBehaviour
{
    private GameObject panel;

    void Awake()
    {
        panel = this.gameObject;
        panel.SetActive(false);
    }

    public void ChangeActive()
    {
        panel.SetActive(!panel.activeSelf);
    }
}
