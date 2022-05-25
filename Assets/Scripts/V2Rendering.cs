using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V2Rendering : MonoBehaviour
{
    public Camera renderCam;
    public bool toRenderTextureFlag;
    public Color color;
    public Renderer target;
    public Vector4[] xCols;
    public Vector4[] yCols;

    public Vector2[] testUVs;
    public bool generate;

    private Matrix4x4 uvToRectX;
    private Matrix4x4 uvToRectY;

    private void Awake()
    {
        uvToRectX = new Matrix4x4(xCols[0], xCols[1], xCols[2], xCols[3]);
        uvToRectY = new Matrix4x4(yCols[0], yCols[1], yCols[2], yCols[3]);

        if (generate)
        {
            List<Vector2> generated = new List<Vector2>();
            int count = 10;
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    generated.Add(new Vector2(i / (float)count, j / (float)count));
                }
            }
            testUVs = generated.ToArray();
        }
    }

    // Start is called before the first frame update
    protected void LateUpdate()
    {
        target.material.SetMatrix("_matrixP", GL.GetGPUProjectionMatrix(renderCam.projectionMatrix, toRenderTextureFlag));
        target.material.SetMatrix("_uvToRectX", uvToRectX);
        target.material.SetMatrix("_uvToRectY", uvToRectY);
    }

    public void Update()
    {
        foreach (var uv in testUVs)
        {
            Vector3 dir = new Vector3(polyval2d(uv.x, uv.y, uvToRectX), -polyval2d(uv.x, uv.y, uvToRectY), 1.0f);
            Debug.DrawRay(renderCam.transform.position, renderCam.transform.TransformDirection(dir), color);
        }
    }

    private float polyval2d(float X, float Y, Matrix4x4 C)
    {
        float X2 = X * X; float X3 = X2 * X;
        float Y2 = Y * Y; float Y3 = Y2 * Y;
        return (
            ((C.m00) + (C.m01 * Y) + (C.m02 * Y2) + (C.m03 * Y3)) +
            ((C.m10 * X) + (C.m11 * X * Y) + (C.m12 * X * Y2) + (C.m13 * X * Y3)) +
            ((C.m20 * X2) + (C.m21 * X2 * Y) + (C.m22 * X2 * Y2) + (C.m23 * X2 * Y3)) +
            ((C.m30 * X3) + (C.m31 * X3 * Y) + (C.m32 * X3 * Y2) + (C.m33 * X3 * Y3))
            );
    }
}
