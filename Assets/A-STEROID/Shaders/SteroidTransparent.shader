Shader "Steroid/Transparent"
{
	Properties
	{
		_Color ("Color", Color) = (1, 1, 1, 1)
		_MainTex ("Texture", 2D) = "white" {}
		Colorize ("Colorize", Range(0.0, 1.0)) = 1
	}
	SubShader
	{
		Tags { "Queue"="Transparent" }
		
		ZWrite off
		Blend SrcAlpha OneMinusSrcAlpha
		LOD 100

		Pass
		{
			Tags { "LightMode" = "Steroid" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 color : COLOR;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Color;
			float Colorize;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = v.color;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float4 texcolor = tex2D(_MainTex, i.uv) * i.color;
                float4 vertexcolor = i.color;
                texcolor.rgb = texcolor.rgb * (1 - Colorize) + vertexcolor.rgb * Colorize;
                return texcolor;
			}
			ENDCG
		}
	}
}