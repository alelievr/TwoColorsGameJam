Shader "Hidden/Jam/Bicolor"
{
    HLSLINCLUDE

        #include "PostProcessing/Shaders/StdLib.hlsl"

        TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
        float _Blend;

		float whiteNoise(in float2 st)
		{
			return frac(sin(dot(st.xy, float2(12.9898,78.233))) * 43758.5453123);
		}

        float4 Frag(VaryingsDefault i) : SV_Target
        {
            float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
            float luminance = dot(color.rgb, float3(0.2126729, 0.7151522, 0.0721750));

			float r = whiteNoise(i.texcoord);

			if (r >= luminance)
				return float4(0, 0, 0, 1);
			else
				return float4(1, 1, 1, 1);
        }

    ENDHLSL

    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM

                #pragma vertex VertDefault
                #pragma fragment Frag

            ENDHLSL
        }
    }
}