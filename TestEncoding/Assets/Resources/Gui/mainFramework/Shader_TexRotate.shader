Shader "Custom/Shader_TexRotate" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_RotateCenter ("Rotate Center(Vector)", Vector) = (0,0,0,0)
		_RotateAngle("Rotate Angle(float)", Float) = 0.0
	}
	SubShader {
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
		Lighting Off
		Fog { Mode Off }
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		LOD 200
		
		Pass{
		
			CGPROGRAM
		
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
		
			sampler2D _MainTex;
			uniform float4 _RotateCenter;
			uniform float _RotateAngle;
			
			struct v2f{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};
			
			float4 _MainTex_ST;
			
			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}
		
			half4 frag(v2f i) : COLOR
			{
				float ca = cos(_RotateAngle);
				float sa = sin(_RotateAngle);
				float2 uv;
				uv.x = 0.5f - i.uv.x;
				uv.y = 0.5f - i.uv.y;
				float2 rotateUV;
				rotateUV.x = uv.x * ca - uv.y * sa + 0.5f;
				rotateUV.y = uv.x * sa + uv.y * ca + 0.5f;
				if(rotateUV.x < 0.0f || rotateUV.x > 1.0f || rotateUV.y < 0.0f || rotateUV.y > 1.0f)
					return float4(0.0f,0.0f,0.0f,0.0f);
				else
					return tex2D(_MainTex,rotateUV);
			}
			
			ENDCG
		}
	}
}
