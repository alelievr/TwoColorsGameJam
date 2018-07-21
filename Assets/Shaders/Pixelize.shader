Shader "Hidden/Jam/Pixelize"
{
    HLSLINCLUDE

        #include "PostProcessing/Shaders/StdLib.hlsl"

        TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
        float _Size;

        float4 Frag(VaryingsDefault i) : SV_Target
        {
            float2 ratioSize = float2(_ScreenParams.x / _ScreenParams.y, 1) * _Size;
            float2 uv1 = round(i.texcoord * ratioSize) / ratioSize;
            // float2 uv2 = uv1 + ratioSize;
            // float2 uv3 = uv1 + float2(ratioSize.x, 0);
            // float2 uv4 = uv1 + float2(0, ratioSize.y);
            float4 color1 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv1);
            // float4 color2 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv2);
            // float4 color3 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv2);
            // float4 color4 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv3);

            // float4 c = (color1 + color2 + color3 + color4) / 4.0;
            return color1;
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