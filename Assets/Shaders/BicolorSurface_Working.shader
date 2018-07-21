Shader "Unlit/BicolorSurface_Working"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			float whiteNoise(in float2 st)
			{
				return frac(sin(dot(st.xy, float2(12.9898,78.233))) * 43758.5453123);
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float4 color = tex2D(_MainTex, i.uv);
				float luminance = dot(color.rgb, float3(0.2126729, 0.7151522, 0.0721750));
	
				float r = whiteNoise(i.vertex.xy);
	
				if (r >= luminance)
					return float4(0, 0, 0, 1);
				else
					return float4(1, 1, 1, 1);
			}
			ENDCG
		}
	}
}
