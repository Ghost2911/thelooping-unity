using System;
using UnityEngine;

[ExecuteInEditMode]
public class ColorReplacement : MonoBehaviour
{
    #region Mask Variables

    [Header("Mask Variables")]

    // Color that you use to mask things with
    [Tooltip("The mask color. This is the color that you replace with something.")]
    public Color MaskColor = Color.white;

    [Tooltip("Tolerance of the mask. Lower number = more precise.")]
    [Range(0.0f, 1.0f)]
    public float Tolerance = 0.1f;

    #endregion

    #region Replacing Variables

    [Header("Replacing Variables")]

    // The color and/or texture that you replace the masked color with
    [Tooltip("The color that replaces the Mask Color")]
    public Color ReplaceColor = Color.red;

    [Tooltip("The texture that replaces the Mask Color")]
    public Texture MaskTexture;

    [Tooltip("Scale of the Mask Texture")]
    public float MaskTextureScale = 1;
    [Space(20)]

    #endregion

    #region Other Variables

    // The actual effect shader
    public Shader CRShader;

    // Material created based on the values above
    [NonSerialized]
    private Material CRMaterial;

    #endregion

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (CRMaterial == null)
        {
            if (!CRShader)
                Debug.LogError("Color Replacement shader missing from ColorReplacement.cs!");
            CRMaterial = new Material(CRShader);
            CRMaterial.hideFlags = HideFlags.HideAndDontSave;
        }

        CRMaterial.SetColor("_MaskColor", MaskColor);
        CRMaterial.SetFloat("_Diff", Tolerance);
        CRMaterial.SetTextureScale("_MaskTex", new Vector2(MaskTextureScale, MaskTextureScale));

        CRMaterial.SetColor("_ReplaceColor", ReplaceColor);
        CRMaterial.SetTexture("_MaskTex", MaskTexture);

        Graphics.Blit(source, destination, CRMaterial);
    }
}
