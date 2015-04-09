Shader "Custom/MultiColor" {
    Properties {
        _MainTex ("Main (RGB)", 2D) = "white" {}
        _RedTex ("Red (RGB)", 2D) = "red" {}
        _GreenTex ("Green (RGB)", 2D) = "green" {}
        _BlueTex ("Blue (RGB)", 2D) = "blue" {}
        _MaskTex ("Mask area (RGB)", 2D) = "black" {}
        _ColorR ("Red Color", Color) = (1,1,1,1)
        _ColorG ("Green Color", Color) = (1,1,1,1)
        _ColorB ("Blue Color", Color) = (1,1,1,1)
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex;
        sampler2D _RedTex;
        sampler2D _GreenTex;
        sampler2D _BlueTex;
        sampler2D _MaskTex;
        half4 _ColorR;
        half4 _ColorG;
        half4 _ColorB;
        half4 _MaskMult;

        struct Input {
            float2 uv_MainTex;
            float2 uv_RedTex;
            float2 uv_GreenTex;
            float2 uv_BlueTex;
        };

        void surf (Input IN, inout SurfaceOutput o) {
            half4 main = tex2D (_MainTex, IN.uv_MainTex);
            half4 red = tex2D (_RedTex, IN.uv_RedTex);
            half4 green = tex2D (_GreenTex, IN.uv_GreenTex);
            half4 blue = tex2D (_BlueTex, IN.uv_BlueTex);
            half4 mask = tex2D (_MaskTex, IN.uv_MainTex);

            half3 cr = red.rgb * _ColorR;
            half3 cg = green.rgb * _ColorG;
            half3 cb = blue.rgb * _ColorB;

            half r = mask.r;
            half g = mask.g;
            half b = mask.b;

            half minv = min(r + g + b, 1);

            half3 cf = lerp(lerp(cr, cg, g*(r+g)), cb, b*(r+g+b));
            half3 c = lerp(main.rgb, cf, minv);

            o.Albedo = c.rgb;
            o.Alpha = main.a;
        }
        ENDCG
    } 
    FallBack "Diffuse"
}
