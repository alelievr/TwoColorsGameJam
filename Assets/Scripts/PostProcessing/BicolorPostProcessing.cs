using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering.PostProcessing;
 
[Serializable]
[PostProcess(typeof(GrayscaleRenderer), PostProcessEvent.AfterStack, "Jam/Bicolor")]
public sealed class Bicolor : PostProcessEffectSettings
{
    [Range(0f, 1f), Tooltip("Spread")]
    public FloatParameter spread = new FloatParameter { value = 0.5f };
	public ColorParameter mainColor = new ColorParameter { value = Color.white };
	public ColorParameter backgroundColor = new ColorParameter { value = Color.black };
}
 
public sealed class GrayscaleRenderer : PostProcessEffectRenderer<Bicolor>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Jam/Bicolor"));
        sheet.properties.SetFloat("_Spread", settings.spread);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}
