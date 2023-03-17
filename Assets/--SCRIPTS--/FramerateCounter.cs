
using TMPro;
using UnityEngine;

public class FramerateCounter : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    private float hudRefreshRate = 1f; 
    private float timer;

    void Start()
    {
        Application.targetFrameRate = 140;
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (Time.unscaledTime > timer)
        {
            int fps = (int)(1f / Time.unscaledDeltaTime);
            textMesh.text = fps.ToString();
            timer = Time.unscaledTime + hudRefreshRate;
        }
    }
}
