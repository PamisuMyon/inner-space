using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nanapinned;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[VolumeComponentMenu("Custom Post-processing/Sobel Filter")]
public class SobelFilter : CustomVolumeComponent
{

    public ClampedFloatParameter lineThickness = new ClampedFloatParameter(0f, .0005f, .0025f);
    public BoolParameter outLineOnly = new BoolParameter(false);
    public BoolParameter posterize = new BoolParameter(false);
    public IntParameter count = new IntParameter(6);

    Material material;

    const string shaderName = "Hidden/PostProcess/SobleFilter";

    public override CustomPostProcessInjectionPoint InjectionPoint => CustomPostProcessInjectionPoint.AfterOpaqueAndSky;

    public override void Setup()
    {
        if (material == null)
            material = CoreUtils.CreateEngineMaterial(shaderName);
    }

    public override bool IsActive() => material != null && lineThickness.value > 0f;

    public override void Render(CommandBuffer cmd, ref RenderingData renderingData, RenderTargetIdentifier source, RenderTargetIdentifier destination)
    {
        if (material == null)
            return;

        material.SetFloat("_Delta", lineThickness.value);
        material.SetInt("_PosterizationCount", count.value);
        if (outLineOnly.value)
            material.EnableKeyword("RAW_OUTLINE");
        else
            material.DisableKeyword("RAW_OUTLINE");
        if (posterize.value)
            material.EnableKeyword("POSTERIZE");
        else
            material.DisableKeyword("POSTERIZE");

        cmd.Blit(source, destination, material);
    }

    public override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        CoreUtils.Destroy(material);
    }

}
