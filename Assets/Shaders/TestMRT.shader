Shader "Custom/TestMRT" {
	Properties {
		_MainTex ("", 2D) = "" {}
	}

		CGINCLUDE
#include "UnityCG.cginc"
	struct v2f
	{
		float4 pos : POSITION;
		float2 uv : TEXCOORD0;
	};

	struct PixelOutput
	{
		float4 col0 : COLOR0;
		float4 col1 : COLOR1;
	};

	sampler2D _MainTex;
	sampler2D _Tex0;
	sampler2D _Tex1;

	v2f vert(appdata_img v)
	{
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv = v.texcoord.xy;
		return o;
	}

	//可以返回多个颜色,对应的  
	//public static void SetRenderTarget(RenderBuffer[] colorBuffers, RenderBuffer depthBuffer); 可用多个buff接受  
	//Graphics.SetRenderTarget(mrtRB, mrtTex[0].depthBuffer); 
	PixelOutput fragTestMRT(v2f pixelData)
	{
		PixelOutput o;
		o.col0 = float4(1.0f, 0.0f, 0.0f, 1.0f);
		o.col1 = float4(0.0f, 1.0f, 0.0f, 1.0f);
		return o;
	}

	float4 fragCanYingAdd(v2f pixelData) : COLOR0
	{
		float4 col1 = tex2D(_Tex1,pixelData.uv);
		float4 colM = tex2D(_MainTex, pixelData.uv);
		float4 col;
		col.rgb = colM.rgb + col1.rgb * (1 - colM.rgb) * 0.9;
		return col;
	}

		float4 fragCanYingBind(v2f pixelData) : COLOR0
	{
		float2 uuv = float2(pixelData.uv.x,1 - pixelData.uv.y);
		float4 col0 = tex2D(_Tex0, uuv);
		float4 col1 = tex2D(_Tex1, pixelData.uv);

		float4 col;
		col = col0 + col1 * (1 - col0.a);
		return col;
	}
		ENDCG

	SubShader {
		Pass
		{
			ZTest Always Cull Off ZWrite Off
			Fog{Mode Off}

			CGPROGRAM
			#pragma glsl
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma vertex vert
			#pragma fragment fragTestMRT
			#pragma target 3.0
			ENDCG
		}

		Pass
		{
			ZTest Always Cull Off ZWrite Off
			Fog {Mode Off}
			//Blend one zero   
			CGPROGRAM
			#pragma glsl
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma vertex vert
			#pragma fragment fragCanYingAdd
			#pragma target 3.0
			ENDCG
		}

		Pass
		{
			ZTest Always Cull Off ZWrite Off
			Fog {Mode Off}
			//Blend one zero   
			CGPROGRAM
			#pragma glsl
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma vertex vert
			#pragma fragment fragCanYingBind
			#pragma target 3.0
			ENDCG
		}
	}
	FallBack off
}
