Shader "Bumped Specular with own map"
{
	Properties
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
		_Shininess ("Shininess", Range (0.03, 1)) = 0.078125
		_RimColor ("Rim Color", Color) = (1,1,1,1)
		_RimPower ("Rim Power", Range(0.5,20.0)) = 3.0
		_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
		_BumpMap ("Normalmap", 2D) = "bump" {}
		_SpecMap ("Specular map", 2D) = "black" {}
	}
	SubShader
	{ 
		Tags { "RenderType"="Opaque" }
		LOD 400
		
		CGPROGRAM
		#pragma exclude_renderers flash
		#pragma surface surf BlinnPhong
		sampler2D _MainTex;
		sampler2D _BumpMap;
		sampler2D _SpecMap;
		fixed4 _Color;
		half _Shininess;
		float4 _RimColor;
		float _RimPower;
		float _BumpAmount;
		
		struct Input
		{
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float2 uv_SpecMap;
			float3 viewDir;
		};
		
		void surf(Input IN, inout SurfaceOutput o)
		{
			fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
			fixed4 specTex = tex2D(_SpecMap, IN.uv_SpecMap);
			o.Albedo = tex.rgb * _Color.rgb;
			o.Gloss = specTex.r;
			o.Alpha = tex.a * _Color.a;
			o.Specular = _Shininess * specTex.g;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
			
			float4 Fresnel0_1_NoInput = float4(0,0,1,1);
			float4 Fresnel0 = (1.0 - dot( normalize( float4( IN.viewDir.x, IN.viewDir.y, IN.viewDir.z, 1.0 ).xyz), normalize( Fresnel0_1_NoInput.xyz ) )).xxxx;
			float4 Pow0 = pow(Fresnel0,_RimPower.xxxx);
			float4 rim = _RimColor * Pow0;
			o.Emission = (float3)rim;

			//half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
			//o.Emission = _RimColor.rgb * pow(rim, _RimPower);
		}
		ENDCG
	}
	
	FallBack "Specular"
}