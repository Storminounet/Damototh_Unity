Shader "Custom/Circle"
{
	Properties
	{
		[HideInInspector] _MainTex("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_Intensity("Intensity", Float) = 1
		_Radius("Circle Radius", Range(0, 0.5)) = 0.5
		_Thickness("Circle Thickness", Range(0, 0.5)) = 0.05
		_Fill("Fill", Range(0, 1)) = 1
	}
	SubShader
	{
		Tags {"Queue" = "Transparent" "RenderType" = "Transparent" }
		LOD 100

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

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
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed _Intensity;
			fixed4 _Color;
			fixed _Thickness;
			fixed _Radius;
			fixed _Fill;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);

				half distanceFromCenter = length((i.uv - (0.5, 0.5)));

				if (distanceFromCenter > _Radius)
					distanceFromCenter = 0;
				else if (distanceFromCenter < _Radius - _Thickness)
					distanceFromCenter = 0;
				else
				{
					distanceFromCenter = 1;

					half angle;
				}

				col *= _Intensity * _Intensity;
				col.a = distanceFromCenter;
				return col * _Color;
			}
			ENDCG
		}
	}
}
