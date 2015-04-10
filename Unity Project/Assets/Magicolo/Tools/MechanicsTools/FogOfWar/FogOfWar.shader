Shader "Custom/Fog Of War" {
	Properties {
		_MainTex ("Main Texture", 2D) = "black" {}
		_AlphaMap ("Alpha Map", 2D) = "black" {}
	}
	
	SubShader {
	    Tags {
	        "Queue"="Transparent"
	        "IgnoreProjector"="True"
	        "RenderType"="Transparent"
	        "PreviewType"="Plane"
	    }
	    
	    Cull Off Lighting Off ZWrite Off Ztest Always Fog { Mode Off }
	    Blend SrcAlpha OneMinusSrcAlpha
	 
	    Pass {    
	        CGPROGRAM
	        #pragma vertex vert
	        #pragma fragment frag
	 
	        #include "UnityCG.cginc"
	 
	        sampler2D _AlphaMap;
	        
	        struct appdata_t {
	            float4 vertex : POSITION;
	            fixed4 color : COLOR;
	            float2 texcoord : TEXCOORD0;
	        };
	 
	        struct v2f {
	            float4 vertex : SV_POSITION;
	            fixed4 color : COLOR;
	            float2 texcoord : TEXCOORD0;
	        };
	 
	        float4 _AlphaMap_ST;
	        
	        v2f vert (appdata_t v)
	        {
	            v2f o;
	            o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
	            o.color = v.color;
	            o.texcoord = TRANSFORM_TEX(v.texcoord, _AlphaMap);
	            return o;
	        }
	 
	        fixed4 frag (v2f i) : SV_Target
	        {
	            fixed4 col;
	            fixed4 tex = tex2D(_AlphaMap, i.texcoord);
	            col.rgb = i.color.rgb * tex.rgb;
	            col.a = tex.a;
	            return col;
	        }
	        ENDCG
	    }
	}     
}