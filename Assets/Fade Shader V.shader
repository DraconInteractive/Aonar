﻿Shader "Custom/Fade Shader V"
{
	Properties{
		_MainTex ("Texture 2D", 2D) = "white"{}
		_NormalTex ("Normal Map", 2D) = "bump"{}
		_EmissionTex ("Emission Texture", 2D) = "white"{}
		_Color("Color", Color) = (1,1,1,1)
		_InvisColor ("Invisible Color", Color) = (1,1,1,0)
		[HDR] _Emission ("Base Emission", Color) = (1,1,1)
		[HDR] _InvisEmission ("Invis Emission", Color) = (1,1,1)
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0

		//_DissolvePercentage("Dissolve Percentage", Range(0,1)) = 0.0
		_DissolveDistance("Dissolve Distance", Float) = 1
		_ClipDistance ("Clip Distance", Float) = 1
		_DissolvePower ("Dissolve Power", Float) = 1
		_DissolveScale ("Dissolve (Perlin) Scale", Float) = 1
		_ShowTexture("ShowTexture", Range(0,1)) = 0.0
		_EmissionStrength ("Emission Strength - Border", Float) = 0
		_EmissionStrengthDetail ("Emission Strength - Detail", Float) = 0
		_EmissionStrengthInvis ("Emission Strength - Invis", Float) = 0
		_EmissionColor ("Emission Color", Color) = (1,1,1,1)
		_EmissionThreshold("Emission Threshold", Float) = 0.3
		_EmissionUpper ("Emission Upper Threshold", Range(0,1)) = 0.2
}
	SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

				// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		float3 mod289(float3 x)
		{
		    return x - floor(x / 289.0) * 289.0;
		}

		float4 mod289(float4 x)
		{
		    return x - floor(x / 289.0) * 289.0;
		}

		float4 permute(float4 x)
		{
		    return mod289((x * 34.0 + 1.0) * x);
		}

		float4 taylorInvSqrt(float4 r)
		{
		    return 1.79284291400159 - r * 0.85373472095314;
		}

		float snoise(float3 v)
		{
		    const float2 C = float2(1.0 / 6.0, 1.0 / 3.0);

		    // First corner
		    float3 i  = floor(v + dot(v, C.yyy));
		    float3 x0 = v   - i + dot(i, C.xxx);

		    // Other corners
		    float3 g = step(x0.yzx, x0.xyz);
		    float3 l = 1.0 - g;
		    float3 i1 = min(g.xyz, l.zxy);
		    float3 i2 = max(g.xyz, l.zxy);

		    // x1 = x0 - i1  + 1.0 * C.xxx;
		    // x2 = x0 - i2  + 2.0 * C.xxx;
		    // x3 = x0 - 1.0 + 3.0 * C.xxx;
		    float3 x1 = x0 - i1 + C.xxx;
		    float3 x2 = x0 - i2 + C.yyy;
		    float3 x3 = x0 - 0.5;

		    // Permutations
		    i = mod289(i); // Avoid truncation effects in permutation
		    float4 p =
		      permute(permute(permute(i.z + float4(0.0, i1.z, i2.z, 1.0))
		                            + i.y + float4(0.0, i1.y, i2.y, 1.0))
		                            + i.x + float4(0.0, i1.x, i2.x, 1.0));

		    // Gradients: 7x7 points over a square, mapped onto an octahedron.
		    // The ring size 17*17 = 289 is close to a multiple of 49 (49*6 = 294)
		    float4 j = p - 49.0 * floor(p / 49.0);  // mod(p,7*7)

		    float4 x_ = floor(j / 7.0);
		    float4 y_ = floor(j - 7.0 * x_);  // mod(j,N)

		    float4 x = (x_ * 2.0 + 0.5) / 7.0 - 1.0;
		    float4 y = (y_ * 2.0 + 0.5) / 7.0 - 1.0;

		    float4 h = 1.0 - abs(x) - abs(y);

		    float4 b0 = float4(x.xy, y.xy);
		    float4 b1 = float4(x.zw, y.zw);

		    //float4 s0 = float4(lessThan(b0, 0.0)) * 2.0 - 1.0;
		    //float4 s1 = float4(lessThan(b1, 0.0)) * 2.0 - 1.0;
		    float4 s0 = floor(b0) * 2.0 + 1.0;
		    float4 s1 = floor(b1) * 2.0 + 1.0;
		    float4 sh = -step(h, 0.0);

		    float4 a0 = b0.xzyw + s0.xzyw * sh.xxyy;
		    float4 a1 = b1.xzyw + s1.xzyw * sh.zzww;

		    float3 g0 = float3(a0.xy, h.x);
		    float3 g1 = float3(a0.zw, h.y);
		    float3 g2 = float3(a1.xy, h.z);
		    float3 g3 = float3(a1.zw, h.w);

		    // Normalise gradients
		    float4 norm = taylorInvSqrt(float4(dot(g0, g0), dot(g1, g1), dot(g2, g2), dot(g3, g3)));
		    g0 *= norm.x;
		    g1 *= norm.y;
		    g2 *= norm.z;
		    g3 *= norm.w;

		    // Mix final noise value
		    float4 m = max(0.6 - float4(dot(x0, x0), dot(x1, x1), dot(x2, x2), dot(x3, x3)), 0.0);
		    m = m * m;
		    m = m * m;

		    float4 px = float4(dot(x0, g0), dot(x1, g1), dot(x2, g2), dot(x3, g3));
		    return 42.0 * dot(m, px);
		}

		float4 snoise_grad(float3 v)
		{
		    const float2 C = float2(1.0 / 6.0, 1.0 / 3.0);

		    // First corner
		    float3 i  = floor(v + dot(v, C.yyy));
		    float3 x0 = v   - i + dot(i, C.xxx);

		    // Other corners
		    float3 g = step(x0.yzx, x0.xyz);
		    float3 l = 1.0 - g;
		    float3 i1 = min(g.xyz, l.zxy);
		    float3 i2 = max(g.xyz, l.zxy);

		    // x1 = x0 - i1  + 1.0 * C.xxx;
		    // x2 = x0 - i2  + 2.0 * C.xxx;
		    // x3 = x0 - 1.0 + 3.0 * C.xxx;
		    float3 x1 = x0 - i1 + C.xxx;
		    float3 x2 = x0 - i2 + C.yyy;
		    float3 x3 = x0 - 0.5;

		    // Permutations
		    i = mod289(i); // Avoid truncation effects in permutation
		    float4 p =
		      permute(permute(permute(i.z + float4(0.0, i1.z, i2.z, 1.0))
		                            + i.y + float4(0.0, i1.y, i2.y, 1.0))
		                            + i.x + float4(0.0, i1.x, i2.x, 1.0));

		    // Gradients: 7x7 points over a square, mapped onto an octahedron.
		    // The ring size 17*17 = 289 is close to a multiple of 49 (49*6 = 294)
		    float4 j = p - 49.0 * floor(p / 49.0);  // mod(p,7*7)

		    float4 x_ = floor(j / 7.0);
		    float4 y_ = floor(j - 7.0 * x_);  // mod(j,N)

		    float4 x = (x_ * 2.0 + 0.5) / 7.0 - 1.0;
		    float4 y = (y_ * 2.0 + 0.5) / 7.0 - 1.0;

		    float4 h = 1.0 - abs(x) - abs(y);

		    float4 b0 = float4(x.xy, y.xy);
		    float4 b1 = float4(x.zw, y.zw);

		    //float4 s0 = float4(lessThan(b0, 0.0)) * 2.0 - 1.0;
		    //float4 s1 = float4(lessThan(b1, 0.0)) * 2.0 - 1.0;
		    float4 s0 = floor(b0) * 2.0 + 1.0;
		    float4 s1 = floor(b1) * 2.0 + 1.0;
		    float4 sh = -step(h, 0.0);

		    float4 a0 = b0.xzyw + s0.xzyw * sh.xxyy;
		    float4 a1 = b1.xzyw + s1.xzyw * sh.zzww;

		    float3 g0 = float3(a0.xy, h.x);
		    float3 g1 = float3(a0.zw, h.y);
		    float3 g2 = float3(a1.xy, h.z);
		    float3 g3 = float3(a1.zw, h.w);

		    // Normalise gradients
		    float4 norm = taylorInvSqrt(float4(dot(g0, g0), dot(g1, g1), dot(g2, g2), dot(g3, g3)));
		    g0 *= norm.x;
		    g1 *= norm.y;
		    g2 *= norm.z;
		    g3 *= norm.w;

		    // Compute noise and gradient at P
		    float4 m = max(0.6 - float4(dot(x0, x0), dot(x1, x1), dot(x2, x2), dot(x3, x3)), 0.0);
		    float4 m2 = m * m;
		    float4 m3 = m2 * m;
		    float4 m4 = m2 * m2;
		    float3 grad =
		        -6.0 * m3.x * x0 * dot(x0, g0) + m4.x * g0 +
		        -6.0 * m3.y * x1 * dot(x1, g1) + m4.y * g1 +
		        -6.0 * m3.z * x2 * dot(x2, g2) + m4.z * g2 +
		        -6.0 * m3.w * x3 * dot(x3, g3) + m4.w * g3;
		    float4 px = float4(dot(x0, g0), dot(x1, g1), dot(x2, g2), dot(x3, g3));
		    return 42.0 * float4(grad, dot(m4, px));
		}

		sampler2D _MainTex;
		sampler2D _NormalTex;
		sampler2D _EmissionTex;
		struct Input 
		{
			float3 worldPos;
			float2 uv_MainTex;
			float2 uv_EmissionTex;
			float2 uv_NormalTex;
		};


		half _Glossiness;
		half _Metallic;
		//half _DissolvePercentage;
		float _DissolveDistance;
		float _ClipDistance;
		float _DissolvePower;
		float _DissolveScale;
		half _ShowTexture;
		fixed4 _Color;
		fixed4 _InvisColor;
		fixed4 _Emission;
		fixed4 _InvisEmission;
		float _EmissionStrength;
		float _EmissionStrengthDetail;
		float _EmissionStrengthInvis;
		fixed4 _EmissionColor;
		float _EmissionThreshold;
		float _EmissionUpper;
		float _ExternalDistModifier;
		float _ExternalClipModifier;
		float _ExternalDetailEmissionModifier;

		void surf(Input IN, inout SurfaceOutputStandard o)
		{		
			// Albedo comes from a texture tinted by color
			half t = tex2D(_MainTex, IN.uv_MainTex).r;
			half4 texColor = tex2D(_MainTex, IN.uv_MainTex);
			fixed4 alphagrab = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			//half gradient = tex2D(_MainTex, IN.worldPos.rg).r;
			float camDistance = distance(IN.worldPos, _WorldSpaceCameraPos);
			float dissolvePercentage = (camDistance / (_DissolveDistance + _ExternalDistModifier));
			float clipPercentage = (camDistance / (_ClipDistance + _ExternalClipModifier));
			dissolvePercentage = pow(dissolvePercentage, 4);
			clipPercentage = pow (clipPercentage, 4);
			half gradient = snoise(IN.worldPos.rgb) * _DissolveScale;
			gradient += 1;
			float threshold = dissolvePercentage * _DissolvePower;
			float clipThreshold = clipPercentage * _DissolvePower;
			float showColor = gradient - threshold < 0;
			clip(gradient - clipThreshold);

			fixed4 c = lerp(t,gradient, _ShowTexture) * _Color;
			c *= texColor;
			fixed4 emissionGrab = tex2D (_EmissionTex, IN.uv_EmissionTex) * _Emission * (_EmissionStrengthDetail + _ExternalDetailEmissionModifier) * (1-showColor);
			float3 albedoOutput = c.rgb * (1-showColor) + _InvisColor * showColor;
			albedoOutput += emissionGrab.rgb;
			albedoOutput += _InvisEmission.rgb * _EmissionStrengthInvis;
			o.Albedo = albedoOutput;
			//color = (1-useDissolve)*color + useDissolve*_DissolveColor;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a * (1-showColor) + _InvisColor.a * showColor;

			float borderThreshold = dissolvePercentage * _EmissionThreshold;
			float emissionBorder = gradient - borderThreshold < _EmissionUpper;

			//o.Emission = _Emission * (1-showColor) + (_EmissionColor * _EmissionStrength * emissionBorder) * (1-showColor);
			o.Emission = (_EmissionColor * _EmissionStrength * emissionBorder) * (1-showColor);
			o.Normal = UnpackNormal (tex2D (_NormalTex, IN.uv_NormalTex) * (1-showColor));
		}

		ENDCG
	}
		FallBack "Diffuse"
}