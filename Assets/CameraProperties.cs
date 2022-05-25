using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraProperties : MonoBehaviour
{
    public Quaternion eyeRotation;
    public Vector3 eyePosition;
    public bool overrideCameraProjection;
    public Vector4 projectionParameters;

    // Start is called before the first frame update
    private void Start()
    {
        transform.position = eyePosition;
        transform.rotation = eyeRotation;

        if (overrideCameraProjection == true)
        {
            var cam = GetComponent<Camera>();
            cam.projectionMatrix = ComposeProjection(projectionParameters);
            Debug.Log($"{transform.name}: {cam.projectionMatrix.ToString("R")}");
        }
    }

    private Matrix4x4 ComposeProjection(Vector4 tangentHalfAngles, float zNear = 0.07f, float zFar = 1000f)
    {
        float fLeft = tangentHalfAngles.x;
        float fRight = tangentHalfAngles.y;
        float fTop = tangentHalfAngles.w;
        float fBottom = tangentHalfAngles.z;

        float idx = 1.0f / (fRight - fLeft);
        float idy = 1.0f / (fBottom - fTop);
        //float idz = 1.0f / (zFar - zNear);
        float sx = fRight + fLeft;
        float sy = fBottom + fTop;

        float c = -(zFar + zNear) / (zFar - zNear);
        float d = -(2.0F * zFar * zNear) / (zFar - zNear);

        Matrix4x4 m = new Matrix4x4();
        m[0, 0] = 2 * idx; m[0, 1] = 0; m[0, 2] = sx * idx; m[0, 3] = 0;
        m[1, 0] = 0; m[1, 1] = 2 * idy; m[1, 2] = sy * idy; m[1, 3] = 0;
        m[2, 0] = 0; m[2, 1] = 0; m[2, 2] = c; m[2, 3] = d;
        m[3, 0] = 0; m[3, 1] = 0; m[3, 2] = -1.0f; m[3, 3] = 0;
        //m[2,2] = -zFar * idz; m[2,3] = -zFar * zNear * idz;
        return m;
    }
}
