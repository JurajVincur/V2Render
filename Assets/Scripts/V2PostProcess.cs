using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V2PostProcess : MonoBehaviour
{
    public Renderer leftTarget;
    public Renderer rightTarget;
    public Vector2Int screenSize;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        RenderTexture tempFull = RenderTexture.GetTemporary(screenSize.x, screenSize.y);
        RenderTexture tempHalf = RenderTexture.GetTemporary(screenSize.x / 2, screenSize.y);
        Graphics.Blit(leftTarget.material.mainTexture, tempHalf, leftTarget.material);
        //Graphics.Blit(tempHalf, null, new Vector2(2f, 1f), new Vector2(0f, 0f));
        Graphics.CopyTexture(tempHalf, 0, 0, 0, 0, tempHalf.width, tempHalf.height, tempFull, 0, 0, 0, 0);
        Graphics.Blit(rightTarget.material.mainTexture, tempHalf, rightTarget.material);
        Graphics.CopyTexture(tempHalf, 0, 0, 0, 0, tempHalf.width, tempHalf.height, tempFull, 0, 0, tempHalf.width, 0);
        Graphics.Blit(tempFull, destination);
        RenderTexture.ReleaseTemporary(tempHalf);
        RenderTexture.ReleaseTemporary(tempFull);
    }
}
