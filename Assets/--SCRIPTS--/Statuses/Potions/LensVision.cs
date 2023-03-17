
using UnityEngine.Rendering.PostProcessing;

public class LensVision : Status
{
    public override void OnActivate()
    {
        foreach (PostProcessEffectSettings effect in GlobalSettings.instance.postProcessVolume.sharedProfile.settings)
            if (effect.GetType() == typeof(LensDistortion))
                effect.enabled.Override(true);
    }
    void OnDisable()
    {
        foreach (PostProcessEffectSettings effect in GlobalSettings.instance.postProcessVolume.sharedProfile.settings)
            if (effect.GetType() == typeof(LensDistortion))
                effect.enabled.Override(false);
    }
    public override void Tick(){ }
}
