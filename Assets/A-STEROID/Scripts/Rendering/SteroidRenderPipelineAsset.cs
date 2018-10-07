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
            SteroidRenderCamera(context, camera);

            context.Submit();
            cmd.Clear();
        }
	}

    void SteroidRenderCamera(ScriptableRenderContext context, Camera camera)
    {
        ScriptableCullingParameters scp;

        if (!CullResults.GetCullingParameters(camera, false, out scp))
            return;
        
        context.SetupCameraProperties(camera);

        var cullResults = CullResults.Cull(ref scp, context);
        
        cmd.ClearRenderTarget(false, true, camera.backgroundColor);
        context.ExecuteCommandBuffer(cmd);

        var sprites = new FilterRenderersSettings(true)
        {
            renderQueueRange = RenderQueueRange.transparent,
            layerMask = asset.visibleLayers.value
        };

        var drs = new DrawRendererSettings(camera, new ShaderPassName("Steroid"))
        {
            flags = DrawRendererFlags.EnableDynamicBatching,
            rendererConfiguration = RendererConfiguration.None,
        };

        drs.sorting.flags = SortFlags.CommonTransparent;

        context.DrawRenderers(cullResults.visibleRenderers, ref drs, sprites);
    }

}
