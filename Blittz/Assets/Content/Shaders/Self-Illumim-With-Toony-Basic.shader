Shader "Custom/Self-Illumim-With-Toony-Basic" {
	Properties {
		_Color ("Main Color", Color) = (0.5,0.5,0.5,1)
		_OutlineColor ("Outline Color", Color) = (0,0,0,1)
		_Outline ("Outline width", Range (0.0, 0.1)) = .005
		_MainTex ("Base (RGB)", 2D) = "white" {}
	
		//_Color ("Main Color", Color) = (1,1,1,1)
		_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
		_Shininess ("Shininess", Range (0.01, 1)) = 0.078125
		//_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
		_Illum ("Illumin (A)", 2D) = "white" {}
		_BumpMap ("Normalmap", 2D) = "bump" {}
		_EmissionLM ("Emission (Lightmapper)", Float) = 0	
	}

	SubShader {
		Tags { "RenderType"="Opaque" }
		UsePass "Toon/Basic Outline/OUTLINE"
		UsePass "Self-Illumin/Bumped Specular/FORWARD"
	} 
	
	Fallback "Self-Illumin/Bumped Specular"
}
