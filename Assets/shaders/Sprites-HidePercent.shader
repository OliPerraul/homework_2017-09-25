// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Sprites/HidePercent"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
	_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		[HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
		[HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
		[PerRendererData] _AlphaTex("External Alpha", 2D) = "white" {}
	[PerRendererData] _EnableExternalAlpha("Enable External Alpha", Float) = 0

		// Add values to determine if outlining is enabled and outline color.
		[PerRendererData] _HidePercent("Hidden Percentage of the sprite", Float) = 0
		[PerRendererData] _Width("width", Float) = 256
		[PerRendererData] _Height("height", Float) = 32
	}

		SubShader
	{
		Tags
	{
		"Queue" = "Transparent"
		"IgnoreProjector" = "True"
		"RenderType" = "Transparent"
		"PreviewType" = "Plane"
		"CanUseSpriteAtlas" = "True"
	}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma target 2.0
#pragma multi_compile_instancing
#pragma multi_compile _ PIXELSNAP_ON
#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
#include "UnitySprites.cginc"

	float _HidePercent;
	float _Width;
	float _Height;
	float4 _MainTex_TexelSize;
	float4 _MainTex_ST;


	struct _v2f
	{
		half2 texcoord  : TEXCOORD0;
		float3 worldPos : TEXCOORD1;
		float3 localPos : TEXCOORD2;
		float4 vertex   : SV_POSITION;
		fixed4 color : COLOR;
	};



	_v2f vert(appdata_t IN)
	{
		_v2f OUT;

		OUT.vertex = UnityObjectToClipPos(IN.vertex);

		float4 scaleVertex = float4(IN.vertex.xyz, 0); //By setting the last value to 0 it ignores the flipping ( loses relative position if sprite is flipped :( )
		float4 wP = mul(unity_ObjectToWorld, scaleVertex); //Get the object to world vertex and store it
		OUT.worldPos = wP.xyz; //For use in fragment shader
		float4 lP = mul(unity_WorldToObject, scaleVertex); //Get the world to object vertex and store it
		OUT.localPos = lP.xyz; //For use in fragment shader


		OUT.texcoord = TRANSFORM_TEX(IN.texcoord, _MainTex);
		OUT.color = IN.color;
		return OUT;
	}

	fixed4 frag(_v2f IN) : SV_Target
	{

	fixed4 c = tex2D(_MainTex, IN.texcoord)*IN.color;
	//Calculate relative position
	fixed2 relativeWorld = fixed2(IN.worldPos.x + IN.localPos.x, IN.worldPos.y + IN.localPos.y);

	//This becomes the UV for the texture I want to apply to the sprite ( using the sprites width and height )
	fixed2 relativePos = fixed2((relativeWorld.x + _Width), (relativeWorld.y + _Height));

	fixed2 uv;
	uv.x = relativePos.x / _Width;
	uv.y = relativePos.y / _Height;

	uv /= 32;
		
	//actual percentage hidden ??
	float hidden = 1.0 - _HidePercent*2;

	c = c*(1- step(hidden, uv.x));
	c = c*step(1,c.a);

	return c;
	}
		ENDCG
	}



		/*
		step(a,x)
		Implements a step function returning one for each component of x
		that is greater than or equal to the corresponding component in the reference vector a, and zero otherwise
		*/
	}
}
