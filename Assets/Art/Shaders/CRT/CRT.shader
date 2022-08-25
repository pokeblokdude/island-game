Shader "Unlit/CRT" {

    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Curvature ("Curvature", Float) = 1
        _VignetteWidth("Vignette Width", Float) = 1
    }

    SubShader {
        Tags { "RenderType"="Opaque" }

        Pass {

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct MeshData {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct interp {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float _Curvature;
            float _VignetteWidth;

            interp vert (MeshData v) {
                interp o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (interp i) : SV_Target {
                // CREDIT TO ACEROLA ON YOUTUBE

                float2 uv = i.uv;

                //shperize UVs
                uv = uv * 2 - 1;
                float2 offset = uv.yx / _Curvature;
                uv += uv * offset * offset;
                uv = uv * 0.5f + 0.5f;

                float4 col = tex2D(_MainTex, uv);
                if (uv.x <= 0.0f || 1.0f <= uv.x || uv.y <= 0.0f || 1.0f <= uv.y)
                    col = 0;

                uv = uv * 2.0f - 1.0f;
                float2 vignette = _VignetteWidth / _ScreenParams.xy;
                vignette = smoothstep(0.0f, vignette, 1.0f - abs(uv));
                vignette = saturate(vignette);

                col.g *= (sin(i.uv.y * _ScreenParams.y * 2.0f) + 1.0f) * 0.15f + 1.0f;
                col.rb *= (cos(i.uv.y * _ScreenParams.y * 2.0f) + 1.0f) * 0.135f + 1.0f; 

                return saturate(col) * vignette.x * vignette.y;
            }

            ENDCG
        }
    }
}
