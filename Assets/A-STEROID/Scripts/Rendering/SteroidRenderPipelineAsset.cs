using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

[ExecuteInEditMode]
public class SteroidRenderPipelineAsset : RenderPipelineAsset
{
    public LayerMask    visibleLayers;

#if UNITY_EDITOR
    public static string GetCurrentPath()
    {
        var path = "";
        var obj = Selection.activeObject;

        if (obj == null)
            return null;
        else
            path = AssetDatabase.GetAssetPath(obj.GetInstanceID());

        if (path.Length > 0)
        {
            if (Directory.Exists(path))
                return path;
            else
                return new FileInfo(path).Directory.FullName;
        }
        return null;
    }

    [UnityEditor.MenuItem("Assets/Create/A-Steroid/RenderPipeline")]
    static void CreateBasicAssetPipeline()
    {
        var instance = ScriptableObject.CreateInstance<SteroidRenderPipelineAsset>();
        UnityEditor.AssetDatabase.CreateAsset(instance, GetCurrentPath() + "/SteroidRenderPipeline.asset");
    }
#endif

    protected override IRenderPipeline InternalCreatePipeline()
    {
        return new SteroidRenderPipeline(this);
    }
}


public class SteroidRenderPipeline : RenderPipeline
{
	CommandBuffer	            cmd;
    SteroidRenderPipelineAsset  asset;
	
	public SteroidRenderPipeline(SteroidRenderPipelineAsset asset)
	{
        this.asset = asset;
		cmd = new CommandBuffer();
	}

	public override void Render(ScriptableRenderContext context, Camera[] cameras)
	{
        base.Render(context, cameras);

        foreach (var camera in cameras)
        {
            if (!SteroidRenderCamera(context, camera))
                continue ;

            cmd.Clear();
        }
	}

    bool SteroidRenderCamera(ScriptableRenderContext context, Camera camera)
    {
        ScriptableCullingParameters scp;

        cmd.ClearRenderTarget(false, true, camera.backgroundColor);

        if (!CullResults.GetCullingParameters(camera, out scp))
            return false;
        
        var cullResults = CullResults.Cull(ref scp, context);

        var sprites = new FilterRenderersSettings
        {
            renderQueueRange = new RenderQueueRange
            {
                min = (int)RenderQueue.GeometryLast,
                max = (int)RenderQueue.Transparent,
            },
            layerMask = asset.visibleLayers.value
        };

        var drs = new DrawRendererSettings(camera, new ShaderPassName("Transparent"))
        {
            flags = DrawRendererFlags.EnableDynamicBatching,
            rendererConfiguration = RendererConfiguration.None,
        };

        drs.sorting.flags = SortFlags.SortingLayer;

        context.DrawRenderers(cullResults.visibleRenderers, ref drs, sprites);
        
        context.ExecuteCommandBuffer(cmd);
        context.Submit();

        return true;
    }

}
