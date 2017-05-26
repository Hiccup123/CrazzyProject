Shader "Custom/CanYing" {
	Properties {
		_MainTex ("", 2D) = "" {}
		_Color("Main Color",Color) = (1,1,1,1)
	}
	
	CGINCLUDE
	#include "UnityCG.cginc"
	struct v2f
	{
		float4 pos:POSITION;
		float2 uv:TEXCOORD0;
	};
	uniform float4 _Color;
	sampler2D _MainTex;

	v2f vert(appdata_img v)
	{
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv = v.texcoord.xy;
		return o;
	}

	float4 fragCanYingCreate(v2f pixelData) : COLOR0
	{
		float4 col1 = tex2D(_MainTex,pixelData.uv);//上一帧的累加图
		float4 col = _Color + col1;
		col.a = _Color.a;//1.0f;//0.1f;
		return col;
	}

	ENDCG

	SubShader {
		Pass
		{
			ZTest Always Cull Off ZWrite Off
			Fog{ Mode Off }
			//Blend one zero
			CGPROGRAM
			#pragma glsl
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma vertex vert
			#pragma fragment fragCanYingCreate
			#pragma target 3.0
			ENDCG
		}
	}
	FallBack off
}
