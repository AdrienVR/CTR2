Shader "Custom/VertexBlendMaskedCleanEdges" {
	Properties
	{
		_Over ("Over", 2D) = "white" {}
		_Under ("Under", 2D) = "white" {}
		_Mask ("Mask", 2D) = "white" {}
		_Bias ("Edge Bias", Range(0.5, 30.0)) = 4.0
		_Edge ("Edge Sharpness", Float) = 10.0
		_Fall ("Blend Falloff", Range(-1,1)) = 1.0
		_Specular ("Specular", Range(0, 1)) = 1.0
	}
		
	SubShader
	{
		Tags
		{
			"Queue" = "Geometry-100"
			"RenderType" = "Opaque"
		}
		CGPROGRAM
		#pragma surface surf Lambert
		struct Input
		{
			float2 uv_Under : TEXCOORD0;
			float2 uv_Over : TEXCOORD1;
			float2 uv_Mask : TEXCOORD2;
			float4 color : COLOR;
		};
		
		sampler2D _Over,_Under,_Mask;
		float _Edge;
		float _Bias;
		float _Fall;
		float _Specular;
		
		void surf (Input IN, inout SurfaceOutput o)
		{
			fixed3 mask = tex2D (_Mask, IN.uv_Mask);
			fixed3 col;
			mask = mask.r;
			mask *= IN.color.r;
			mask *= _Bias;
			mask = pow(mask, _Edge);
			mask = clamp((mask.r - IN.color.r) / _Fall, 0.0, _Specular);
			col = tex2D (_Under, IN.uv_Under).rgb * 1 - mask.r;
			//col = max(tex2D (_Over, IN.uv_Over).rgb * mask.r, col);
			o.Albedo = col;
			o.Alpha = 0.0;
		}
		ENDCG
	}
}