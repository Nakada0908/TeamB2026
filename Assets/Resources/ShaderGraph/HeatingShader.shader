Shader "Custom/HeatingShader"
{
    	Properties {
		_MainTex ("Render Input", 2D) = "white" {}
		_DistortionTex ("Normal Map", 2D) = "white" {}
		_Strength ("Strength", Range(0,1) ) = 0.1
	}
	SubShader {
		ZTest Always Cull Off ZWrite Off Fog { Mode Off }
		
		Pass {
			CGPROGRAM
				#pragma vertex vert_img
				#pragma fragment frag
				#include "UnityCG.cginc"

				UNITY_DECLARE_SCREENSPACE_TEXTURE(_MainTex)
				UNITY_DECLARE_SCREENSPACE_TEXTURE(_DistortionTex)
				float _Strength;

				float4 frag(v2f_img IN) : COLOR {
					float3 wNorm = UNITY_SAMPLE_SCREENSPACE_TEXTURE( _DistortionTex, IN.uv ).rgb * 2 - 1;
					float3 vNorm = mul((float3x3)unity_WorldToCamera, wNorm);
					
					// The 100 is just to adjust the range to something more reasonable.
					// This takes place in UV space, so a value of 1 will offset by the entire
					// width of the display.
					float scale = _Strength / 100;
					float2 uv_offset = refract( float3(0,0,1), vNorm, 1.0 ).xy * scale;

					return UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MainTex, IN.uv + uv_offset);
				}
			ENDCG
		}
	}
}