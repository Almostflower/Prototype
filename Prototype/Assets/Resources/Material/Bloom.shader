Shader "Custom/Bloom"
{
    Properties
    {
        //_Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture2D", 2D) = "white" {}	// ポストエフェクトがかかる前のレンダリングが入る
        //_Glossiness ("Smoothness", Range(0,1)) = 0.5
        //_Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
		// 追加項目
		CGINCLUDE
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"

        sampler2D _MainTex;
		float4 _MainTex_ST;
		float4 _MainTex_TexelSize;
		sampler2D _SourceTex;
		//float _Threshold;		// 明るさに影響0に近いと明るくなる
		half4 _FilterParams;
		float _Intensity;		// ぼかし

		// 頂点シェーダーに入れるデータ
		struct appdata
		{
			float4 vertex:POSITION;
			float2 uv:TEXCOORD0;
		};

		// ピクセルシェーダーに入れるデータ
		struct v2f
		{
			float2 uv:TEXCOORD0;
			float4 vertex:SV_POSITION;
		};

		// テクスチャの色をuv座標から取り出す
		half3 SampleMain(float2 uv)
		{
			return tex2D(_MainTex, uv).rgb;
		}

		// 対角線上の４点からサンプリングした色の平均値を返す
		half3 SampleBox(float2 uv, float delta)
		{
			float4 offset = _MainTex_TexelSize.xyxy * float2(-delta, delta).xxyy;
			half3 sum = SampleMain(uv + offset.xy) + SampleMain(uv + offset.zy) + SampleMain(uv + offset.xw) + SampleMain(uv + offset.zw);
			return sum * 0.25f;
		}

		// 明度を返す
		half GetBrightness(half3 color)
		{
			return max(color.r, max(color.g, color.b));
		}

		// 頂点シェーダー(映っているそのものの映像を送る)
		v2f vert(appdata v)
		{
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.uv = TRANSFORM_TEX(v.uv, _MainTex);
			return o;
		}

		ENDCG

			cull off
			ZTest Always
			ZWrite off

			Tags{ "RenderType" = "Opaque" }

			// 適用するピクセル抽出用のパス
			Pass
		{
			CGPROGRAM

			//fixed4 frag(v2f i) :SV_Target
			//{
			//	half4 col = 1;
			//	col.rgb = SampleBox(i.uv, 1.0f);
			//	half brightness = GetBrightness(col.rgb);
			//
			//	// 明度がTtresholdより大きいピクセルだけブルームの対象とする
			//	half contribution = max(0, brightness - _Threshold);
			//	contribution /= max(brightness, 0.00001);
			//
			//	return col * contribution;
			//}

			//
			fixed4 frag(v2f i) :SV_Target
			{
				half4 col = 1;
				col.rgb = SampleBox(i.uv, 1.0f);
				half brightness = GetBrightness(col.rgb);

				half soft = brightness - _FilterParams.y;
				soft = clamp(soft, 0, _FilterParams.z);
				soft = soft * soft * _FilterParams.w;
				half contribution = max(soft, brightness - _FilterParams.x);
				contribution /= max(brightness, 0.00001);
				return col * contribution;
			}

			ENDCG

		}

		// 1:ダウンサンプリング用のパス（ぼかし）
		Pass
		{
			CGPROGRAM

			// ダウンサンプリング時には１ピクセル分ずらす
			fixed4 frag(v2f i) : SV_Target
			{
				half4 col = 1;
				col.rgb = SampleBox(i.uv, 1.0f);
				return col;
			}

			ENDCG

		}

		// 2:アップサンプリング用のパス
		Pass
		{
			Blend One One
			CGPROGRAM

			// アップサンプリング時には0.5ピクセル分ずらす
			fixed4 frag(v2f i) :SV_Target
			{
				half4 col = 1;
				col.rgb = SampleBox(i.uv, 0.5f);
				return col;
			}

			ENDCG

		}

		// 3:最後の一回のアップサンプリング用のパス
		Pass
		{
			CGPROGRAM

			fixed4 frag(v2f i) :SV_Target
			{
				half4 col = tex2D(_SourceTex, i.uv);
				col.rgb += SampleBox(i.uv, 0.5f) * _Intensity;
				return col;
			}

			ENDCG

		}

		// 4:デバッグ用
		Pass
		{
			CGPROGRAM

			fixed4 frag(v2f i) :SV_Target
			{
				half4 col = 1;
				col.rgb += SampleBox(i.uv, 0.5f) * _Intensity;
				return col;
			}

			ENDCG

		}
    }
    FallBack "Diffuse"
}
