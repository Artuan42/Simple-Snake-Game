Shader "Unlit/VHS"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Curvature("Curvature", float) = 0.0
        _VignetteWidth("Width", float) = 0.0
        _CRTSpeed("Speed", float) = 0.0
        _CRTSize("Size", float) = 0.0
        _CRTOpacity("Opacity", float) = 0.0
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

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 ScreenPosition : TEXCOORD1;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 ScreenPosition : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _Curvature;
            float _VignetteWidth;
            float _CRTSize;
            float _CRTSpeed;
            float _CRTOpacity;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.ScreenPosition = ComputeScreenPos(o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                uv = uv * 2 - 1;

                float offset = uv/_Curvature;

                uv = uv + uv * offset * offset;

                uv = uv * 0.5 + 0.5;

                float2 uvVignette = 1 - abs(uv* 2 - 1);

                float2 VNette = float2(_VignetteWidth/_ScreenParams.x, _VignetteWidth/_ScreenParams.y);

                float finalVignetteX = smoothstep(0,VNette.x, uvVignette.x);
                float finalVignetteY = smoothstep(0,VNette.y, uvVignette.y);

                float finalVignette = finalVignetteX * finalVignetteY;

                float linesVertical = sin((uv.y + _Time * _CRTSpeed) * _ScreenParams.y * _CRTSize) + _CRTOpacity;
                #if uv < 0 || uv > 1
                    fixed4 col = float4(0.0,0.0,0.0,0.0);
                #else
                    fixed4 col = tex2D(_MainTex, uv);
                #endif

                col *= finalVignette * linesVertical;

                return col;
            }
            ENDCG
        }
    }
}
