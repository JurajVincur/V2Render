using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CalibrationData
{
    public float[] left_uv_to_rect_x;
    public float[] left_uv_to_rect_y;
    public float[] right_uv_to_rect_x;
    public float[] right_uv_to_rect_y;
}

public class CalibrationLoader : MonoBehaviour
{
    public TextAsset jsonFile;
    public V2Rendering leftRendering;
    public V2Rendering rightRendering;

    private void Awake()
    {
        CalibrationData data = JsonUtility.FromJson<CalibrationData>(jsonFile.text);
        for (int i = 0; i < 4; i++)
        {
            leftRendering.xCols[i] = new Vector4(data.left_uv_to_rect_x[0+i], data.left_uv_to_rect_x[4+i], data.left_uv_to_rect_x[8+i], data.left_uv_to_rect_x[12+i]);
            leftRendering.yCols[i] = new Vector4(data.left_uv_to_rect_y[0+i], data.left_uv_to_rect_y[4+i], data.left_uv_to_rect_y[8+i], data.left_uv_to_rect_y[12+i]);

            rightRendering.xCols[i] = new Vector4(data.right_uv_to_rect_x[0+i], data.right_uv_to_rect_x[4+i], data.right_uv_to_rect_x[8+i], data.right_uv_to_rect_x[12+i]);
            rightRendering.yCols[i] = new Vector4(data.right_uv_to_rect_y[0+i], data.right_uv_to_rect_y[4+i], data.right_uv_to_rect_y[8+i], data.right_uv_to_rect_y[12+i]);
        }
    }
}
