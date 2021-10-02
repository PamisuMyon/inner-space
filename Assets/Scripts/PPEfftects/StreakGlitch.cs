using UnityEngine;
using Nanapinned;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[VolumeComponentMenu("Custom Post-processing/Streak Glitch")]
public class StreakGlitch : CustomVolumeComponent
{

    public FloatParameter density = new FloatParameter(0);
    public FloatParameter power = new FloatParameter(10f);

    Material material;

    const string shaderName = "Hidden/PostProcess/StreakGlitch";

    public override int OrderInPass => 10;

    public override void Setup()
    {
        if (material == null)
            material = CoreUtils.CreateEngineMaterial(shaderName);
    }

    public override void Render(CommandBuffer cmd, ref RenderingData renderingData, RenderTargetIdentifier source, RenderTargetIdentifier destination)
    {
        if (material == null)
            return;

        material.SetFloat(Shader.PropertyToID("_Density"), density.value);

        cmd.Blit(source, destination, material);
    }

    public override bool IsActive() => material != null && density.value > 0f;

    public override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        CoreUtils.Destroy(material);
    }
}
