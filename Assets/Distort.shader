Shader "Custom/Distort" {
	Properties{
		_MainTex("Texture", 2D) = "white" {}
	}

		SubShader{
			Pass {
				CGPROGRAM
				#pragma vertex vert_img
				#pragma fragment frag
				#include "UnityCG.cginc" // required for v2f_img

				// Properties
				sampler2D _MainTex;
				float4x4 _uvToRectX;
				float4x4 _uvToRectY;
				float4x4 _matrixP;

				float polyval2d(float X, float Y, float4x4 C) {
					float X2 = X * X; float X3 = X2 * X;
					float Y2 = Y * Y; float Y3 = Y2 * Y;
					return (
						((C[0][0]) + (C[0][1] * Y) + (C[0][2] * Y2) + (C[0][3] * Y3)) +
						((C[1][0] * X) + (C[1][1] * X * Y) + (C[1][2] * X * Y2) + (C[1][3] * X * Y3)) +
						((C[2][0] * X2) + (C[2][1] * X2 * Y) + (C[2][2] * X2 * Y2) + (C[2][3] * X2 * Y3)) +
						((C[3][0] * X3) + (C[3][1] * X3 * Y) + (C[3][2] * X3 * Y2) + (C[3][3] * X3 * Y3))
						);
				}

				float4 frag(v2f_img input) : COLOR {
					// sample texture for color
					//polyval uv to get eye coords / view space
					//mult by UNITY_MATRIX_P to get clip pos
					//ComputeScreenPos to get uv
					//float3 viewSpace = float3(polyval2d(1.0 - input.uv[0], input.uv[1], _uvToRectX), polyval2d(1.0 - input.uv[0], input.uv[1], _uvToRectY), 1.0); //TODO check 1.0 -
					float3 viewSpace = float3(polyval2d(input.uv[0], input.uv[1], _uvToRectX), -polyval2d(input.uv[0], input.uv[1], _uvToRectY), -1.0); //flip y to convert it from opencv to unity coordinate space, flip z bcs camera's forward is the negative Z axis, see: https://docs.unity3d.com/ScriptReference/Camera-worldToCameraMatrix.html
					float4 clipSpace = mul(_matrixP, float4(viewSpace, 1.0));
					float4 screenPos = ComputeScreenPos(clipSpace); //camera independent
					float4 base = tex2D(_MainTex, screenPos.xy / screenPos.w);
					return base;
				}

				ENDCG
			}
	}
}