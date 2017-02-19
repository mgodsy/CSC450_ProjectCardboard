Shader "Hidden/Internal-Flare"
{
    SubShader
    {
        Tags {"RenderType"="Overlay"} //Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        ZWrite Off
        ZTest Always //ZTest Less
        Cull Off
        Blend One One
        ColorMask RGB

        Pass
        {   
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"

            sampler2D _FlareTexture;
            sampler2D _CameraDepthTexture;
            
            struct appdata_t
            {
                float4 vertex : POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                float4 projPos : TEXCOORD1;
            };

            float4 _FlareTexture_ST;
            
            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.color = v.color;
                o.texcoord = TRANSFORM_TEX(v.texcoord, _FlareTexture);

                float4 pos = mul(UNITY_MATRIX_MVP, v.vertex);
                o.projPos = ComputeScreenPos(pos);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 finalColor = tex2D(_FlareTexture, i.texcoord) * i.color;
                float depth = Linear01Depth (tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)).r);

                if(depth >= 1)
                //if(depth >= 1 && i.projPos.x < 1)
                    return finalColor;
                else
                    return 0;
            }

            ENDCG 
        }
    }
}

