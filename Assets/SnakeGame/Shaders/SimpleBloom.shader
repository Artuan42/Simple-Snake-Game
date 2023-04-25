Shader "Unlit/SimpleBloom"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlurSize("BlurSize", float) = 0.0
        _Intensity("Intensity", float) = 0.35
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

            float _BlurSize;
            float _Intensity;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.ScreenPosition = ComputeScreenPos(o.vertex);
                return o;
            }

            fixed4 frag(v2f input) : SV_Target
            {
                //Bloom effect using box blur made by seven in https://www.shadertoy.com/view/lsXGWn
               fixed4 sum = float4(0.0, 0.0, 0.0, 0.0);

               float4 texcoord = float4(input.ScreenPosition.xy, 0.0, 1.0);

               sum += tex2D(_MainTex, float2(texcoord.x - 4.0*_BlurSize, texcoord.y)) * 0.05;
               sum += tex2D(_MainTex, float2(texcoord.x - 3.0*_BlurSize, texcoord.y)) * 0.09;
               sum += tex2D(_MainTex, float2(texcoord.x - 2.0*_BlurSize, texcoord.y)) * 0.12;
               sum += tex2D(_MainTex, float2(texcoord.x - _BlurSize, texcoord.y)) * 0.15;
               sum += tex2D(_MainTex, float2(texcoord.x, texcoord.y)) * 0.16;
               sum += tex2D(_MainTex, float2(texcoord.x + _BlurSize, texcoord.y)) * 0.15;
               sum += tex2D(_MainTex, float2(texcoord.x + 2.0*_BlurSize, texcoord.y)) * 0.12;
               sum += tex2D(_MainTex, float2(texcoord.x + 3.0*_BlurSize, texcoord.y)) * 0.09;
               sum += tex2D(_MainTex, float2(texcoord.x + 4.0*_BlurSize, texcoord.y)) * 0.05;
            	

               sum += tex2D(_MainTex, float2(texcoord.x, texcoord.y - 4.0*_BlurSize)) * 0.05;
               sum += tex2D(_MainTex, float2(texcoord.x, texcoord.y - 3.0*_BlurSize)) * 0.09;
               sum += tex2D(_MainTex, float2(texcoord.x, texcoord.y - 2.0*_BlurSize)) * 0.12;
               sum += tex2D(_MainTex, float2(texcoord.x, texcoord.y - _BlurSize)) * 0.15;
               sum += tex2D(_MainTex, float2(texcoord.x, texcoord.y)) * 0.16;
               sum += tex2D(_MainTex, float2(texcoord.x, texcoord.y + _BlurSize)) * 0.15;
               sum += tex2D(_MainTex, float2(texcoord.x, texcoord.y + 2.0*_BlurSize)) * 0.12;
               sum += tex2D(_MainTex, float2(texcoord.x, texcoord.y + 3.0*_BlurSize)) * 0.09;
               sum += tex2D(_MainTex, float2(texcoord.x, texcoord.y + 4.0*_BlurSize)) * 0.05;
                
                fixed4 finalCol = sum*_Intensity + tex2D(_MainTex, texcoord);

               return finalCol;
            }

            ENDCG
        }
    }
}
