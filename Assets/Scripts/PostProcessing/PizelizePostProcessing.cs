using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering.PostProcessing;
 
[Serializable]
[PostProcess(typeof(PizelizeRenderer), PostProcessEvent.AfterStack, "Jam/Pixelize")]
public sealed class Pixelize : PostProcessEffectSettings
{
    [Range(1, 200), Tooltip("Size")]
    public FloatParameter size = new FloatParameter { value = 100 };
}
 
public sealed class PizelizeRenderer : PostProcessEffectRenderer<Pixelize>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Jam/Pixelize"));
        sheet.properties.SetFloat("_Size", settings.size);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}
