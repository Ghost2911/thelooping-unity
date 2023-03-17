using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ChromaticVision : Status
{
    float speedUp = 1.4f;
    public override void OnActivate()
    {
        target.speedMultiplier *= speedUp;
        foreach (PostProcessEffectSettings effect in GlobalSettings.instance.postProcessVolume.sharedProfile.settings)
            if (effect.GetType() == typeof(ChromaticAberration))
                effect.enabled.Override(true);
    }
    void OnDisable()
    {
        target.speedMultiplier /= speedUp;
        foreach (PostProcessEffectSettings effect in GlobalSettings.instance.postProcessVolume.sharedProfile.settings)
            if (effect.GetType() == typeof(ChromaticAberration))
                effect.enabled.Override(false);
    }
    public override void Tick() { }
}
