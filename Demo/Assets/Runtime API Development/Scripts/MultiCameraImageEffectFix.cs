using UnityEngine;
using UnityEngine.Rendering;

// UNITY BUG: When using the deferred rendering path it seems that the result after image effects does not get copied back into the
// camera's render target.  So, when daisy chaining multiple camera passes with image effects, the final processed image from one
// camera does not get used as input to the next camera.  In other words, image effects from all but the last camera are lost.

// FIX: Add this to each camera in the chain, it copies the final processed image back into the camera's render target.
// Note that this script must be ordered after all other image effects!

[RequireComponent(typeof(Camera))]
public class MultiCameraImageEffectFix : MonoBehaviour
{
    RenderTexture resultAfterImageEffects = null;

    void Awake()
    {
        // Add a command buffer to blit the result after image effects back into the camera's render target.
        CommandBuffer commandBuffer = new CommandBuffer();
        commandBuffer.name = "MultiCameraImageEffectFix";

        commandBuffer.Blit( resultAfterImageEffects as Texture, BuiltinRenderTextureType.CameraTarget );

        GetComponent<Camera>().AddCommandBuffer( CameraEvent.AfterImageEffects, commandBuffer );
    }

    void OnRenderImage( RenderTexture src, RenderTexture dest )
    {
        resultAfterImageEffects = src;
    }
}

