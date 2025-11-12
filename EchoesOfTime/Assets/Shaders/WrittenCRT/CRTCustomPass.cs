using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class CRTCustomPass : CustomPass
{
    public Material material;

    protected override void Execute(CustomPassContext ctx)
    {
        if (material == null)
            return;

        // Asigna la textura de cámara actual al shader (_MainTex)
        material.SetTexture("_MainTex", ctx.cameraColorBuffer);

        // Dibuja en pantalla completa usando el render target actual
        CoreUtils.SetRenderTarget(ctx.cmd, ctx.cameraColorBuffer, ClearFlag.None);
        CoreUtils.DrawFullScreen(ctx.cmd, material, shaderPassId: 0);
    }
}
