Shader "Custom/VisionMask"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _VisionRange ("Vision Range", Float) = 10
        _FadeStart ("Fade Start Point", Range(0,1)) = 0.8
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _VisionRange;
            float _FadeStart;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Calculate distance from center
                float dist = distance(i.worldPos, _WorldSpaceCameraPos);
                float normalizedDist = dist / _VisionRange;
                
                // Calculate alpha based on distance
                float alpha = 1.0;
                if (normalizedDist > _FadeStart)
                {
                    alpha = 1.0 - smoothstep(_FadeStart, 1.0, normalizedDist);
                }
                
                return fixed4(0, 0, 0, alpha);
            }
            ENDCG
        }
    }
}