using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nanapinned;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[VolumeComponentMenu("Custom Post-processing/RGB Shift")]
public class RGBShift : CustomVolumeComponent
{

    public FloatParameter speed = new FloatParameter(0f);
    public ClampedFloatParameter range = new ClampedFloatParameter(.1f, 0, 1f);
    public FloatParameter power = new FloatParameter(8f);

    Material material;

    const string shaderName = "Hidden/PostProcess/RGBShift";

    public override int OrderInPass => 11;

    public override CustomPostProcessInjectionPoint InjectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

    public override bool IsActive() => material != null && speed.value > 0f;

    public override void Setup()
    {
        if (material == null)
            material = CoreUtils.CreateEngineMaterial(shaderName);
    }

    public override void Render(CommandBuffer cmd, ref RenderingData renderingData, RenderTargetIdentifier source, RenderTargetIdentifier destination)
    {
        if (material == null)
            return;

        material.SetFloat("_Speed", speed.value);
        material.SetFloat("_Range", range.value);
        material.SetFloat("_Power", power.value);

        cmd.Blit(source, destination, material);
    }

    public override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        CoreUtils.Destroy(material);
    }

}
